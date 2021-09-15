using IntegrationConnectors.Octopus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusBackup.Model
{
    public class OctopusSpaceBackup
    {
        public List<OctopusEnvironment> Environments { get; set; }
        public List<OctopusMachine> Machines { get; set; }
        public List<OctopusCertificate> Certificates { get; set; }
        public List<OctopusVariableSet> LibraryVariableSets { get; set; }
        public List<OctopusProject> Projects { get; set; }
    }
}
