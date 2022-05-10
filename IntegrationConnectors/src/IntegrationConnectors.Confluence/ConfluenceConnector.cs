using IntegrationConnectors.Common;
using IntegrationConnectors.Confluence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntegrationConnectors.Confluence
{
    /// <summary>
    /// https://docs.atlassian.com/confluence/REST/latest/
    /// </summary>
    public class ConfluenceConnector : HttpConnector
    {
        public ConfluenceConnector(string baseUrl, string apiKey, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {
        }

        public async Task UpdatePage(string pageId, string htmlContent, string comment)
        {
            //Get ConfluenceConnector Page Info
            var response = await GetAsync($"{_url}/wiki/rest/api/content/{pageId}");
            var wikiPage = JsonSerializer.Deserialize<ConfluencePage>(response, _jsonSerializerOptions);

            //Update ConfluenceConnector Page
            var requestBody = new
            {
                type = "page",
                title = wikiPage.Title,
                body = new
                {
                    storage = new
                    {
                        value = htmlContent,
                        representation = "storage"
                    }
                },
                version = new
                {
                    number = wikiPage.Version.Number + 1,
                    message = comment
                }
            };

            var requestJson = JsonSerializer.Serialize(requestBody);

            var result = await PutAsync($"{_url}/wiki/rest/api/content/{pageId}", requestJson);
        }


        public async Task<List<ConfluencePageSearchResult>> SearchContentAsync(string contributorAccountId, string spaceKey, string label, string lastModifiedYear, string createdYear)
        {
            string contributorClause = string.Empty;
            string spaceClause = string.Empty;
            string labelClause = string.Empty;
            string lastModifiedClause = string.Empty;
            string createdYearClause = string.Empty;

            if (!string.IsNullOrEmpty(contributorAccountId))
            {
                contributorClause = $" and  contributor='{contributorAccountId}'";
            }

            if (!string.IsNullOrEmpty(spaceKey))
            {
                spaceClause = $" and space.key='{spaceKey}'";
            }

            if (!string.IsNullOrEmpty(label))
            {
                spaceClause = $" and label='{label}'";
            }

            if (!string.IsNullOrEmpty(lastModifiedYear))
            {
                lastModifiedClause = $" and lastModified <= '{lastModifiedYear}-12-31'";
            }

            if (!string.IsNullOrEmpty(createdYear))
            {
                createdYearClause = $" and created  <= '{createdYear}-12-31'";
            }

            var results = new List<ConfluencePageSearchResult>();
            var response = await GetAsync($"{_url}/wiki/rest/api/content/search?limit=10000&cql=type=page{contributorClause}{spaceClause}{labelClause}{lastModifiedClause}{createdYearClause}&expand=history,history.lastUpdated,metadata.labels");
            var wikiPageSearchResults = JsonSerializer.Deserialize<ConfluencePageSearchResults>(response, _jsonSerializerOptions);
            results.AddRange(wikiPageSearchResults.Results);
            while (wikiPageSearchResults.Links.Next != null)
            {
                response = await GetAsync($"{_url}/wiki{wikiPageSearchResults.Links.Next}");
                wikiPageSearchResults = JsonSerializer.Deserialize<ConfluencePageSearchResults>(response, _jsonSerializerOptions);
                results.AddRange(wikiPageSearchResults.Results);
            }

            return results.OrderByDescending(r => r.History.LastUpdated.When).ThenBy(r => r.History.CreatedDate).ToList();
        }

        public async Task<List<ConfluencePageSearchResult>> SearchContentByCreator(string userAccountId)
        {
            var results = new List<ConfluencePageSearchResult>();
            var response = await GetAsync($"{_url}/wiki/rest/api/content/search?limit=10000&cql=type=page%20and%20(creator='{userAccountId}')&expand=history,history.lastUpdated");
            var wikiPageSearchResults = JsonSerializer.Deserialize<ConfluencePageSearchResults>(response, _jsonSerializerOptions);
            results.AddRange(wikiPageSearchResults.Results);
            while (wikiPageSearchResults.Links.Next != null)
            {
                response = await GetAsync($"{_url.Replace("/rest/api", "")}{wikiPageSearchResults.Links.Next}");
                wikiPageSearchResults = JsonSerializer.Deserialize<ConfluencePageSearchResults>(response, _jsonSerializerOptions);
                results.AddRange(wikiPageSearchResults.Results);
            }

            return results.OrderByDescending(r => r.History.LastUpdated.When).ThenBy(r => r.History.CreatedDate).ToList();
        }

        public async Task<List<ConfluencePageSearchResult>> SearchContentByParentId(string parentId)
        {
            var results = new List<ConfluencePageSearchResult>();
            int counter = 0;
            var confluenceLimit = 1000; //Confluence Max Limit = 1000 pages
            var extendedLimit = confluenceLimit * 20;
            while (counter != extendedLimit)//Workaround to increase limit
            {
                var response = await GetAsync($"{_url}/wiki/rest/api/search?limit={confluenceLimit}&start={counter}&cql=ancestor={parentId}+and+type=page");
                var wikiPageSearchResults = JsonSerializer.Deserialize<ConfluencePageSearchResults>(response, _jsonSerializerOptions);

                foreach (var result in wikiPageSearchResults.Results)
                {
                    result.Content.History = new ConfluencePageHistory() { LastUpdated = new ConfluencePageLastUpdated() { When = !result.LastModified.HasValue ? DateTime.Now : result.LastModified.Value } };
                    results.Add(result.Content);
                }

                counter = counter + confluenceLimit;
            }

            return results.OrderByDescending(r => r.History.LastUpdated.When).ToList();
        }
    }
}
