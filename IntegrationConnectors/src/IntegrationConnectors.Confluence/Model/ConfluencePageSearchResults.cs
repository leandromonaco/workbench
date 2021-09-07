using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntegrationConnectors.Confluence.Model
{
    public class ConfluencePageSearchResults
    {
        public List<ConfluencePageSearchResult> Results { get; set; }
        [JsonPropertyName("_links")]
        public ConfluencePageLinks Links { get; set; }
    }
}