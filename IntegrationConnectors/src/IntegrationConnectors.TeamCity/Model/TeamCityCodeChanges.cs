using System.Text.Json.Serialization;

namespace IntegrationConnectors.TeamCity.Model
{
    public class TeamCityCodeChanges
    {
        [JsonPropertyName("change")]
        public TeamCityCodeChange[] Changes { get; set; }
        public string Href { get; set; }
        public int Count { get; set; }
    }
}