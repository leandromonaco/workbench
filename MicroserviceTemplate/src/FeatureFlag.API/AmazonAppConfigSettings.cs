namespace FeatureFlag.API
{
    public class AmazonAppConfigSettings
    {
        /// <summary>
        /// Name or Id
        /// </summary>
        public string ApplicationId { get; set; } = String.Empty;

        /// <summary>
        /// Name or Id
        /// </summary>
        public string EnvironmentId { get; set; } = String.Empty;

        /// <summary>
        /// Name or Id
        /// </summary>
        public string ConfigurationProfileId { get; set; } = String.Empty;

        /// <summary>
        /// Data poll frequency, default is 10 minutes
        /// </summary>
        public TimeSpan? DataPollFrequency { get; set; } = TimeSpan.FromMinutes(10);

        /// <summary>
        /// e.g.: FeatureManagement or MyApp:AWSFlags  
        /// </summary>
        public string ConfigSectionNaming { get; set; } = String.Empty;
    }
}
