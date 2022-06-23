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
internal class Build : NukeBuild
{
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, string> _versions;
    private readonly List<FileInfo> _srcProjects;
    private readonly List<FileInfo> _testProjects;
    private readonly List<FileInfo> _outputProjects;
    private readonly List<string> _releaseableProjects;

    public static int Main() => Execute<Build>(x => x.Push);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration Configuration = Configuration.Release;

    [Parameter] private string OctopusServerUrl;
    [Parameter] private string OctopusApiKey;
    [Parameter] private string OctopusSpaceId;

    [Solution] private readonly Solution Solution;
    private AbsolutePath SolutionDirectory => RootDirectory.Parent;
    private AbsolutePath SourceDirectory => SolutionDirectory / "src";
    private AbsolutePath TestDirectory => SolutionDirectory / "tests";
    private AbsolutePath ToolDirectory => SolutionDirectory / "tools";
    private AbsolutePath OutputDirectory => SolutionDirectory / "output";

    public Build()
    {
        _configuration = new ConfigurationBuilder()
                            .SetBasePath(Environment.CurrentDirectory)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

        _versions = new Dictionary<string, string>();
        _releaseableProjects = _configuration.GetSection("ReleaseableProjects").Get<List<string>>();
        _srcProjects = SourceDirectory.GlobFiles("**/*.csproj").Select(p => new FileInfo(p)).ToList();
        _testProjects = TestDirectory.GlobFiles("**/*.csproj").Select(p => new FileInfo(p)).ToList();
        _outputProjects = _srcProjects.Where(p => _releaseableProjects.Contains(p.Name)).ToList();
    }

    private Target Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            ToolDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    private Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            foreach (var project in _srcProjects)
            {
                DotNetTasks.DotNetRestore(o => o.SetProjectFile(project.FullName));
            }
        });

    private Target Versioning => _ => _
    .DependsOn(Restore)
          .Executes(() =>
          {
              foreach (var csProject in _outputProjects)
              {
                  var versionResult = NerdbankGitVersioningTasks.NerdbankGitVersioningGetVersion(v => v.SetProcessWorkingDirectory(csProject.DirectoryName).SetProcessArgumentConfigurator(a => a.Add("-f json"))).Result;
                  NerdbankGitVersioningTasks.NerdbankGitVersioningSetVersion(v => v.SetProject(csProject.DirectoryName)
                                                                                   .SetVersion(versionResult.Version)
                                                                                   );
                  _versions.Add(csProject.Name, versionResult.Version);
              }
          });

    private Target Compile => _ => _
    .DependsOn(Versioning)
    .Executes(() =>
    {
        foreach (var project in _srcProjects)
        {
            DotNetTasks.DotNetBuild(o => o.SetProjectFile(project.FullName));
        }
    });

    private Target Test => _ => _
      .DependsOn(Compile)
      .Executes(() =>
      {
          foreach (var project in _testProjects)
          {
              DotNetTasks.DotNetTest(o => o.SetProjectFile(project.FullName));
          }
      });

    private Target Pack => _ => _
    .DependsOn(Test)
    .Executes(() =>
    {
        foreach (var csProject in _outputProjects)
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

    private Target Push => _ => _
              .DependsOn(Pack)
              .Executes(() =>
              {
                  var outputPackages = OutputDirectory.GlobFiles("**/*.nupkg").ToList();
                  foreach (var package in outputPackages)
                  {
                      //if the package exists the default behaviour is to reject the package
                      OctopusTasks.OctopusPush(o => o.SetServer(OctopusServerUrl)
                                                     .SetApiKey(OctopusApiKey)
                                                     .SetSpace(OctopusSpaceId)
                                                     .SetPackage(package));
                  }
              });
}