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
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[ShutdownDotNetAfterServerBuild]
internal class Build : NukeBuild
{
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, string> _versions;
    private readonly List<FileInfo> _testProjects;
    private readonly List<FileInfo> _outputProjects;
    private readonly List<FileInfo> _allProjects;
    private readonly List<AbsolutePath> _allCleanUpDirectories;
    private readonly List<string> _projectFilter;

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
        _projectFilter = _configuration.GetSection("ReleaseableProjects").Get<List<string>>();
        _testProjects = TestDirectory.GlobFiles("**/*.csproj").Select(p => new FileInfo(p)).ToList();
        _allCleanUpDirectories = SolutionDirectory.GlobDirectories("**/bin", "**/obj", "**/output").Where(d => !d.Parent.Name.Equals("build")).ToList();
        _allProjects = SolutionDirectory.GlobFiles("**/*.csproj").Where(f => !f.Parent.Name.Equals("build")).Select(p => new FileInfo(p)).ToList();
        _outputProjects = _allProjects.Where(p => _projectFilter.Contains(p.Name)).ToList();
    }

    private Target Clean => _ => _
        .Executes(() =>
        {
            _allCleanUpDirectories.ForEach(DeleteDirectory);
        });

    private Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            foreach (var project in _allProjects)
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
        foreach (var project in _allProjects)
        {
            DotNetTasks.DotNetBuild(o => o.SetProjectFile(project.FullName)
                                          .SetConfiguration(Configuration));
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
            var packageId = csProject.Name.Replace(csProject.Extension, string.Empty);

            OctopusTasks.OctopusPack(o => o.SetBasePath($"{csProject.Directory}\\{_configuration["ReleaseFolder"]}")
                                         .SetOutputFolder(OutputDirectory)
                                         .SetId(packageId)
                                         .SetVersion(_versions.GetValueOrDefault(csProject.Name)));


            //Use this for libraries
            //<PropertyGroup>
            //  <IsPackable>true</IsPackable>
            //</PropertyGroup>
            //DotNetTasks.DotNetPack(o => o.EnableNoBuild()
            //                             .EnableNoRestore()
            //                             .SetConfiguration(Configuration)
            //                             .SetPackageId(packageId)
            //                             .SetVersion(_versions.GetValueOrDefault(csProject.Name))
            //                             .SetProcessWorkingDirectory(csProject.Directory.FullName)
            //                             .SetProject(csProject.FullName)
            //                             .SetOutputDirectory(OutputDirectory)
            //                             );

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