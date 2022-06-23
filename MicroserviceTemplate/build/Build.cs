using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NerdbankGitVersioning;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.Octopus;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    readonly IConfiguration _configuration;
    readonly Dictionary<string, string> _versions;
    readonly List<FileInfo> _csProjects;
    readonly List<string> _releaseableProjects;


    public static int Main() => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = Configuration.Release;

    [Parameter] string OctopusServerUrl;
    [Parameter] string OctopusApiKey;
    [Parameter] string OctopusSpaceId;

    [Solution] readonly Solution Solution;
    AbsolutePath SolutionDirectory => RootDirectory.Parent;
    AbsolutePath SourceDirectory => SolutionDirectory / "src";
    AbsolutePath TestDirectory => SolutionDirectory / "test";
    AbsolutePath ToolDirectory => SolutionDirectory / "tools";
    AbsolutePath OutputDirectory => SolutionDirectory / "output";

    public Build()
    {
        _configuration = new ConfigurationBuilder()
                            .SetBasePath(Environment.CurrentDirectory)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

        _versions = new Dictionary<string, string>();
        _releaseableProjects = _configuration.GetSection("ReleaseableProjects").Get<List<string>>();
        _csProjects = SolutionDirectory.GlobFiles("**/*.csproj").Where(p => _releaseableProjects.Contains(new FileInfo(p).Name)).Select(p => new FileInfo(p)).ToList(); 
    }

    Target Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            ToolDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            NuGetTasks.NuGetRestore(s => s.SetTargetPath(SolutionDirectory));
        });

    Target Versioning => _ => _
    .DependsOn(Restore)
          .Executes(() =>
          {
              foreach (var csProject in _csProjects)
              {
                  var versionResult = NerdbankGitVersioningTasks.NerdbankGitVersioningGetVersion(v => v.SetProcessWorkingDirectory(csProject.DirectoryName).SetProcessArgumentConfigurator(a => a.Add("-f json"))).Result;
                  NerdbankGitVersioningTasks.NerdbankGitVersioningSetVersion(v => v.SetProject(csProject.DirectoryName)
                                                                                   .SetVersion(versionResult.Version)
                                                                                   );
                  _versions.Add(csProject.Name, versionResult.Version);
              }
          });

    Target Compile => _ => _
    .DependsOn(Versioning)
    .Executes(() =>
    {
        MSBuildTasks.MSBuild(s => s
            .SetTargetPath(SolutionDirectory)
            .SetConfiguration(Configuration)
            );
    });

    Target Pack => _ => _
    .DependsOn(Compile)
    .Executes(() =>
    {
        foreach (var csProject in _csProjects)
        {
            var packageId = csProject.Name.Replace(".csproj", string.Empty);
            var sourcePath = $"{csProject.DirectoryName}\\bin";
            var targetPath = $"{OutputDirectory}\\{packageId}";

            var directories = Directory.GetDirectories(sourcePath, "*net*", SearchOption.AllDirectories).ToList();

            if (directories.Count > 0)
            {
                sourcePath = directories.FirstOrDefault();
            }

            CopyDirectoryRecursively(sourcePath, targetPath);

            OctopusTasks.OctopusPack(o => o.SetBasePath(targetPath)
                                           .SetOutputFolder(OutputDirectory)
                                           .SetId(packageId)
                                           .SetVersion(_versions.GetValueOrDefault(csProject.Name)));

            DeleteDirectory(targetPath);
        }
    });

    //Target Push => _ => _
    //          .DependsOn(Pack)
    //          .Executes(() =>
    //          {
    //              var packages = SolutionDirectory.GlobFiles("**/output/*.nupkg");
    //              foreach (var package in packages)
    //              {
    //                  //if the package exists the default behaviour is to reject the package
    //                  OctopusTasks.OctopusPush(o => o.SetServer(OctopusServerUrl)
    //                                                 .SetApiKey(OctopusApiKey)
    //                                                 .SetSpace(OctopusSpaceId)
    //                                                 .SetPackage(package));
    //              }

    //          });
}
