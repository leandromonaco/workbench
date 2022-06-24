using Amazon;
using Amazon.KeyManagementService;
using Microsoft.Extensions.Configuration;
using ServiceName.Infrastructure.Repositories.DynamoDBModel;

namespace ServiceName.Test.Helpers
{
    internal class TestHelper
    {
        readonly private IConfiguration _configuration;

        public TestHelper()
        {
            _configuration = new ConfigurationBuilder()
                                .SetBasePath(Environment.CurrentDirectory)
                                .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
                                .Build();
        }

        public IConfiguration Configuration
        { get { return _configuration; } }

        public SettingDbRecord GetDynamoDBRecord(string tenantId)
        {
            var dynamoDbData = _configuration.GetSection("TestConfiguration:DynamoDBData").Get<List<SettingDbRecord>>();
            return dynamoDbData.Where(d => d.TenantId.Equals(tenantId)).FirstOrDefault();
        }

        public IAmazonKeyManagementService GetAmazonKms()
        {
            var accessKey = _configuration["ModuleConfiguration:Infrastructure:Kms:AccessKey"];
            var secretKey = _configuration["ModuleConfiguration:Infrastructure:Kms:SecretKey"];
            var regionEndpoint = RegionEndpoint.GetBySystemName(_configuration["ModuleConfiguration:Infrastructure:Kms:RegionEndpoint"]);
            var localTestEndpoint = _configuration["ModuleConfiguration:Infrastructure:Kms:LocalTestEndpoint"];

            AmazonKeyManagementServiceConfig amazonKeyManagementServiceConfig = new()
            {
                RegionEndpoint = regionEndpoint,
            };

            if (!string.IsNullOrEmpty(localTestEndpoint))
            {
                amazonKeyManagementServiceConfig.UseHttp = true;
                amazonKeyManagementServiceConfig.ServiceURL = localTestEndpoint;
            }

            if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey))
            {
                return new AmazonKeyManagementServiceClient(accessKey, secretKey, amazonKeyManagementServiceConfig);
            }

            return new AmazonKeyManagementServiceClient(amazonKeyManagementServiceConfig);
        }
    }
}