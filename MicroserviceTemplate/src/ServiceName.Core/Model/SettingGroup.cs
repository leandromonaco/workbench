using System.Text.Json.Serialization;

namespace ServiceName.Core.Model
{
    public class SettingGroup
    {
        [JsonPropertyName("IsSettingAEnabled")]
        public bool IsSettingAEnabled { get; set; }

        [JsonPropertyName("IsSettingBEnabled")]
        public bool IsSettingBEnabled { get; set; }
    }
}