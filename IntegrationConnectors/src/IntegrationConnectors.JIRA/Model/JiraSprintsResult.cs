namespace IntegrationConnectors.JIRA.Model
{
    internal class JiraSprintsResult
    {
        public bool IsLast { get; set; }
        public List<JiraSprint> Values { get; set; }
    }
}
