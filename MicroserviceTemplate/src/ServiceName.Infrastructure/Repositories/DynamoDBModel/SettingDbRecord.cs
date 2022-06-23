using Amazon.DynamoDBv2.DataModel;

namespace ServiceName.Infrastructure.Repositories.DynamoDBModel
{
    [DynamoDBTable("ServiceName_Setting")]
    public class SettingDbRecord
    {
        [DynamoDBHashKey] //Partition key
        public string TenantId
        {
            get; set;
        }

        public string Settings
        {
            get; set;
        }
    }
}