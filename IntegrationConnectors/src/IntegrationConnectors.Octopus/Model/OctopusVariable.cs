namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusVariable
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public OctopusScope Scope { get; set; }
    }
}