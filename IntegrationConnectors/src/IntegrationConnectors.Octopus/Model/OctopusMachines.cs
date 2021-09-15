using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusMachines
    {
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<OctopusMachine> Items { get; set; }
    }
}