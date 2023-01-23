namespace IntegrationConnectors.TeamCity.Model
{
    public class TeamCityCodeChange
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Username { get; set; }
        public string Date { get; set; }
        public string Href { get; set; }
        public string WebUrl { get; set; }
        public string Comment { get; set; }
        public TeamCityCodeChangeFiles Files { get; set; }
        public TeamCityCodeChangeVcsrootinstance VcsRootInstance { get; set; }
        public TeamCityCodeChangeCommiter Commiter { get; set; }
    }
}