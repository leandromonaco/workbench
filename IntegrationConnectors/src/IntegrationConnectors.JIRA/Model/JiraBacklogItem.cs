namespace IntegrationConnectors.JIRA.Model
{
    public class JiraBacklogItem
    {
        public string EpicId { get; set; }
        public string EpicTitle { get; set; }
        public string IssueType { get; set; }
        public string Sprint { get; set; }
        public string Status { get; set; }
        public int Points { get; set; }
        public string AssignedTo { get; set; }
        public DateTime Date { get; set; }
        public string IssueId { get; set; }
        public string IssueTitle { get; set; }
        public string Priority { get; set; }
        public string FixVersion { get; set; }
    }
}