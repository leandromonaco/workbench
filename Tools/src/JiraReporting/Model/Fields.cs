using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JiraReport
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

    }
}