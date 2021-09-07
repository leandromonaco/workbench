using System.Text.Json.Serialization;

namespace IntegrationConnectors.VersionOne.Model
{
    public class VersionOneFeature
    {
        public string Number { get; set; }
        [JsonPropertyName("Priority.Name")]
        public string Priority { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("Status.Name")]
        public string Status { get; set; }
        public string Team { get; set; }
        [JsonPropertyName("Scope.Name")]
        public string Scope { get; set; }
        public VersionOneAssetState State { get; set; }
        public long Order { get; set; }
        [JsonPropertyName("_oid")]
        public string Url { get; set; }
    }
}
