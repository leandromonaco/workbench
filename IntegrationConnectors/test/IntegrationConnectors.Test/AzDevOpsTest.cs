using IntegrationConnectors.AzDevOps;
using IntegrationConnectors.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationConnectors.Test
{

    public class AzDevOpsTest
    {
        IConfiguration _configuration;
        AzDevOpsConnector _azDevOpsTestRepository;

        public AzDevOpsTest()
        {
            _configuration = new ConfigurationBuilder()
                                        //.SetBasePath(outputPath)
                                        .AddJsonFile("appsettings.json", optional: true)
                                        .AddUserSecrets("cf9af699-d3c2-4090-8231-fd3a1cb45a5f")
                                        .AddEnvironmentVariables()
                                        .Build();

            _azDevOpsTestRepository = new AzDevOpsConnector(_configuration["AzDevOps:Url"], _configuration["AzDevOps:Key"], AuthenticationType.DefaultCredentials);
        }

        [Fact]
        async Task GetRepository()
        {
            //var repository = await _azDevOpsTestRepository.GetRepositoryAsync("Misc");
            //var branches = await _azDevOpsTestRepository.GetBranchesAsync(repository.Id);
            //var commits = await _azDevOpsTestRepository.GetCommitsAsync(repository.Id, "master", new DateTime(2020,1,1), new DateTime(2020, 12, 31));

            //var pendingPRs = await _azDevOpsTestRepository.GetPullRequestsAsync(repository.Id, false);
            //var completePRs = await _azDevOpsTestRepository.GetPullRequestsAsync(repository.Id, true);
            //var prThreads = await _azDevOpsTestRepository.GetPullRequestThreadsAsync(repository.Id, completePRs[0].PullRequestId);

            var buildDefinitions = await _azDevOpsTestRepository.GetBuildDefinitionsAsync(true, true);

                //var usedBuildDefinitions = buildDefinitions.Where(bd => bd.LatestBuild!=null && bd.LatestBuild.QueueTime.Year.Equals(2020)).ToList();
                //var abandonedBuildDefinitions = buildDefinitions.Where(bd => bd.LatestBuild == null || !bd.LatestBuild.QueueTime.Year.Equals(2020)).ToList();

            //var tests = await _azDevOpsTestRepository.GetTestRunsAsync(true);
            var builds = await _azDevOpsTestRepository.GetBuildsAsync();
            var build = await _azDevOpsTestRepository.GetBuildAsync(builds[0].Id);
        }

        [Fact]
        async Task GetBranches()
        {
            var repository = await _azDevOpsTestRepository.GetRepositoryAsync("Misc");
            var branches = await _azDevOpsTestRepository.GetBranchesAsync(repository.Id);
        }

        [Fact]
        async Task GetPullRequests()
        {
            var completePRs = await _azDevOpsTestRepository.GetPullRequestsAsync(true, 1000); //limit is 1000
            var completePRs2020 = completePRs.Where(pr => pr.CreationDate.Date.Year.Equals(2020)).ToList();
        }

        [Fact]
        async Task GetPullRequestThreads()
        {
            var pullRequests = await _azDevOpsTestRepository.GetPullRequestsAsync(false, 1000); //limit is 1000

            foreach (var pr in pullRequests)
            {
                pr.Threads = await _azDevOpsTestRepository.GetPullRequestThreadsAsync(pr.Repository.Id, pr.PullRequestId);
                pr.Threads = pr.Threads.Where(t => t.Comments.Count(c => c.CommentType.Equals("text")) > 0).ToList();
                //pr.Threads = pr.Threads.Where(t => t.Status.Equals("active")).ToList();
            }
        }
   

        [Fact]
        async Task GetCommits()
        {
            var repositoryNames = new List<string>() {"Misc"};
            foreach (var repoName in repositoryNames)
            {
                var repository = await _azDevOpsTestRepository.GetRepositoryAsync(repoName);
                var commits = await _azDevOpsTestRepository.GetCommitsAsync(repository.Id, "master", new DateTime(2020, 1, 1), new DateTime(2020, 12, 31), 1000);
            }
        }


        [Fact]
        async Task QueueBuild()
        {
            var build = await _azDevOpsTestRepository.QueueBuildAsync("Build Definition Name", "main", true);
        }
    }
}
