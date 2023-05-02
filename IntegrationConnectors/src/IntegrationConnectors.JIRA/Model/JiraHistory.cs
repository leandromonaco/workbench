using System.Data.SqlTypes;

namespace IntegrationConnectors.JIRA.Model
{
    public class JiraHistory
    {
        public string Id { get; set; }
        public JiraAuthor Author { get; set; }
        public string Created { get; set; }
        public JiraItem[] Items { get; set; }
    }
}