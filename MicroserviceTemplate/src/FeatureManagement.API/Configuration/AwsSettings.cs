namespace FeatureFlag.API.Configuration
{
    public class AwsSettings
    {
        public string ApplicationId { get; set; } = string.Empty;

        public string EnvironmentId { get; set; } = string.Empty;

        public string ConfigurationProfileId { get; set; } = string.Empty;

        public TimeSpan? DataPollFrequency { get; set; } = TimeSpan.FromMinutes(10);

        public string ConfigSectionNaming { get; set; } = string.Empty;
    }
}
