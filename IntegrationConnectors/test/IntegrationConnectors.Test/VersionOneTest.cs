using IntegrationConnectors.VersionOne;
using IntegrationConnectors.Common;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationConnectors.Test
{
    public class VersionOneTest
    {
        IConfiguration _configuration;
        VersionOneConnector _v1Repository;

        public VersionOneTest()
        {
            _configuration = new ConfigurationBuilder()
                                        //.SetBasePath(outputPath)
                                        .AddJsonFile("appsettings.json", optional: true)
                                        .AddUserSecrets("cf9af699-d3c2-4090-8231-fd3a1cb45a5f")
                                        .AddEnvironmentVariables()
                                        .Build();

            _v1Repository = new VersionOneConnector(_configuration["VersionOne:Url"], _configuration["VersionOne:Key"], AuthenticationType.Bearer);
        }

        [Fact]
        async Task TestMethods()
        {
            var results = await _v1Repository.RetrieveDefectsByTeamAsync("teamName");
        }


       
    }
}
