using System;
using System.Collections.Generic;
using IntegrationConnectors.AzDevOps.Model.Repository;
using IntegrationConnectors.AzDevOps.Model.Shared;

namespace IntegrationConnectors.AzDevOps.Model.PullRequest
{
    public class AzDevOpsPullRequest
    {
        public AzDevOpsCodeRepository Repository { get; set; }
        public string PullRequestId { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string SourceRefName { get; set; }
        public string TargetRefName { get; set; }
        public AzDevOpsUser CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public List<AzDevOpsPullRequestReviewer> Reviewers { get; set; }
        public List<AzDevOpsPullRequestThread> Threads { get; set; }
        public List<AzDevOpsPullRequestComment> Comments { get; set; }
        public List<AzDevOpsPullRequestComment> VoteDates { get; set; }
    }
}

