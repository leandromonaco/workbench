using ServiceName.Infrastructure.Repositories.DynamoDBModel;

namespace ServiceName.Test.Helpers
{
    internal class TestHelper
    {
        public static SettingDbRecord GetSettingsRecordObject()
        {
            return new SettingDbRecord()
            {
                TenantId = "53a13ec4-fde8-4087-8e2a-5fb6b1fbc062",
                Settings = @"{""CategoryA"":{""IsSettingAEnabled"":true,""IsSettingBEnabled"":true}}"
            };
        }
    }
}
