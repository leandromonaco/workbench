using System.Text.Json.Serialization;

namespace ServiceName.Core.Model
{
    public class SettingGroup
    {
        [JsonPropertyName("Setting1")]
        public string Setting1 { get; set; }
        [JsonPropertyName("Setting2")]
        public string Setting2 { get; set; }
    }
}