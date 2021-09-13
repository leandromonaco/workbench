using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusCertificates
    {
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<OctopusCertificate> Items { get; set; }
    }
}