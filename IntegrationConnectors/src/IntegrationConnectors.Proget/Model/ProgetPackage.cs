using System.Collections.Generic;

namespace IntegrationConnectors.Proget.Model
{
    public class ProgetPackage
    {
        public string PackageName { get; set; }
        public string PackageType { get; set; }
        public string Version { get; set; }
        public List<ProgetPromotion> Promotions { get; set; }
    }
}