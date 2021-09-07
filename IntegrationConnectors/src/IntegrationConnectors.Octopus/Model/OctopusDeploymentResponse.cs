using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusDeploymentResponse
    {
        public List<OctopusDeployment> Items { get; set; }
    }
}