using System;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusDeployment
    {
        public string State { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CompletedTime { get; set; }
        public bool IsCompleted { get; set; }
        public bool HasWarningsOrErrors { get; set; }
    }
}