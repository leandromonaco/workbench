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

            var response = await GetAsync($"{_url}/2.0/repositories/{company}/{repository}/refs/branches?page={pageNumber}&pagelen=100");
            var branches = JsonSerializer.Deserialize<BitbucketBranches>(response, _jsonSerializerOptions);
            allBranches.AddRange(branches.Values);

            while (branches.Values.Count > 0)
            {
                pageNumber++;

                response = await GetAsync($"{_url}/2.0/repositories/{company}/{repository}/refs/branches?page={pageNumber}&pagelen=100");
                branches = JsonSerializer.Deserialize<BitbucketBranches>(response, _jsonSerializerOptions);
                allBranches.AddRange(branches.Values);
            }

            return allBranches.OrderByDescending(b => b.Target.Date).ToList();
        }

        public async Task<BitBucketPullRequests> GetPullRequestsAsync(string company, string repository)
        {
            var response = await GetAsync($"{_url}/2.0/repositories/{company}/{repository}/pullrequests?page=1");
            var pullRequests = JsonSerializer.Deserialize<BitBucketPullRequests>(response, _jsonSerializerOptions);
            return pullRequests;
        }
    }
}