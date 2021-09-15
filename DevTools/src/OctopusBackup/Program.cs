using CommandLine;
using IntegrationConnectors.Common;
using IntegrationConnectors.Octopus;
using IntegrationConnectors.Octopus.Model;
using OctopusBackup;
using OctopusBackup.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


var apiUrl = string.Empty;
var apiKey = string.Empty;
var space = string.Empty;

var jsonDisplay = string.Empty;
var output = string.Empty;
var outputFile = string.Empty;

Parser.Default.ParseArguments<Options>(args)
       .WithParsed(o =>
       {
           apiUrl = o.ApiUrl;
           apiKey = o.ApiKey;
           space = o.Space;
       });

var _octopusRepository = new OctopusConnector(apiUrl, apiKey, space, AuthenticationType.Octopus);

var octopusProjectGroups = (await _octopusRepository.GetProjectGroupsAsync()).Items;
var octopusProjects = (await _octopusRepository.GetProjectsAsync()).Items;
var octopusMachines = (await _octopusRepository.GetMachinesAsync()).Items;
var octopusEnvironments = (await _octopusRepository.GetEnvironmentsAsync()).Items;
var octopusLibraryVariableSets = (await _octopusRepository.GetLibraryVariableSetsAsync()).Items;
var octopusCertificates = (await _octopusRepository.GetCertificatesAsync()).Items;

//Backup Space
var octopusSpaceBackup = new OctopusSpaceBackup();
octopusSpaceBackup.Environments = octopusEnvironments;
octopusSpaceBackup.Machines = octopusMachines;
octopusSpaceBackup.Certificates = octopusCertificates;
octopusSpaceBackup.LibraryVariableSets = octopusLibraryVariableSets;
octopusSpaceBackup.Projects = octopusProjects;

output = $"{space}.json";
jsonDisplay = System.Text.Json.JsonSerializer.Serialize(octopusSpaceBackup, new JsonSerializerOptions() { WriteIndented = true });
outputFile = Path.Combine(Directory.GetCurrentDirectory(), output);
File.WriteAllText(outputFile, jsonDisplay);


//Backup Processes and Project Variables
foreach (var project in octopusProjects)
{
    var projectGroupDirectoryName = octopusProjectGroups.Where(pg => pg.Id.Equals(project.ProjectGroupId)).FirstOrDefault().Name;
    var projectGroupDirectory = Directory.CreateDirectory($"{space}\\{projectGroupDirectoryName}");
    output = $"{project.Name}.json";
    var projectBackup = new OctopusProjectBackup();
    //Backup Project Process
    
    var deploymentProcess = await _octopusRepository.GetDeploymentProcessAsync(project.DeploymentProcessId);

    projectBackup.Process = JsonDocument.Parse(deploymentProcess);

    //Backup Project Variables
    var variableBackup = new OctopusVariableBackup
    {
        ProjectVariables = await _octopusRepository.GetVariableSetAsync(project.VariableSetId),
        LibrarySets = new List<OctopusVariableSet>()
    };

    foreach (var libraryVariableSetId in project.IncludedLibraryVariableSetIds)
    {
        var librarySet = await _octopusRepository.GetLibraryVariableSetAsync(libraryVariableSetId);
        variableBackup.LibrarySets.Add(librarySet);
    }

    projectBackup.Variables = variableBackup;

    jsonDisplay = System.Text.Json.JsonSerializer.Serialize(projectBackup, new JsonSerializerOptions() { WriteIndented = true });
    outputFile = Path.Combine(projectGroupDirectory.FullName, output);
    File.WriteAllText(outputFile, jsonDisplay);
}

Console.WriteLine($"Files saved in {Directory.GetCurrentDirectory()}");