using System;
using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusVariableSets
    {
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<OctopusVariableSet> Items { get; set; }

        public string Where()
        {
            throw new NotImplementedException();
        }
    }
}