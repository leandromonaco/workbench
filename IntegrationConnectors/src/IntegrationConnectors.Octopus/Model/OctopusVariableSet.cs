using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusVariableSet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string VariableSetId { get; set; }
        public List<OctopusVariable> Variables { get; set; }
    }
}