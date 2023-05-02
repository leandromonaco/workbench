namespace IntegrationConnectors.JIRA.Model
{
    public class JiraAuthor
    {
        public string Self { get; set; }
        public string AccountId { get; set; }
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
        public string TimeZone { get; set; }
        public string AccountType { get; set; }
    }
}