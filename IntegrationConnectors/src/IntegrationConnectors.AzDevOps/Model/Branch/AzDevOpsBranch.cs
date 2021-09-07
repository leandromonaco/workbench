using IntegrationConnectors.AzDevOps.Model.Shared;

namespace IntegrationConnectors.AzDevOps.Model.Branch
{
    public class AzDevOpsBranch
    {
        public string Name { get; set; }
        public AzDevOpsUser Creator { get; set; }
    }
}
