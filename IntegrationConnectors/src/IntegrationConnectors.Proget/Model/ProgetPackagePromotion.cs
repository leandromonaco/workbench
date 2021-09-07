namespace IntegrationConnectors.Proget.Model
{
    public class ProgetPackagePromotion
    {
        public string PackageName { get; set; }
        public string GroupName { get; set; }
        public string Version { get; set; }
        public string FromFeed { get; set; }
        public string ToFeed { get; set; }
        public string Comments { get; set; }
    }
}