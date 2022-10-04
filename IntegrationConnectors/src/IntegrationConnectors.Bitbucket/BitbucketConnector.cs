using System.Text.Json;
using IntegrationConnectors.Common;

namespace IntegrationConnectors.Bitbucket
{
    public class BitbucketConnector: HttpConnector
    {
        public BitbucketConnector(string baseUrl, string apiKey, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {

        }

        public async Task<List<BitbucketBranch>> GetBranchesAsync(string company, string repository)
        {
            var pageNumber = 1;
            var allBranches = new List<BitbucketBranch>();
            var branches = new BitbucketBranches() { Values = new List<BitbucketBranch>() { new BitbucketBranch() } };

            while (branches.Values.Count > 0)
            {
                var response = await GetAsync($"{_url}/2.0/repositories/{company}/{repository}/refs/branches?page={pageNumber}&pagelen=100");
                branches = JsonSerializer.Deserialize<BitbucketBranches>(response, _jsonSerializerOptions);
                allBranches.AddRange(branches.Values);
                pageNumber++;
            }

            return allBranches.OrderByDescending(b => b.Target.Date).ToList();
        }

        public async Task<List<BitBucketPullRequest>> GetPullRequestsAsync(string company, string repository)
        {
            var pageNumber = 1;
            var allPullRequests = new List<BitBucketPullRequest>();
            var pullRequests = new BitBucketPullRequests() { Values = new List<BitBucketPullRequest>() { new BitBucketPullRequest() } };

            while (pullRequests.Values.Count > 0)
            {
                var response = await GetAsync($"{_url}/2.0/repositories/{company}/{repository}/pullrequests?page={pageNumber}");
                pullRequests = JsonSerializer.Deserialize<BitBucketPullRequests>(response, _jsonSerializerOptions);
                allPullRequests.AddRange(pullRequests.Values);
                pageNumber++;
            }

            return allPullRequests.OrderByDescending(pr => pr.Created).ToList();
        }
    }
}