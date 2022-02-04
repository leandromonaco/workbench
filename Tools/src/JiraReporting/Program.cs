using Ardalis.GuardClauses;
using CommandLine;
using IntegrationConnectors.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReport
{
    class Program
    {
        static void Main(string[] args)
        {
            string jiraEndpoint = string.Empty;
            string jiraProject = string.Empty;
            string jiraUsername = string.Empty;
            string jiraAuthenticationToken = string.Empty;
            string powerBiDatasetEndpoint = string.Empty;

            Parser.Default.ParseArguments<Options>(args)
               .WithParsed(o =>
               {
                   jiraEndpoint = Guard.Against.NullOrEmpty(o.JiraEndpoint, nameof(o.JiraEndpoint));
                   jiraProject = Guard.Against.NullOrEmpty(o.JiraProject, nameof(o.JiraProject));
                   jiraUsername = Guard.Against.NullOrEmpty(o.JiraUsername, nameof(o.JiraUsername));
                   jiraAuthenticationToken = Guard.Against.NullOrEmpty(o.JiraAuthenticationToken, nameof(o.JiraAuthenticationToken));
                   powerBiDatasetEndpoint = Guard.Against.NullOrEmpty(o.PowerBiDatasetEndpoint, nameof(o.PowerBiDatasetEndpoint));
               });

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{jiraUsername}:{jiraAuthenticationToken}"));

            HttpConnector httpConnector = new("", credentials, AuthenticationType.Basic);

            var _jsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                MaxDepth = 0
            };

            var report = new List<BacklogReportRow>();

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
                    var row = new BacklogReportRow
                    {
                        Date = DateTime.Now.Date,
                        JiraId = issue.Key,
                        Sprint = issue.Fields.Sprints?.OrderByDescending(s => s.StartDate).FirstOrDefault().Name,
                        JiraDescription = issue.Fields.Summary,
                        Epic = issue.Fields.Parent?.Fields.Summary,
                        IssueType = issue.Fields.IssueType.Name,
                        Severity = issue.Fields.Severity?.Value,
                        Status = issue.Fields.Status.Name,
                        Points = issue.Fields.Points?.Value,
                        AssignedTo = issue.Fields.Assignee == null ? "Unassigned" : issue.Fields.Assignee.DisplayName
                    };

                    report.Add(row);
                }
            }

            Helper.ExportExcel(report);

            File.WriteAllText($"{Environment.CurrentDirectory}\\report_{DateTime.Now.Date.ToString("yyyy-MM-dd")}.json", JsonSerializer.Serialize(report));

            var result = httpConnector.PostAsync(powerBiDatasetEndpoint, JsonSerializer.Serialize(report)).Result;
        }
    }
}
