using System.Text.Json.Serialization;

namespace IntegrationConnectors.Bitbucket
{
    public class BitBucketPullRequest
    {
        public string Title { get; set; }
        [JsonPropertyName("created_on")]
        public DateTime Created { get; set; }
        public BitBucketLinks Links { get; set; }
    }
}