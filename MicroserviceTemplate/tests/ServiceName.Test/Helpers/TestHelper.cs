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
    }
}
