namespace IntegrationConnectors.TeamCity.Model
{
    public class TeamCityBuild
    {
        public int Id { get; set; }
        public string BuildTypeId { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public string BranchName { get; set; }
        public bool DefaultBranch { get; set; }
        public string Href { get; set; }
        public string WebUrl { get; set; }
        public DateTime FinishOnAgentDate { get; set; }
        public bool Customized { get; set; }
    }
}