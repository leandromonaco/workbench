using System.Text.Json.Serialization;

namespace IntegrationConnectors.AzDevOps.Model.Backlog
{
    public class AzDevOpsWorkItemFields
    {
        private string _team;
        [JsonPropertyName("System.AreaPath")]
        public string Team
        {
            get { return _team != null ? _team.Replace($"{Project}\\", string.Empty) : string.Empty; }
            set { _team = value; }
        }
        [JsonPropertyName("System.TeamProject")]
        public string Project { get; set; }
        private string _sprint;
        [JsonPropertyName("System.IterationPath")]
        public string Sprint
        {
            get { return _sprint != null ? _sprint.Replace($"{Project}\\", string.Empty) : string.Empty; }
            set { _sprint = value; }
        }
        [JsonPropertyName("System.State")]
        public string Status { get; set; }
        [JsonPropertyName("System.Title")]
        public string Title { get; set; }
        [JsonPropertyName("Microsoft.VSTS.Scheduling.Effort")]
        public double Size { get; set; }
        [JsonPropertyName("Microsoft.VSTS.Common.Priority")]
        public int Priority { get; set; }
    }
}