using System.Collections.Generic;

namespace JiraReport
{
    internal class JqlQueryResult
    {
        public int StartAt { get; set; }
        public int MaxResults { get; set; }
        public int Total { get; set; }
        public List<Issue> Issues { get; set; }
    }
}