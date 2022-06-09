using System.Text.Json.Serialization;

namespace ServiceName.Core.Model
{
    public class Settings
    {
        [JsonPropertyName("CategoryA")]
        public SettingGroup CategoryA { get; set; }
    }
}