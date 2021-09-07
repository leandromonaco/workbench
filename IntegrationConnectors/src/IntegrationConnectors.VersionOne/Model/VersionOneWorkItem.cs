using System;
using System.Text.Json.Serialization;

namespace IntegrationConnectors.VersionOne.Model
{
    public class VersionOneWorkItem
    {

        [JsonPropertyName("Super.Number")]
        public string SuperNumber { get; set; }
        [JsonPropertyName("Super.Name")]
        public string SuperName { get; set; }
        [JsonPropertyName("Super.Order")]
        public long SuperOrder { get; set; }
        [JsonPropertyName("Super.Team.Name")]
        public string SuperTeamName { get; set; }
        [JsonPropertyName("Super.Scope.Name")]
        public string SuperScopeName { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public double? Estimate { get; set; }
        [JsonPropertyName("Status.Name")]
        public string Status { get; set; }
        [JsonPropertyName("Team.Name")]
        public string Team { get; set; }
        [JsonPropertyName("Timebox.Name")]
        public string Sprint { get; set; }
        public long Order { get; set; }
        [JsonPropertyName("AssetState")]
        public VersionOneAssetState State { get; set; }
        [JsonPropertyName("ResolutionReason.Name")]
        public object Custom1 { get; set; }
        [JsonPropertyName("Custom_Severity.Name")]
        public object Custom2 { get; set; }
        [JsonPropertyName("_oid")]
        public string Url { get; set; }
        public DateTime ChangeDate { get; set; }
        [JsonPropertyName("CreatedBy.Email")]
        public string CreatedByEmail { get; set; }
        [JsonPropertyName("Super.Priority.Name")]
        public string SuperPriorityName { get; set; }
    }
}
