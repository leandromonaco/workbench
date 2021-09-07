using IntegrationConnectors.AzDevOps.Model.Shared;
using System;
using System.Text.Json.Serialization;

namespace IntegrationConnectors.AzDevOps.Model.Build
{
    public class AzDevOpsBuild
    {
        public string Id { get; set; }
        public string Status { get; set; }
        [JsonPropertyName("result")]
        public string SubStatus { get; set; }
        public DateTime QueueTime { get; set; }
        public string Parameters { get; set; }
        public AzDevOpsBuildLog Logs { get; set; }
        public AzDevOpsUser RequestedBy { get; set; }
    }
}
