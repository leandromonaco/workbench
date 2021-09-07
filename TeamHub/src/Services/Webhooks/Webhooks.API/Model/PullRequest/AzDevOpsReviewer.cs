namespace Webhooks.API.Model.PullRequest
{
    public class AzDevOpsReviewer
    {
        public int Vote { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UniqueName { get; set; }
    }
}