using System.Text.Json.Serialization;

namespace JiraReporting.Model
{
    public class JiraFields
    {
        public string Summary { get; set; }
        public JiraParent Parent { get; set; }
        public JiraStatus Status { get; set; }
        public JiraIssueType IssueType { get; set; }
        public JiraAssignee Assignee { get; set; }

        public JiraPriority Priority { get; set; }
        
        //TODO: Make this customizable
        [JsonPropertyName("customfield_10020")]
        public List<JiraSprint> Sprints { get; set; }

        [JsonPropertyName("customfield_10026")]
        public double? Points { get; set; }

        [JsonPropertyName("fixVersions")]
        public List<JiraFixVersion> FixVersions { get; set; }

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