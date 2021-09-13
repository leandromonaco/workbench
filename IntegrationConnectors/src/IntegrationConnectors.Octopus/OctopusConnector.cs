using IntegrationConnectors.Common;
using IntegrationConnectors.Octopus.Model;
using System.Collections.Generic;
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

        public async Task<List<OctopusEnvironment>> GetEnvironmentsAsync()
        {
            var response = await GetAsync($"{_url}/{_spaceId}/environments?skip=0&take=2147483647");
            var envs = JsonSerializer.Deserialize<OctopusEnvironmentResponse>(response, _jsonSerializerOptions);
            return envs.Items;
        }

        public async Task<OctopusEnvironment> GetEnvironmentAsync(string environmentId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/environments/{environmentId}");
            var environment = JsonSerializer.Deserialize<OctopusEnvironment>(response, _jsonSerializerOptions);
            return environment;
        }

        public async Task<List<OctopusMachine>> GetMachinesAsync(string environmentId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/environments/{environmentId}/machines");
            var machines = JsonSerializer.Deserialize<OctopusMachineResponse>(response, _jsonSerializerOptions);
            return machines.Items;
        }

        public async Task<OctopusMachine> GetMachineAsync(string machineId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/machines/{machineId}");
            var machine = JsonSerializer.Deserialize<OctopusMachine>(response, _jsonSerializerOptions);
            return machine;
        }

        public async Task<List<OctopusDeployment>> GetDeploymentsAsync(string machineId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/machines/{machineId}/tasks");
            var deployments = JsonSerializer.Deserialize<OctopusDeploymentResponse>(response, _jsonSerializerOptions);
            return deployments.Items;
        }

        public async Task<OctopusProjects> GetProjectsAsync()
        {
            var response = await GetAsync($"{_url}/{_spaceId}/projects?take=1000");
            var proj = JsonSerializer.Deserialize<OctopusProjects>(response, _jsonSerializerOptions);
            return proj;
        }

        public async Task<OctopusProject> GetProjectAsync(string project)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/projects/{project}");
            var proj = JsonSerializer.Deserialize<OctopusProject>(response, _jsonSerializerOptions);
            return proj;
        }

        public async Task<OctopusVariableSets> GetVariableSetsAsync()
        {
            var response = await GetAsync($"{_url}/{_spaceId}/LibraryVariableSets?take=1000");
            var octopusVariableSet = JsonSerializer.Deserialize<OctopusVariableSets>(response, _jsonSerializerOptions);
            octopusVariableSet.Items.ForEach(vs => vs.Variables = GetVariableSetAsync(vs.VariableSetId).Result.Variables);
            return octopusVariableSet;
        }

        public async Task<OctopusVariableSet> GetVariableSetAsync(string variableSetId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/variables/{variableSetId}");
            var octopusVariableSet = JsonSerializer.Deserialize<OctopusVariableSet>(response, _jsonSerializerOptions);
            return octopusVariableSet;
        }

        public async Task<List<OctopusVariableSet>> GetVariableSetsAsync(List<string> variableSetIds)
        {
            var octopusVariableSets = new List<OctopusVariableSet>();
            foreach (var variableSetId in variableSetIds)
            {
                //Spaces-1/variables/
                var response = await GetAsync($"{_url}/{_spaceId}/variables/{variableSetId}");
                var octopusVariableSet = JsonSerializer.Deserialize<OctopusVariableSet>(response, _jsonSerializerOptions);
                octopusVariableSets.Add(octopusVariableSet);
            }

            return octopusVariableSets;
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
            var response = await GetAsync($"{_url}/{_spaceId}/certificates?take=1000");
            var certificates = JsonSerializer.Deserialize<OctopusCertificates>(response);
            return certificates;
        }
    }
}