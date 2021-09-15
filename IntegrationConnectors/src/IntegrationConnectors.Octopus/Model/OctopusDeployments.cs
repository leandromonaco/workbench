using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusDeployments
    {
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<OctopusDeployment> Items { get; set; }
    }
}