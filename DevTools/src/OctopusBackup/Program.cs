using CommandLine;
using IntegrationConnectors.Common;
using IntegrationConnectors.Octopus;
using IntegrationConnectors.Octopus.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OctopusBackup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


var apiUrl = string.Empty;
var apiKey = string.Empty;
var space = string.Empty;
var project = string.Empty;
var display = string.Empty;
var output = string.Empty;

Parser.Default.ParseArguments<Options>(args)
       .WithParsed(o =>
       {
           apiUrl = o.ApiUrl;
           apiKey = o.ApiKey;
           space = o.Space;
           project = o.Project;
           display = o.Display;
       });

var _octopusRepository = new OctopusConnector(apiUrl, apiKey, space, AuthenticationType.Octopus);

var jsonDisplay = string.Empty;
var environments = await _octopusRepository.GetEnvironmentsAsync();

switch (display)
{
    case "process":
        output = $"{space}_{project}_process.json";
        var octopusProject1 = await _octopusRepository.GetProjectAsync(project);
        var deploymentProcess = await _octopusRepository.GetDeploymentProcessAsync(octopusProject1.DeploymentProcessId);
        jsonDisplay = JValue.Parse(deploymentProcess).ToString(Formatting.Indented);
        break;

    case "variables":
        output = $"{space}_{project}_variables.json";
        var octopusProject = await _octopusRepository.GetProjectAsync(project);

        var variableSets = octopusProject.IncludedLibraryVariableSetIds;

        var projectVariableSetId = octopusProject.VariableSetId;

        var allVariableSetsRoutes = new List<string>();

        allVariableSetsRoutes.Add(projectVariableSetId);

        foreach (var variableSet in variableSets)
        {
            allVariableSetsRoutes.Add($"variableset-{variableSet}");
        }

        //Remove Duplicates
        allVariableSetsRoutes = allVariableSetsRoutes.Distinct().ToList();

        var octopusVariableSets = await _octopusRepository.GetVariableSetsAsync(allVariableSetsRoutes);

        var variablesOutput = new List<OctopusVariable>();
        octopusVariableSets.ForEach(vs => variablesOutput.AddRange(vs.Variables));
        variablesOutput = variablesOutput.OrderBy(s => s.Name).ToList();

        foreach (var v in variablesOutput)
        {
            if (v.Scope.Machines!=null)
            {
                var machineNames = new List<string>();
                foreach (var m in v.Scope.Machines)
                {
                    var machineName = (await _octopusRepository.GetMachineAsync(m)).Name;
                    machineNames.Add(machineName);
                }
                //replace machine IDs with their names
                v.Scope.Machines = machineNames;
            }


            if (v.Scope.Environments != null)
            {
                var environmentNames = new List<string>();
                foreach (var e in v.Scope.Environments)
                {
                    var environmentName = (await _octopusRepository.GetEnvironmentAsync(e)).Name;
                    environmentNames.Add(environmentName);
                }
                //replace environment IDs with their names
                v.Scope.Environments = environmentNames;
            }
        }

        //Helper.ExportExcel(variablesOutput);
        jsonDisplay = System.Text.Json.JsonSerializer.Serialize(variablesOutput, new JsonSerializerOptions() { WriteIndented = true }); ;
        break;
    case "environments":
        output = $"{space}_environments.json";
        jsonDisplay = System.Text.Json.JsonSerializer.Serialize(environments, new JsonSerializerOptions() { WriteIndented = true }); ;
        break;
    case "roles":
        output = $"{space}_roles.json";
        var roles = new List<string>();
        foreach (var environment in environments)
        {
            var machines = await _octopusRepository.GetMachinesAsync(environment.Id);
            machines.ForEach(m => roles.AddRange(m.Roles));
        }

        jsonDisplay = System.Text.Json.JsonSerializer.Serialize(roles.Distinct().OrderBy(r => r), new JsonSerializerOptions() { WriteIndented = true });
        break;
    case "machines":
        output = $"{space}_machines.json";
        var machinesDetails = new Dictionary<string, List<OctopusMachine>>();
        
        foreach (var environment in environments)
        {
            var machines = await _octopusRepository.GetMachinesAsync(environment.Id);
            machines.ForEach(async m => m.LastDeployment = (await _octopusRepository.GetDeploymentsAsync(m.Id)).OrderByDescending(d=>d.StartTime).FirstOrDefault());
            machinesDetails.Add(environment.Name, machines);
        }

        jsonDisplay = System.Text.Json.JsonSerializer.Serialize(machinesDetails, new JsonSerializerOptions() { WriteIndented = true }); ;
        break;
    case "certificates":
        output = $"{space}_certificates.json";
        var certificates = await _octopusRepository.GetCertificatesAsync();
        jsonDisplay = System.Text.Json.JsonSerializer.Serialize(certificates, new JsonSerializerOptions() { WriteIndented = true }); ;
        break;
    default:
        Console.WriteLine("Error: See Display Options using --help");
        break;
}

var outputFile = Path.Combine(Directory.GetCurrentDirectory(), output);
File.WriteAllText(outputFile, jsonDisplay);
Console.WriteLine($"File saved: {outputFile}");