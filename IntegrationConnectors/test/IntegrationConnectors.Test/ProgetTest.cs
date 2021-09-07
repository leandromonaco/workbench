using IntegrationConnectors.Common;
using IntegrationConnectors.Proget;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationConnectors.Test
{
    public class ProgetTest
    {
        IConfiguration _configuration;
        ProgetConnector _progetRepository;

        public ProgetTest()
        {
            _configuration = new ConfigurationBuilder()
                                        //.SetBasePath(outputPath)
                                        .AddJsonFile("appsettings.json", optional: true)
                                        .AddUserSecrets("cf9af699-d3c2-4090-8231-fd3a1cb45a5f")
                                        .AddEnvironmentVariables()
                                        .Build();

            _progetRepository = new ProgetConnector(_configuration["Proget:Url"], _configuration["Proget:Key"], AuthenticationType.Proget);
        }

        [Fact]
        async Task GetLastDeployment()
        {
            var packages = await _progetRepository.GetPromotionsAsync(_configuration["Proget:SourceFeed"], _configuration["Proget:TargetFeed"]);
        }
    }
}
