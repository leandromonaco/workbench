using IntegrationConnectors.Common;
using IntegrationConnectors.Octopus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


var _octopusRepository = new OctopusConnector("https://octopusserver/api/", "API-123", "Spaces-1", AuthenticationType.Octopus);

var octopusProject = await _octopusRepository.GetProjectAsync("project-name");

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

var octopusVariableSets = _octopusRepository.GetVariableSetsAsync(allVariableSetsRoutes).Result;

var services = new List<Tuple<string, string>>();
octopusVariableSets.ForEach(vs => services.AddRange(vs.Variables.Select(v => new Tuple<string, string>(v.Name, v.Value))));
services = services.OrderBy(s => s.Item1).ToList();

var json = JsonSerializer.Serialize(services);

await File.WriteAllTextAsync("C:\\Temp\\Variables.json", json);