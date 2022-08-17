using Amazon.AppConfigData;

namespace FeatureFlag.API.Configuration
{
    public static class AwsConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddAmazonFeatureFlags(
            this IConfigurationBuilder builder,
            AmazonAppConfigDataClient client,
            AwsSettings config)
        {
            var cfg = config ?? throw new ArgumentNullException();
            if (string.IsNullOrEmpty(cfg.EnvironmentId))
            {
                throw new ArgumentException("Environment id (or name) missing from Amazon AppConfig settings");
            }
            if (string.IsNullOrEmpty(cfg.ApplicationId))
            {
                throw new ArgumentException("Application id (or name) missing from Amazon AppConfig settings");
            }
            if (string.IsNullOrEmpty(cfg.ConfigurationProfileId))
            {
                throw new ArgumentException("Configuration id (or name) missing from Amazon AppConfig settings");
            }

            builder.Add(new AwsFeatureFlagConfigurationSource(client, cfg));

            return builder;
        }


        public static IConfigurationBuilder AddAmazonFeatureFlags(this IConfigurationBuilder builder, AwsSettings config)
        {
            return builder.AddAmazonFeatureFlags(new AmazonAppConfigDataClient(), config);
        }

        public static IConfigurationBuilder AddAmazonFeatureFlags(this IConfigurationBuilder builder, Action<AwsSettings> configAction)
        {
            var settings = new AwsSettings();
            configAction(settings);
            return builder.AddAmazonFeatureFlags(new AmazonAppConfigDataClient(), settings);
        }

        public static IConfigurationBuilder AddAmazonFeatureFlags(this IConfigurationBuilder builder, AwsSettings config, AmazonAppConfigDataClient client)
        {
            return builder.AddAmazonFeatureFlags(client, config);
        }
    }
}

