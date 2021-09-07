using System;
using System.Collections.Generic;

namespace Webhooks.API.Model.PullRequest
{
    public class AzDevOpsPullRequestResource
    {
        public AzDevOpsRepository Repository { get; set; }
        public int PullRequestId { get; set; }
        public string Status { get; set; }
        public AzDevOpsCreatedBy CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SourceRefName { get; set; }
        public string TargetRefName { get; set; }
        public string MergeStatus { get; set; }
        public List<AzDevOpsReviewer> Reviewers { get; set; }
    }
}