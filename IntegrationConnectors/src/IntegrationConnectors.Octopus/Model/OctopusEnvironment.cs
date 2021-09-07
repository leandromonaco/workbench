using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusEnvironment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<OctopusMachine> Machines { get; set; }
        public DateTime LastDeploymentDate { get; set; }
    }
}
