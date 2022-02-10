using CommandLine;
using Hangfire;
using Hangfire.MemoryStorage;
using IntegrationConnectors.Common;
using JiraReporting.Model;
using JiraReporting.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReporting
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();

            Parser.Default.ParseArguments<Options>(args)
               .WithParsed(o =>
               {
                   RecurringJob.AddOrUpdate("JiraReportJob",
                                           () => ExecuteJob(o.JiraEndpoint, o.JiraProject, o.JiraUsername, o.JiraAuthenticationToken, o.PowerBiDatasetEndpoint),
                                           Cron.Minutely);
               });

            using var server = new BackgroundJobServer();
            Console.ReadLine();
        }

        public static void ExecuteJob(string jiraEndpoint, string jiraProject, string jiraUsername, string jiraAuthenticationToken, string powerBiDatasetEndpoint)
        {
            Console.WriteLine($"Jira Report started {DateTime.Now}");

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{jiraUsername}:{jiraAuthenticationToken}"));

            HttpConnector httpConnector = new("", credentials, AuthenticationType.Basic);

            var _jsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                MaxDepth = 0
            };

            var backlogItems = new List<BacklogItem>();

            var increment = 100;
            var startAt = 0;
            var finishAt = 1;

            while (startAt <= finishAt)
            {
                var query = $@"{{
                                ""jql"": ""project={jiraProject}"",
                                ""maxResults"": {increment},
                                ""startAt"": {startAt}
                            }}";

                var response = httpConnector.PostAsync($"{jiraEndpoint}/rest/api/2/search", query).Result;
                var jqlQueryResult = JsonSerializer.Deserialize<JqlQueryResult>(response, _jsonSerializerOptions);

                startAt += increment;
                finishAt = jqlQueryResult.Total - 1;

                foreach (var issue in jqlQueryResult.Issues)
                {
                    var row = new BacklogItem
                    {
                        Date = DateTime.Now.Date,
                        JiraId = issue.Key,
                        Sprint = issue.Fields.Sprints?.OrderByDescending(s => s.StartDate).FirstOrDefault().Name,
                        JiraDescription = issue.Fields.Summary,
                        Epic = issue.Fields.Parent?.Fields.Summary,
                        IssueType = issue.Fields.IssueType.Name != "RAID" ? issue.Fields.IssueType.Name : $"{issue.Fields.IssueType.Name} - {issue.Fields.RaidType?.Value}",
                        Severity = issue.Fields.Severity?.Value,
                        Status = issue.Fields.Status.Name,
                        Points = issue.Fields.Points?.Value,
                        AssignedTo = issue.Fields.Assignee == null ? "Unassigned" : issue.Fields.Assignee.DisplayName
                    };

                    backlogItems.Add(row);
                }
            }

            var outputFile = $"{Environment.CurrentDirectory}\\report_{DateTime.Now.Date.ToString("yyyy-MM-dd")}";

            Helper.ExportExcel(backlogItems, null, $"{outputFile}.xlsx");

            File.WriteAllText($"{outputFile}.json", JsonSerializer.Serialize(backlogItems));

            var result = httpConnector.PostAsync(powerBiDatasetEndpoint, JsonSerializer.Serialize(backlogItems)).Result;

            Console.WriteLine($"Jira Report stopped {DateTime.Now}");
        }
    }
}
