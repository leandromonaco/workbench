using JiraReporting.Model;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JiraReporting.Model
{
    public class Fields
    {
        public string Summary { get; set; }
        public Parent Parent { get; set; }
        public Status Status { get; set; }
        public IssueType IssueType { get; set; }
        public Assignee Assignee { get; set; }

        public Priority Priority { get; set; }
        
        //TODO: Make this customizable
        [JsonPropertyName("customfield_10020")]
        public List<Sprint> Sprints { get; set; }

        [JsonPropertyName("customfield_10026")]
        public double? Points { get; set; }

        [JsonPropertyName("fixVersions")]
        public List<FixVersion> FixVersions { get; set; }

        /*
         "fixVersions": [
            {
                "self": "https://humanforce.atlassian.net/rest/api/2/version/10613",
                "id": "10613",
                "description": "",
                "name": "5.0.16",
                "archived": false,
                "released": true,
                "releaseDate": "2022-08-25"
            }
        ],
         
         */

    }
}