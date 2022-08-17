using Amazon.AppConfigData;

namespace FeatureFlag.API.Configuration
{
    public class AwsFeatureFlagConfigurationSource : IConfigurationSource
    {
        private readonly AmazonAppConfigDataClient _client;
        private readonly AwsSettings _cfg;

        public AwsFeatureFlagConfigurationSource(AmazonAppConfigDataClient client, AwsSettings cfg)
        {
            _client = client;
            _cfg = cfg;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AwsFeatureFlagConfigurationProvider(_client, _cfg);
        }
    }
}
