namespace Webhooks.API.Model.Code
{
    public class AzDevOpsCommit
    {
        public string CommitId { get; set; }
        public AzDevOpsCommitAuthor Author { get; set; }
        public AzDevOpsCommitAuthor Committer { get; set; }
        public string Comment { get; set; }

    }
}