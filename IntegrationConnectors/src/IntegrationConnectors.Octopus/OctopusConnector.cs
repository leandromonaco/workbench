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

        public async Task<List<OctopusMachine>> GetMachinesAsync(string environmentId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/environments/{environmentId}/machines");
            var machines = JsonSerializer.Deserialize<OctopusMachineResponse>(response, _jsonSerializerOptions);
            return machines.Items;
        }

        public async Task<List<OctopusDeployment>> GetDeploymentsAsync(string machineId)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/machines/{machineId}/tasks");
            var deployments = JsonSerializer.Deserialize<OctopusDeploymentResponse>(response, _jsonSerializerOptions);
            return deployments.Items;
        }

        public async Task<OctopusProject> GetProjectAsync(string project)
        {
            var response = await GetAsync($"{_url}/{_spaceId}/projects/{project}");
            var proj = JsonSerializer.Deserialize<OctopusProject>(response, _jsonSerializerOptions);
            return proj;
        }

        public async Task<OctopusVariableSet> GetVariableSetAsync(string ownerId)
        {
            var response = await GetAsync($"{_url}/LibraryVariableSets/{ownerId}");
            var octopusVariableSet = JsonSerializer.Deserialize<OctopusVariableSet>(response, _jsonSerializerOptions);
            return octopusVariableSet;
        }

        public async Task<List<OctopusVariableSet>> GetVariableSetsAsync(List<string> variableSets)
        {
            var octopusVariableSets = new List<OctopusVariableSet>();
            foreach (var variableSet in variableSets)
            {
                //Spaces-1/variables/
                var response = await GetAsync($"{_url}/{_spaceId}/variables/{variableSet}");
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
    }
}