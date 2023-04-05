using IntegrationConnectors.Common;
using IntegrationConnectors.JIRA.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IntegrationConnectors.JIRA
{

    public class JiraConnector : HttpConnector
    {
        public JiraConnector(string baseUrl, string user, string key) : base(baseUrl, user, key)
        {
        }

        public async Task<List<JiraBacklogItem>> GetBacklogItemsAsync(string jql)
        {
            var latestBacklog = new List<JiraBacklogItem>();

            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                //MaxDepth = 1
            };

            var increment = 100;
            var startAt = 0;
            var finishAt = 1;

            while (startAt <= finishAt)
            {
                var query = $@"{{
                                ""jql"": ""{jql}"",
                                ""maxResults"": {increment},
                                ""startAt"": {startAt}
                            }}";

                var response = await PostAsync($"{_url}/rest/api/2/search", query);

                var jqlQueryResult = JsonSerializer.Deserialize<JiraQueryResult>(response, jsonSerializerOptions);

                Console.WriteLine($"Processing {startAt} of {jqlQueryResult.Total} {DateTime.Now}");

                startAt += increment;
                finishAt = jqlQueryResult.Total - 1;

                foreach (var issue in jqlQueryResult.Issues)
                {
                    var issueType = issue.Fields.IssueType.Name;

                    var row = new JiraBacklogItem
                    {
                        Date = DateTime.Now.Date,
                        Sprint = issue.Fields.Sprints?.OrderByDescending(s => s.StartDate).FirstOrDefault().Name,
                        IssueId = issue.Key,
                        IssueTitle = issue.Fields.Summary,
                        EpicId = issue.Fields.Parent?.Key,
                        EpicTitle = issue.Fields.Parent?.Fields.Summary,
                        IssueType = issueType,
                        Priority = issue.Fields.Priority?.Value,
                        Status = issue.Fields.Status.Name,
                        Points = Convert.ToInt32(issue.Fields.Points),
                        AssignedTo = issue.Fields.Assignee == null ? "Unassigned" : issue.Fields.Assignee.DisplayName,
                        FixVersion = issue.Fields.FixVersions.LastOrDefault()?.Name
                    };

                    latestBacklog.Add(row);
                }
            }

            return latestBacklog;
        }

        public async Task<List<JiraSprint>> GetSprintsAsync(int boardId)
        {
            var increment = 50;
            var startAt = 0;
            var isLast = false;
            var result = new List<JiraSprint>();

            while (!isLast)
            {
                var response = await GetAsync($"{_url}/rest/agile/latest/board/{boardId}/sprint?startAt={startAt}&maxResults=50");
                var sprintsResult = JsonSerializer.Deserialize<JiraSprintsResult>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                isLast = sprintsResult.IsLast;
                result.AddRange(sprintsResult.Values);
                startAt += increment;
            }

            return result;
        }
    }
}
