namespace IntegrationConnectors.JIRA.Model
{
    public class JiraIssue
    {
        public JiraFields Fields { get; set; }
        public string Key { get; set; }
        public JiraChangelog Changelog { get; set; }
    }
}