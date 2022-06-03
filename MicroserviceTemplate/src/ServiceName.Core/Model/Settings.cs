using System.Text.Json.Serialization;

namespace ServiceName.Core.Model
{
    public class Settings
    {
        [JsonPropertyName("Group1")]
        public SettingGroup Group1 { get; set; }
    }
}