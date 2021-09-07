using System;
using System.Text.Json.Serialization;

namespace IntegrationConnectors.VersionOne.Model
{
    public class VersionOneIssue
    {
        public string Number { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("Team.Name")]
        public string Team { get; set; }
        [JsonPropertyName("Category.Name")]
        public string Category { get; set; }
        public string IdentifiedBy { get; set; }
        [JsonPropertyName("Owner.Name")]
        public string Owner { get; set; }
        public string Resolution { get; set; }
        [JsonPropertyName("ResolutionReason.Name")]
        public string ResolutionReason { get; set; }
        public DateTime ChangeDate { get; set; }
        public VersionOneAssetState AssetState { get; set; }
        [JsonPropertyName("_oid")]
        public string Url { get; set; }
        [JsonPropertyName("CreatedBy.Email")]
        public string CreatedByEmail { get; set; }
    }
}
