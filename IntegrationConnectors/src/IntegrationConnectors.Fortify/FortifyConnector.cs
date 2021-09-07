using IntegrationConnectors.Common;
using IntegrationConnectors.Fortify.Model;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntegrationConnectors.Fortify
{
    public class FortifyConnector : HttpConnector
    {
        public FortifyConnector(string baseUrl, string apiKey, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {
        }

        public async Task<string> GetUnifiedLoginTokenAsync()
        {
            //Get ConfluenceConnector Page Info
            var response = await PostAsync($"{_url}/api/v1/tokens", "{\"type\": \"UnifiedLoginToken\"}");
            var authResponse = JsonSerializer.Deserialize<FortifyTokenResponse>(response, _jsonSerializerOptions);
            return authResponse.Data.Token;
        }

        public async Task<List<FortifyProject>> GetProjectsAsync()
        {
            //Get ConfluenceConnector Page Info
            var response = await GetAsync($"{_url}/api/v1/projects");
            var projectResponse = JsonSerializer.Deserialize<FortifiyProjectsResponse>(response, _jsonSerializerOptions);
            return projectResponse.Data;
        }

        public async Task<List<FortifyProjectVersion>> GetProjectVersionsAsync(int projectId)
        {
            //Get ConfluenceConnector Page Info
            var response = await GetAsync($"{_url}/api/v1/projects/{projectId}/versions");
            var projectResponse = JsonSerializer.Deserialize<FortifiyProjectVersionsResponse>(response, _jsonSerializerOptions);
            return projectResponse.Data;
        }

        public async Task<ExportToCsvResponse> ExportToCsvAsync(int projectVersionId, string csvFilename, string filterSet)
        {
            var response = await PostAsync($"{_url}/api/v1/dataExports/action/exportAuditToCsv", $"{{ \"datasetName\": \"Audit\", \"fileName\": \"{csvFilename}\", \"filterSet\": \"{filterSet}\", \"includeCommentsInHistory\": true, \"includeHidden\": true, \"includeRemoved\": true, \"includeSuppressed\": true, \"projectVersionId\": {projectVersionId} }}");
            var projectResponse = JsonSerializer.Deserialize<ExportToCsvResponse>(response, _jsonSerializerOptions);
            return projectResponse;
        }

        public async Task<List<FortifyDataExport>> GetDataExportsAsync()
        {
            //Get ConfluenceConnector Page Info
            var response = await GetAsync($"{_url}/api/v1/dataExports");
            var projectResponse = JsonSerializer.Deserialize<FortifyDataExportResponse>(response, _jsonSerializerOptions);
            return projectResponse.Data;
        }

        public async Task<string> GetReportFileTokenAsync()
        {
            //Get ConfluenceConnector Page Info
            var response = await PostAsync($"{_url}/api/v1/fileTokens", "{\"fileTokenType\": \"REPORT_FILE\"}");
            var authResponse = JsonSerializer.Deserialize<FortifyTokenResponse>(response, _jsonSerializerOptions);
            return authResponse.Data.Token;
        }

        public async Task<string> DownloadCsvAsync(string fileToken, int reportId)
        {
            //Get ConfluenceConnector Page Info
            var response = await GetAsync($"{_url}/transfer/dataExportDownload.html?mat={fileToken}&id={reportId}");
            return response;
        }

        public async Task<List<FortifyIssue>> GetIssuesAsync(int projectVersionId)
        {
            //Get ConfluenceConnector Page Info
            var response = await GetAsync($"{_url}/api/v1/projectVersions/{projectVersionId}/issues?limit=-1");
            var issuesResponse = JsonSerializer.Deserialize<FortifyIssuesResponse>(response, _jsonSerializerOptions);
            return issuesResponse.Data;
        }

        public async Task<List<FortifyIssueDetails>> GetIssueDetailsAsync(string projectName, string projectVersionName, string instanceId)
        {
            //Get ConfluenceConnector Page Info
            var response = await GetAsync($"{_url}/api/v1/issueDetails?projectName={projectName}&projectVersionName={projectVersionName}&instanceId={instanceId}");
            var issuesResponse = JsonSerializer.Deserialize<FortifyIssueDetailsResponse>(response, _jsonSerializerOptions);
            return issuesResponse.Data;
        }

    }
}
