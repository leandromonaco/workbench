using System;

namespace JiraReport
{
    internal class BacklogReportRow
    {
        public BacklogReportRow()
        {
        }

        public string Epic { get; internal set; }
        public string IssueType { get; internal set; }
        public string Sprint { get; internal set; }
        public string Status { get; internal set; }
        public string Points { get; internal set; }
        public string AssignedTo { get; internal set; }
        public DateTime Date { get; internal set; }
        public string JiraId { get; internal set; }
        public string JiraDescription { get; internal set; }
        public string Severity { get; internal set; }

    }
}