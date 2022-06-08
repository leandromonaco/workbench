//using System.Net;
//using System.Text.Json;
//using Amazon.DynamoDBv2;
//using Amazon.DynamoDBv2.Model;
//using ServiceName.Core.Common.Interfaces;
//using ServiceName.Core.Model;

//namespace ServiceName.Infrastructure.Databases
//{
//    /// <summary>
//    //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/CodeSamples.DotNet.html
//    //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LowLevelDotNetItemCRUD.html
//    //aws --endpoint-url=http://localhost:8000 dynamodb create-table --table-name ServiceName_Setting --attribute-definitions AttributeName=TenantId,AttributeType=S --key-schema AttributeName=TenantId,KeyType=HASH --billing-mode PAY_PER_REQUEST
//    //awslocal dynamodb create-table --table-name ServiceName_Setting --attribute-definitions AttributeName=TenantId,AttributeType=S --key-schema AttributeName=TenantId,KeyType=HASH --billing-mode PAY_PER_REQUEST
//    /// </summary>
//    public class DynamoDbDatabaseService : IDatabaseService
//    {
//        AmazonDynamoDBClient _amazonDynamoDBClient;
//        //string _dynamoTableName = "ServiceName_Setting";

//        public DynamoDbDatabaseService()
//        {
//            _amazonDynamoDBClient = new AmazonDynamoDBClient("test", "test");
//        }
        
//        public async Task<object> ExecuteQuery(QueryRead queryRead)
//        {
//            var getRequest = new GetItemRequest
//            {
//                TableName = queryRead.TableName,
//                Key = new Dictionary<string, AttributeValue>() { { queryRead.FilterFields[0].Name, new AttributeValue { S = queryRead.FilterFields[0].Value?.ToString().ToLower() } } },
//            };

//            var response = await _amazonDynamoDBClient.GetItemAsync(getRequest);

//            //If there is no settings configured for the tenant create a new one
//            if (response.Item.Count.Equals(0))
//            {
//                //Save
//                var queryWrite = new QueryWrite();
//                queryWrite.FilterFields = queryRead.FilterFields;
//                queryWrite.UpdateFields = new List<QueryField>() { new QueryField() { Name = queryRead.SelectFields[0].ToString(), Value = queryRead.DefaultResult } };
//                await ExecuteQuery(queryWrite);
//                return queryRead.DefaultResult;
//            }

//            var existingSettings = JsonSerializer.Deserialize<Settings>(response.Item[queryRead.SelectFields[0].ToString()].S, new JsonSerializerOptions() { MaxDepth = 0, PropertyNameCaseInsensitive = true });

//            return existingSettings;
//        }

//        public async Task<bool> ExecuteQuery(QueryWrite query)
//        {
//            var putRequest = new PutItemRequest
//            {
//                TableName = query.TableName,
//                Item = new Dictionary<string, AttributeValue>()
//                {
//                    { query.FilterFields[0].Name, new AttributeValue { S = query.FilterFields[0].Value.ToString() } },
//                    { query.UpdateFields[0].Name, new AttributeValue { S = JsonSerializer.Serialize(query.UpdateFields[0].Value).ToLower() } }
//                }
//            };

//            var response = await _amazonDynamoDBClient.PutItemAsync(putRequest);

//            if (!response.HttpStatusCode.Equals(HttpStatusCode.OK))
//            {
//                return false;
//            }

//            return true;
//        }

//    }
//}
