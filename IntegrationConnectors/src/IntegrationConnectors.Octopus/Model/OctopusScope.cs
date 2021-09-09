using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusScope
    {
        [JsonPropertyName("Environment")]
        public List<string> Environments { get; set; }
        [JsonPropertyName("Machine")]
        public List<string> Machines { get; set; }
        [JsonPropertyName("Action")]
        public List<string> Actions { get; set; }
        [JsonPropertyName("Role")]
        public List<string> Roles { get; set; }
        [JsonPropertyName("Channel")]
        public List<string> Channels { get; set; }
    }
}