namespace Webhooks.API.Model.PullRequest
{
    public class AzDevOpsRepository
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AzDevOpsProject Project { get; set; }
    }
}