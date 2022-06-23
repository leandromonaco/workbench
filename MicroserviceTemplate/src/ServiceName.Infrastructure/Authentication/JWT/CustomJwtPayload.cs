using System.Text.Json.Serialization;

namespace ServiceName.Infrastructure.Authentication.JWT
{
    internal class CustomJwtPayload
    {
        [JsonPropertyName("exp")]
        public long ExpirationTime { get; set; }

        [JsonPropertyName("iat")]
        public long IssuedAt { get; set; }

        [JsonPropertyName("iss")]
        public string Issuer { get; set; }

        [JsonPropertyName("aud")]
        public string Audience { get; set; }

        [JsonPropertyName("sub")]
        public string Subject { get; set; }

        [JsonPropertyName("custom:tenantId")]
        public string TenantId { get; set; }

        [JsonPropertyName("custom:username")]
        public string Username { get; set; }
    }
}