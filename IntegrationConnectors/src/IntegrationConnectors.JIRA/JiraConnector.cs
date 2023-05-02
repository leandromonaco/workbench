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

        public async Task<List<JiraIssue>> GetIssuesAsync(string jql)
        {
            var allJiraIssues = new List<JiraIssue>();

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
                var response = await GetAsync($"{_url}/rest/api/2/search?jql={jql}&startAt={startAt}");

                var jqlQueryResult = JsonSerializer.Deserialize<JiraQueryResult>(response, jsonSerializerOptions);

                Console.WriteLine($"Processing {startAt} of {jqlQueryResult.Total} {DateTime.Now}");

                startAt += increment;
                finishAt = jqlQueryResult.Total - 1;
                allJiraIssues.AddRange(jqlQueryResult.Issues);
            }

            return allJiraIssues;
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
