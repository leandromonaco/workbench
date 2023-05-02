namespace IntegrationConnectors.JIRA.Model
{
    public class JiraItem
    {
        public string Field { get; set; }
        public string Fieldtype { get; set; }
        public string FieldId { get; set; }
        public string From { get; set; }
        public string FromString { get; set; }
        public string To { get; set; }
        public string ToString { get; set; }
        public object TmpFromAccountId { get; set; }
        public string TmpToAccountId { get; set; }
    }
}