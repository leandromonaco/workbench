using System;
using Webhooks.API.Model.PullRequest;

namespace Webhooks.API.Model.Code
{
    public class AzDevOpsCodePushEvent
    {
        public string EventType { get; set; }
        public AzDevOpsMessage Message { get; set; }
        public AzDevOpsDetailedMessage DetailedMessage { get; set; }
        public AzDevOpsCodePushResource Resource { get; set; }


        public DateTime CreatedDate { get; set; }
    }
}