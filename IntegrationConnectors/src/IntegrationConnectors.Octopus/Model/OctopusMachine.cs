using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusMachine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<OctopusDeployment> Deployments { get; set; }
    }
}