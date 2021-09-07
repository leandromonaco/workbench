using System;
using System.Text.Json.Serialization;

namespace IntegrationConnectors.Fortify.Model
{
    public class FortifyIssue
    {
        public string IssueInstanceId { get; set; }
        public string IssueName { get; set; }
        public string PrimaryLocation { get; set; }
        public int LineNumber { get; set; }
        public string Kingdom { get; set; }
        //[JsonPropertyName("friority")] //workaround
        public string friority { get; set; }
        public string IssueStatus { get; set; }
        public string ScanStatus { get; set; }
        public DateTime FoundDate { get; set; }
        public DateTime? RemovedDate { get; set; }
        public string PrimaryTag { get; set; }
        public string EngineType { get; set; }
        public bool Removed { get; set; }
        public bool Suppressed { get; set; }
        public bool Hidden { get; set; }
        public bool HasComments { get; set; }
        public bool HasAttachments { get; set; }
        public double Impact { get; set; }
        public double Likelihood { get; set; }
        public double Severity { get; set; }
        public double Confidence { get; set; }
    }
}