using IntegrationConnectors.SonaType;
using IntegrationConnectors.Common;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace IntegrationConnectors.Test
{
    /// <summary>
    /// https://ossindex.sonatype.org/doc/rest
    /// </summary>
    public class SonaTypeTest
    {
        IConfiguration _configuration;
        SonaTypeConnector _sonaTypeTestRepository;
        WireMockServer _httpMockServer;

        public SonaTypeTest()
        {
            _configuration = new ConfigurationBuilder()
                                        //.SetBasePath(outputPath)
                                        .AddJsonFile("appsettings.json", optional: true)
                                        .AddUserSecrets("cf9af699-d3c2-4090-8231-fd3a1cb45a5f")
                                        .AddEnvironmentVariables()
                                        .Build();

            _httpMockServer = WireMockServer.Start();

            _httpMockServer.Given(Request.Create().WithPath("/v3/component-report").UsingGet())
                           .RespondWith(Response.Create()
                                                .WithStatusCode(200)
                                                .WithBody(@"{ ""msg"": ""Hello world!"" }")
                                       );

            _sonaTypeTestRepository = new SonaTypeConnector($"{_httpMockServer.Urls[0]}/v3/component-report", _configuration["SonaType:Key"], AuthenticationType.Bearer);

        }

        [Fact]
        async Task ScanComponent()
        {
            var scanResult = await _sonaTypeTestRepository.ScanComponent("{ 	\"coordinates\": [	\"pkg:nuget/jQuery@3.4.1\" 	] }");
        }
    }
}
