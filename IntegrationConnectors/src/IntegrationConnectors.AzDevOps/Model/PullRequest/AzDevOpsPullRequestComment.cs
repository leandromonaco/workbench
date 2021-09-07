using IntegrationConnectors.AzDevOps.Model.Shared;
using System;

namespace IntegrationConnectors.AzDevOps.Model.PullRequest
{
    public class AzDevOpsPullRequestComment
    {
        public int Id { get; set; }
        public int ParentCommentId { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime LastContentUpdatedDate { get; set; }
        public string CommentType { get; set; }
        public string Content { get; set; }
        public AzDevOpsUser Author { get; set; }
    }
}