using JiraReporting.Model;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JiraReporting.Model
{
    public class Fields
    {
        public string Summary { get; set; }
        public Parent Parent { get; set; }
        public Status Status { get; set; }
        public IssueType IssueType { get; set; }
        public Assignee Assignee { get; set; }

        [JsonPropertyName("customfield_10263")]
        public Severity Severity { get; set; }

        [JsonPropertyName("customfield_10018")]
        public List<Sprint> Sprints { get; set; }

        [JsonPropertyName("customfield_10281")]
        public Points Points { get; set; }

        [JsonPropertyName("customfield_10283")]
        public RaidType RaidType { get; set; }
    }
}