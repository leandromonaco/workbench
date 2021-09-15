using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusEnvironments
    {
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<OctopusEnvironment> Items { get; set; }
    }
}
