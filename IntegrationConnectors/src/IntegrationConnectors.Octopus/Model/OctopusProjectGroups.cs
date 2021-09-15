using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusProjectGroups
    {
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<OctopusProjectGroup> Items { get; set; }
    }
}