using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ServiceName.API.Extensions.HealthCheck
{
    public class DynamoDbHealthCheck : IHealthCheck
    {
        readonly IConfiguration _configuration;
        
        public DynamoDbHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var accessKey = _configuration["ModuleConfiguration:Infrastructure:DynamoDb:AccessKey"];
                var secretKey = _configuration["ModuleConfiguration:Infrastructure:DynamoDb:SecretKey"];
                var regionEndpoint = RegionEndpoint.GetBySystemName(_configuration["ModuleConfiguration:Infrastructure:DynamoDb:RegionEndpoint"]);
                var tableName = _configuration["ModuleConfiguration:Infrastructure:DynamoDb:TableName"];
                var localTestEndpoint = _configuration["ModuleConfiguration:Infrastructure:DynamoDb:LocalTestEndpoint"];

                AmazonDynamoDBConfig clientConfig = new()
                {
                    RegionEndpoint = regionEndpoint,
                };

                if (!string.IsNullOrEmpty(localTestEndpoint))
                {
                    clientConfig.UseHttp = true;
                    clientConfig.ServiceURL = localTestEndpoint;
                }

                var amazonDynamoDBClient = new AmazonDynamoDBClient(accessKey, secretKey, clientConfig);  
                
                Table.LoadTable(amazonDynamoDBClient, tableName);
               
                return Task.FromResult(HealthCheckResult.Healthy($"Table {tableName} exists."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, ex.Message));
            }

          
        }
    }
}
