
namespace IntegrationConnectors.AzDevOps.Model.PullRequest
{
    public class AzDevOpsPullRequestReviewer
    {
        public string UniqueName { get; set; }
        public string DisplayName { get; set; }
        public int Vote { get; set; }
        public bool IsRequired { get; set; }
    }
}