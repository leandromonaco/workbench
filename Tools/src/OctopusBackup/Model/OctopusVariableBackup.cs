using IntegrationConnectors.Octopus.Model;
using System.Collections.Generic;

namespace OctopusBackup.Model
{
    public class OctopusVariableBackup
    {
        public OctopusVariableSet ProjectVariables { get; set; }
        public List<OctopusVariableSet> LibrarySets { get; set; }
    }
}