namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusCertificate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SubjectDistinguishedName { get; set; }
        public string SubjectCommonName { get; set; }
        public string Thumbprint { get; set; }
        public string NotAfter { get; set; }
        public string NotBefore { get; set; }
        public bool HasPrivateKey { get; set; }
        public bool IsExpired { get; set; }
    }
}