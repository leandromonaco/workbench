using System;

namespace IntegrationConnectors.AzDevOps.Model.Commit
{
    public class AzDevOpsCommitAuthor
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}