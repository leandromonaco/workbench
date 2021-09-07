namespace IntegrationConnectors.Octopus.Model
{
    public class OctopusRelease
    {
        public string ProjectId { get; set; }
        public string ChannelId { get; set; }
        public string ReleaseNotes { get; set; }
        public string Version { get; set; }
    }
}