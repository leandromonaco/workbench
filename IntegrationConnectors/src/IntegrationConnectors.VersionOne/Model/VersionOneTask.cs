using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntegrationConnectors.VersionOne.Model
{
    public class VersionOneTask
    {
        public string Number { get; set; }
        [JsonPropertyName("Owners.Email")]
        public List<string> OwnersEmail { get; set; }
        [JsonPropertyName("Status.Name")]
        public string Status { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("Parent.Number")]
        public string ParentNumber { get; set; }
        [JsonPropertyName("Parent.Name")]
        public string ParentName { get; set; }
        [JsonPropertyName("Parent.Order")]
        public long ParentOrder { get; set; }
        [JsonPropertyName("AssetState")]
        public VersionOneAssetState State { get; set; }
        public DateTime ChangeDate { get; set; }
        [JsonPropertyName("_oid")]
        public string Url { get; set; }
    }
}
