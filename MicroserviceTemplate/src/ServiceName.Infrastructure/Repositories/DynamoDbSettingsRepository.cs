﻿using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Infrastructure.Repositories
{
    /// <summary>
    //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/CodeSamples.DotNet.html
    //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LowLevelDotNetItemCRUD.html
    //aws --endpoint-url=http://localhost:8000 dynamodb create-table --table-name ServiceName_Setting --attribute-definitions AttributeName=TenantId,AttributeType=S --key-schema AttributeName=TenantId,KeyType=HASH --billing-mode PAY_PER_REQUEST
    //awslocal dynamodb create-table --table-name ServiceName_Setting --attribute-definitions AttributeName=TenantId,AttributeType=S --key-schema AttributeName=TenantId,KeyType=HASH --billing-mode PAY_PER_REQUEST
    /// </summary>
    public class DynamoDbSettingsRepository : IRepositoryService<Settings>
    {
        AmazonDynamoDBClient _amazonDynamoDBClient;
        string _dynamoTableName = "ServiceName_Setting";

        public DynamoDbSettingsRepository()
        {
            _amazonDynamoDBClient = new AmazonDynamoDBClient("test", "test");
        }

        public async Task<Settings> GetAsync(Guid tenantId)
        {
            var getRequest = new GetItemRequest
            {
                TableName = _dynamoTableName,
                Key = new Dictionary<string, AttributeValue>() { { "TenantId", new AttributeValue { S = tenantId.ToString().ToLower() } } },
            };

            var response = await _amazonDynamoDBClient.GetItemAsync(getRequest);

            //If there is no settings configured for the tenant create a new one
            if (response.Item.Count.Equals(0))
            {
                var newSettings = new Settings()
                {
                    Group1 = new SettingGroup()
                    {
                        Setting1 = "a",
                        Setting2 = "b"
                    },
                };

                await SaveAsync(tenantId, newSettings);

                return newSettings;
            }

            var existingSettings = JsonSerializer.Deserialize<Settings>(response.Item["Settings"].S, new JsonSerializerOptions() { MaxDepth = 0, PropertyNameCaseInsensitive = true });

            return existingSettings;
        }

        public async Task<bool> SaveAsync(Guid tenantId, Settings settings)
        {
            var putRequest = new PutItemRequest
            {
                TableName = _dynamoTableName,
                Item = new Dictionary<string, AttributeValue>()
                {
                    { "TenantId", new AttributeValue { S = tenantId.ToString() } },
                    { "Settings", new AttributeValue { S = JsonSerializer.Serialize(settings).ToLower() } }
                }
            };

            var response = await _amazonDynamoDBClient.PutItemAsync(putRequest);

            if (!response.HttpStatusCode.Equals(HttpStatusCode.OK))
            {
                return false;
            }

            return true;
        }

    }
}
