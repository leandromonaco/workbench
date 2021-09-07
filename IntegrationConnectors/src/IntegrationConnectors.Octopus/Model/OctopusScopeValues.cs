using System.Collections.Generic;

namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusScopeValues
    {
        public List<OctopusEnvironment> Environments { get; set; }
        public List<OctopusMachine> Machines { get; set; }
        public List<OctopusAction> Actions { get; set; }
        public List<OctopusRole> Roles { get; set; }
        public List<OctopusChannel> Channels { get; set; }

    }
}