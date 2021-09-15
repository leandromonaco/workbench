using IntegrationConnectors.Common;
using IntegrationConnectors.Octopus.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntegrationConnectors.Octopus
{
    public class OctopusConnector : HttpConnector
    {
        private string _spaceId;

        public OctopusConnector(string baseUrl, string apiKey, string spaceId, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {
            _spaceId = spaceId;
        }

        public async Task<OctopusEnvironments> GetEnvironmentsAsync()
        {
            var response = await GetAsync($"{_url}/{_spaceId}/environments?take=200000");
            var envs = JsonSerializer.Deserialize<OctopusEnvironments>(response, _jsonSerializerOptions);
            return envs;
        }

        public async Task<OctopusEnvironment> GetEnvironmentAsync(string environmentId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/environments/{environmentId}");
            var environment = JsonSerializer.Deserialize<OctopusEnvironment>(response, _jsonSerializerOptions);
            return environment;
        }

        public async Task<OctopusMachines> GetMachinesAsync(string environmentId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/environments/{environmentId}/machines");
            var machines = JsonSerializer.Deserialize<OctopusMachines>(response, _jsonSerializerOptions);
            return machines;
        }

        public async Task<OctopusMachine> GetMachineAsync(string machineId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/machines/{machineId}");
            var machine = JsonSerializer.Deserialize<OctopusMachine>(response, _jsonSerializerOptions);
            return machine;
        }

        public async Task<OctopusMachines> GetMachinesAsync()
        {
            var response = await GetAsync($"{_url}/{_spaceId}/machines?take=200000");
            var machine = JsonSerializer.Deserialize<OctopusMachines>(response, _jsonSerializerOptions);
            return machine;
        }

        public async Task<List<OctopusDeployment>> GetDeploymentsAsync(string machineId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/machines/{machineId}/tasks");
            var deployments = JsonSerializer.Deserialize<OctopusDeployments>(response, _jsonSerializerOptions);
            return deployments.Items;
        }

        public async Task<OctopusProjectGroups> GetProjectGroupsAsync()
        {
            var response = await GetAsync($"{_url}/{_spaceId}/projectgroups?take=200000");
            var projects = JsonSerializer.Deserialize<OctopusProjectGroups>(response, _jsonSerializerOptions);
            return projects;
        }

        public async Task<OctopusProjects> GetProjectsAsync()
        {
            var response = await GetAsync($"{_url}/{_spaceId}/projects?take=200000");
            var projects = JsonSerializer.Deserialize<OctopusProjects>(response, _jsonSerializerOptions);
            return projects;
        }

        public async Task<OctopusProject> GetProjectAsync(string project)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/projects/{project}");
            var proj = JsonSerializer.Deserialize<OctopusProject>(response, _jsonSerializerOptions);
            return proj;
        }

        public async Task<OctopusVariableSets> GetLibraryVariableSetsAsync()
        {
            var response = await GetAsync($"{_url}/{_spaceId}/LibraryVariableSets?take=200000");
            var octopusVariableSet = JsonSerializer.Deserialize<OctopusVariableSets>(response, _jsonSerializerOptions);
            foreach (var item in octopusVariableSet.Items)
            {
                item.Variables = (await GetVariableSetAsync(item.VariableSetId)).Variables;
            }
            return octopusVariableSet;
        }

        public async Task<OctopusVariableSet> GetLibraryVariableSetAsync(string id)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/LibraryVariableSets/{id}");
            var octopusVariableSet = JsonSerializer.Deserialize<OctopusVariableSet>(response, _jsonSerializerOptions);
            octopusVariableSet.Variables = (await GetVariableSetAsync(octopusVariableSet.VariableSetId)).Variables;
            return octopusVariableSet;
        }

        public async Task<OctopusVariableSet> GetVariableSetAsync(string variableSetId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/variables/{variableSetId}");
            var octopusVariableSet = JsonSerializer.Deserialize<OctopusVariableSet>(response, _jsonSerializerOptions);
            octopusVariableSet.Variables = octopusVariableSet.Variables.OrderBy(v => v.Name).ToList();
            return octopusVariableSet;
        }

        public async Task<string> GetDeploymentProcessAsync(string deploymentProcessId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/deploymentprocesses/{deploymentProcessId}");
            return response;
        }

        public async Task UpdateDeploymentProcessAsync(string targetDeploymentProcessId, string jsonContent)
        {
            await PutAsync($"{_url}/{_spaceId}/deploymentprocesses/{targetDeploymentProcessId}", jsonContent);
        }

        public async Task<OctopusCertificates> GetCertificatesAsync()
        {
            var response = await GetAsync($"{_url}/{_spaceId}/certificates?take=200000");
            var certificates = JsonSerializer.Deserialize<OctopusCertificates>(response);
            return certificates;
        }
    }
}