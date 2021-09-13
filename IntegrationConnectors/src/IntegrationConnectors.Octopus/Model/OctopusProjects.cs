using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusProjects
    {
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<OctopusProject> Items { get; set; }
    }
}