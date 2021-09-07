namespace IntegrationConnectors.AzDevOps.Model.Commit
{
    public class AzDevOpsCommit
    {
        public string CommitId { get; set; }
        public AzDevOpsCommitAuthor Author { get; set; }
        public string Comment { get; set; }
        public AzDevOpsCommitChangeCounts CommitChangeCounts { get; set; }
        public AzDevOpsPush Push { get; set; }
    }
}
