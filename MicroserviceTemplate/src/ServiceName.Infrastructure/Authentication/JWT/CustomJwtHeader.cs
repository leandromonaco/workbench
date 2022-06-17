using System.Text.Json.Serialization;

namespace ServiceName.Infrastructure.Authentication.JWT
{
    internal class CustomJwtHeader
    {
        [JsonPropertyName("kid")]
        public string KeyId { get; set; }
        [JsonPropertyName("alg")]
        public string Algorithm { get; set; }
    }
}
