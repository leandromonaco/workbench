using System.Text.Json.Serialization;

namespace IntegrationConnectors.TeamCity.Model
{
    public class TeamCityBuilds
    {
        public int Count { get; set; }
        public string Href { get; set; }
        [JsonPropertyName("build")]
        public TeamCityBuild[] Builds { get; set; }
    }
}