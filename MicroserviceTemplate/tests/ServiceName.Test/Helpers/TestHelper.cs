using Microsoft.Extensions.Configuration;
using ServiceName.Infrastructure.Repositories.DynamoDBModel;

namespace ServiceName.Test.Helpers
{
    internal class TestHelper
    {
        public static SettingDbRecord GetDynamoDBRecord(string tenantId)
        {
            switch (tenantId)
            {
                case "53a13ec4-fde8-4087-8e2a-5fb6b1fbc062":
                    return new SettingDbRecord()
                    {
                        TenantId = "53a13ec4-fde8-4087-8e2a-5fb6b1fbc062",
                        Settings = @"{""CategoryA"":{""IsSettingAEnabled"":true,""IsSettingBEnabled"":true}}"
                    };
                case "e2c92679-5309-438b-8efc-64054a7babc2":
                    return new SettingDbRecord()
                    {
                        TenantId = "e2c92679-5309-438b-8efc-64054a7babc2",
                        Settings = String.Empty
                    };                  
                default:
                    return null;
            }      
        }

        internal static IConfigurationRoot GetConfigurationMock()
        {
           var configDictionary = new Dictionary<string, string>
                                    {
                                            {"ModuleConfiguration:AwsServices:Kms:AccessKey", "test"},
                                            {"ModuleConfiguration:AwsServices:Kms:SecretKey", "test"},
                                            {"ModuleConfiguration:AwsServices:Kms:RegionEndpoint", "eu-west-2"},
                                            {"ModuleConfiguration:AwsServices:Kms:LocalTestEndpoint", "http://localhost:52002"},
                                            {"ModuleConfiguration:AwsServices:KMS:SigningKeyId", "6732c7ca-6ec9-4b96-9711-fd1c7d637c8e"},
                                            {"ModuleConfiguration:AwsServices:KMS:PublicKey", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA56tfR6w3YJpbH5XZ6Ze2kB2evnUpbyZJiTLPKSc/VeA46m09lVB7bJRp0pKX2LusT2pccrVe5AYtbnikKqhOQWUdjLJnSONPNpd4yjEseqPblsXicA+xdP+Fk2W0yDxOc79LUAywgjV8JqNbbtVbhzqVPOLalJYnPEAVa3NQV138dnU7NzxbAjPjXINi7BBZ2OLRuocJRMfe16AUiQtH8MaWfRnnRRwdCBLJCXnZy+0hVc701SrVoTS+CA8RfGTCnzutx9MXW7t4SCEjZH0MSfhSZbKggPfi36HeUdClacgD6L0+FhSBKzd8kOC06CDf5WM9oV/XtWVXEWWGPDHv8wIDAQAB"},
                                            {"ModuleConfiguration:Jwt:Issuer", "Issuer"},
                                            {"ModuleConfiguration:Jwt:Audience", "Audience"},
                                    };



            var configuration = new ConfigurationBuilder()
                        .AddInMemoryCollection(configDictionary)
                        .Build();

            return configuration;
        }
    }
}
