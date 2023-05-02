namespace IntegrationConnectors.JIRA.Model
{
    public class JiraChangelog
    {
        public int StartAt { get; set; }
        public int MaxResults { get; set; }
        public int Total { get; set; }
        public JiraHistory[] Histories { get; set; }
    }
}