using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusVariableSet
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public OctopusVariableSetType Type 
        {
            get 
            {
                if (OwnerId.StartsWith("Project"))
                {
                    return OctopusVariableSetType.Project;
                }
                else
                {
                    return OctopusVariableSetType.Library;
                }
            }
        }
        public List<OctopusVariable> Variables { get; set; }
        public OctopusScopeValues ScopeValues { get; set; }
    }
}