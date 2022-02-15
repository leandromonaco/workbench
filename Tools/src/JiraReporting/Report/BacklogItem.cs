using System;
using System.Text.Json.Serialization;

namespace JiraReporting.Report
{
    public class BacklogItem
    {
        public BacklogItem()
        {
        }


        public string EpicId { get; set; }
        public string EpicTitle { get; set; }
        public string IssueType { get; set; }
        public string Sprint { get; set; }
        public string Status { get; set; }
        public string Points { get; set; }
        public string AssignedTo { get; set; }
        public DateTime Date { get; set; }
        public string IssueId { get; set; }
        public string IssueTitle { get; set; }
        public string Severity { get; set; }

    }
}