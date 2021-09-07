using IntegrationConnectors.Common;
using IntegrationConnectors.Fortify;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationConnectors.Test
{
    public class FortifyTest
    {
        IConfiguration _configuration;
        FortifyConnector _fortifyTestRepository;
        FortifyConnector _fortifyTestRepository2;

        public FortifyTest()
        {
            _configuration = new ConfigurationBuilder()
                                        //.SetBasePath(outputPath)
                                        .AddJsonFile("appsettings.json", optional: true)
                                        .AddUserSecrets("cf9af699-d3c2-4090-8231-fd3a1cb45a5f")
                                        .AddEnvironmentVariables()
                                        .Build();

            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_configuration["Fortify:User"]}:{_configuration["Fortify:Password"]}"));

            _fortifyTestRepository = new FortifyConnector(_configuration["Fortify:Url"], key, AuthenticationType.Basic);
        }

        [Fact]
        async Task RunReport()
        {
            var unifiedLoginToken = await _fortifyTestRepository.GetUnifiedLoginTokenAsync();

            _fortifyTestRepository2 = new FortifyConnector(_configuration["Fortify:Url"], unifiedLoginToken, AuthenticationType.Fortify);

            var projects = await _fortifyTestRepository2.GetProjectsAsync();

            foreach (var project in projects)
            {
                var versions = await _fortifyTestRepository2.GetProjectVersionsAsync(project.Id);
                foreach (var projectVersion in versions.Where(v => v.Name.Equals("master")))
                {
                    var issues = await _fortifyTestRepository2.GetIssuesAsync(projectVersion.Id);
                    foreach (var issue in issues)
                    {
                        var issueDetails = await _fortifyTestRepository2.GetIssueDetailsAsync(project.Name, projectVersion.Name, issue.IssueInstanceId);
                    }
                }
            }
        }
    }
}
