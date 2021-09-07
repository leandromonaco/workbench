using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeploymentProcessId { get; set; }
        public List<OctopusPackage> Packages { get; set; }
        public List<string> IncludedLibraryVariableSetIds { get; set; }
        public string VariableSetId { get; set; }
    }
}