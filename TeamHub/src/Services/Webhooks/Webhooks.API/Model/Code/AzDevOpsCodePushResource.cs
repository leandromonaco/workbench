using System;
using System.Collections.Generic;
using Webhooks.API.Model.PullRequest;

namespace Webhooks.API.Model.Code
{
    public class AzDevOpsCodePushResource
    {
        public List<AzDevOpsCommit> Commits { get; set; }
        public List<AzDevOpsRefUpdates> RefUpdates { get; set; }
        public AzDevOpsRepository Repository { get; set; }
        public AzDevOpsPushedBy PushedBy { get; set; }
        public int PushId { get; set; }
        public DateTime Date { get; set; }
    }
}