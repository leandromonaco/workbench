using IntegrationConnectors.Common;
using IntegrationConnectors.Octopus;
using IntegrationConnectors.Octopus.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationConnectors.Test
{
    public class OctopusTest
    {
        IConfiguration _configuration;
        OctopusConnector _octopusRepository;

        public OctopusTest()
        {
            _configuration = new ConfigurationBuilder()
                                        //.SetBasePath(outputPath)
                                        .AddJsonFile("appsettings.json", optional: true)
                                        .AddUserSecrets("cf9af699-d3c2-4090-8231-fd3a1cb45a5f")
                                        .AddEnvironmentVariables()
                                        .Build();

            _octopusRepository = new OctopusConnector(_configuration["Octopus:Url"], _configuration["Octopus:Key"], _configuration["Octopus:SpaceId"], AuthenticationType.Octopus);
        }

        //[Fact]
        //async Task GetLastDeployment()
        //{
        //    var environments = await _octopusRepository.GetEnvironmentsAsync();
        //    foreach (var environment in environments.Items)
        //    {
        //        environment.Machines = await _octopusRepository.GetMachinesAsync(environment.Id);
        //        foreach (var machine in environment.Machines)
        //        {
        //            machine.Deployments = await _octopusRepository.GetDeploymentsAsync(machine.Id);
        //            var machineLastDeployment = machine.Deployments.OrderByDescending(d => d.CompletedTime).FirstOrDefault()?.CompletedTime;
        //            if (machineLastDeployment > environment.LastDeploymentDate)
        //            {
        //                environment.LastDeploymentDate = machineLastDeployment.Value;
        //            }
        //        }

        //    }
        //}

        [Fact]
        async Task GetVariables()
        {
            var octopusLibraryVariableSets = (await _octopusRepository.GetLibraryVariableSetsAsync()).Items;
        }

        [Fact]
        async Task GetProcess()
        {
            var sourceDeploymentProcessId = (await _octopusRepository.GetProjectAsync(_configuration["Octopus:TestProject"])).DeploymentProcessId;
            var sourceDeploymentProcess = await _octopusRepository.GetDeploymentProcessAsync(sourceDeploymentProcessId);
        }
    }
}
