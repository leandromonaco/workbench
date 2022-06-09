using System.Net;
using System.Text.Json;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Repositories.DynamoDBModel;

namespace ServiceName.Infrastructure.Databases
{
    /// <summary>
    //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/CodeSamples.DotNet.html
    //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LowLevelDotNetItemCRUD.html
    //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DotNetSDKHighLevel.html    
    //aws --endpoint-url=http://localhost:8000 dynamodb create-table --table-name ServiceName_Setting --attribute-definitions AttributeName=TenantId,AttributeType=S --key-schema AttributeName=TenantId,KeyType=HASH --billing-mode PAY_PER_REQUEST
    //aws dynamodb list-tables --endpoint-url http://localhost:8000
    /// </summary>
    public class LocalDynamoDatabaseService : IRepositoryService<Settings>
    {
        DynamoDBContext _dynamoDBContext;

        public LocalDynamoDatabaseService()
        {
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName("ap-southeast-2"),
                UseHttp = true,
                ServiceURL = "http://localhost:8000"
            };

            var amazonDynamoDBClient = new AmazonDynamoDBClient("test", "test", clientConfig);
            var dynamoDBContextConfig = new DynamoDBContextConfig() { ConsistentRead = true };
            _dynamoDBContext = new DynamoDBContext(amazonDynamoDBClient, dynamoDBContextConfig);            
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Settings>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Settings> GetByIdAsync(Guid id)
        {
            var settingDbRecord = await _dynamoDBContext.LoadAsync<SettingDbRecord>(id.ToString());
            if (settingDbRecord == null)
            {
                settingDbRecord = new SettingDbRecord()
                {
                    TenantId = id.ToString(),
                    Settings = JsonSerializer.Serialize(new Settings() { CategoryA = new SettingGroup() })
                };

                await _dynamoDBContext.SaveAsync<SettingDbRecord>(settingDbRecord);
            }

            var settings = JsonSerializer.Deserialize<Settings>(settingDbRecord.Settings, new JsonSerializerOptions() { MaxDepth = 0, PropertyNameCaseInsensitive = true });
            return settings;
        }

        public async Task<bool> SaveAsync(Guid id, Settings obj)
        {
            var settingDbRecord = new SettingDbRecord()
            {
                TenantId = id.ToString(),
                Settings = JsonSerializer.Serialize(obj)
            };
            
            await _dynamoDBContext.SaveAsync<SettingDbRecord>(settingDbRecord);
           
            return true;
        }
    }
}
