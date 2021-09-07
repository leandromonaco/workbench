using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
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
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;


[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{

    public static int Main() => Execute<Build>(x => x.Push);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    //readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
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

    Dictionary<string, string> versions = new Dictionary<string, string>();

    Target Versioning => _ => _
    .DependsOn(Restore)
          .Executes(() =>
          {

              var projectFiles = SolutionDirectory.GlobFiles("**/*.csproj");
              foreach (var projectFile in projectFiles)
              {
                  FileInfo fileInfo = new FileInfo(projectFile);
                  var versionResult = NerdbankGitVersioningTasks.NerdbankGitVersioningGetVersion(v => v.SetProcessWorkingDirectory(projectFile.Parent).SetProcessArgumentConfigurator(a => a.Add("-f json"))).Result;
                  NerdbankGitVersioningTasks.NerdbankGitVersioningSetVersion(v => v.SetProject(projectFile.Parent)
                                                                                   .SetVersion(versionResult.Version)
                                                                                   );
                  versions.Add(fileInfo.Name, versionResult.Version);
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
        DeploymentList deploymentList;

        FileInfo buildAssembly = new FileInfo(Assembly.GetExecutingAssembly().Location);
        using (StreamReader r = new StreamReader($"{buildAssembly.Directory}\\Deployment\\deployment_list.json"))
        {
            string json = r.ReadToEnd();
            deploymentList = JsonSerializer.Deserialize<DeploymentList>(json);
        }

        var projectFiles = SolutionDirectory.GlobFiles("**/*.csproj");
        foreach (var projectFile in projectFiles)
        {
            FileInfo fileInfo = new FileInfo(projectFile);

            //Check in deployment list to see if the project is deployable
            if (deploymentList?.Applications.Count(a => a.Equals(fileInfo.Name)) > 0)
            {
                var packageId = fileInfo.Name.Replace(".csproj", string.Empty);
                var sourcePath = $"{projectFile.Parent}\\bin";
                var targetPath = $"{OutputDirectory}\\{packageId}";

                //if it's a .NET Core Project, override source path
                var directories = new List<string>();
                var netCoreDirectories = Directory.GetDirectories(sourcePath, "*net*", SearchOption.AllDirectories).ToArray();
                var netFrameworkDirectories = Directory.GetDirectories(sourcePath, "*release*", SearchOption.AllDirectories).ToArray();
                directories.AddRange(netCoreDirectories);
                directories.AddRange(netFrameworkDirectories);
                if (directories.Count > 0)
                {
                    sourcePath = directories.FirstOrDefault();
                }

                CopyDirectoryRecursively(sourcePath, targetPath);

                OctopusTasks.OctopusPack(o => o.SetBasePath(targetPath)
                                               .SetOutputFolder(OutputDirectory)
                                               .SetId(packageId)
                                               .SetVersion(versions.GetValueOrDefault(fileInfo.Name)));

                DeleteDirectory(targetPath);
            }
        }




    });

    Target Push => _ => _
              .DependsOn(Pack)
              .Executes(() =>
              {
                  var packages = SolutionDirectory.GlobFiles("**/output/*.nupkg");
                  foreach (var package in packages)
                  {
                      //if the package exists the default behaviour is to reject the package
                      OctopusTasks.OctopusPush(o => o.SetServer(OctopusServerUrl)
                                                     .SetApiKey(OctopusApiKey)
                                                     .SetSpace(OctopusSpaceId)
                                                     .SetPackage(package));
                  }

              });
}
