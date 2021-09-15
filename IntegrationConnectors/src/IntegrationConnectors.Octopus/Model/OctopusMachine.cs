using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusMachine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string StatusSummary { get; set; }
        public string HealthStatus { get; set; }
        public string OperatingSystem { get; set; }
        public List<string> Roles { get; set; }
        public List<string> EnvironmentIds { get; set; }
        public OctopusDeployment LastDeployment { get; set; }
    }
}