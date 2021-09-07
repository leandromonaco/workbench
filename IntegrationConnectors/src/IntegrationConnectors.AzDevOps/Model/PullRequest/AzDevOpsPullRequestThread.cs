using System;
using System.Collections.Generic;

namespace IntegrationConnectors.AzDevOps.Model.PullRequest
{
    public class AzDevOpsPullRequestThread
    {
        public int Id { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public List<AzDevOpsPullRequestComment> Comments { get; set; }
    }
}