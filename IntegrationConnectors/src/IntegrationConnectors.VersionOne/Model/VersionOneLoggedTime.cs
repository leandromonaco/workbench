using System;
using System.Text.Json.Serialization;

namespace IntegrationConnectors.VersionOne.Model
{
    public class VersionOneLoggedTime
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        [JsonPropertyName("Workitem.Number")]
        public string WorkitemNumber { get; set; }
        [JsonPropertyName("Workitem.Parent.Number")]
        public string WorkitemParentNumber { get; set; }
        [JsonPropertyName("Workitem.Parent.Super.Number")]
        public string WorkitemSuperNumber { get; set; }
        [JsonPropertyName("Workitem.Parent.Super.Name")]
        public string WorkitemSuperName { get; set; }
        [JsonPropertyName("Member.Email")]
        public string MemberEmail { get; set; }
        [JsonPropertyName("Member.Username")]
        public string MemberUsername { get; set; }
        [JsonPropertyName("Timebox.Schedule.Name")]
        public string TimeboxScheduleName { get; set; }
        [JsonPropertyName("Workitem.Parent.Scope.Name")]
        public string WorkitemParentScopeName { get; set; }
    }
}