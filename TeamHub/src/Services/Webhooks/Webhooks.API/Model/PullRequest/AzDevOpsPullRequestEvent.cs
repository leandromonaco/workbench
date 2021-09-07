using System;

namespace Webhooks.API.Model.PullRequest
{
    public class AzDevOpsPullRequestEvent
    {
        public string EventType { get; set; }
        public AzDevOpsMessage Message { get; set; }
        public AzDevOpsDetailedMessage DetailedMessage { get; set; }
        public AzDevOpsPullRequestResource Resource { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}