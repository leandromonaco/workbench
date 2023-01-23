using System.Text.Json;
using IntegrationConnectors.Common;
using IntegrationConnectors.TeamCity.Converters;
using IntegrationConnectors.TeamCity.Model;

namespace IntegrationConnectors.JIRA
{

    public class TeamCityConnector : HttpConnector
    {
        public TeamCityConnector(string baseUrl, string apiKey, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {
        }

        public async Task<List<TeamCityCodeChange>> GetBuildChanges(int buildId)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new DateTimeConverter());


            var response = await GetAsync($"{_url}/app/rest/changes?locator=build:(id:{buildId})");
            var teamcityCodeChanges = JsonSerializer.Deserialize<TeamCityCodeChanges>(response, options);
            return teamcityCodeChanges?.Changes.ToList();
        }

        public async Task<List<TeamCityBuild>> GetBuilds()
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new DateTimeConverter());


            var response = await GetAsync($"{_url}/app/rest/builds?locator=count:100,start:0");
            var teamcityBuilds = JsonSerializer.Deserialize<TeamCityBuilds>(response, options);
            return teamcityBuilds?.Builds.ToList();
        }

        public async Task<TeamCityCodeChange> GetChange(int changeId)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new DateTimeConverter());

            var response = await GetAsync($"{_url}/app/rest/changes/{changeId}");
            var teamCityCodeChange = JsonSerializer.Deserialize<TeamCityCodeChange>(response, options);
            return teamCityCodeChange;
        }
    }
}
