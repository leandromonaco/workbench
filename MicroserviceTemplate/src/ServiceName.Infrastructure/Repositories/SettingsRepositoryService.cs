using System.Text.Json;
using Amazon.DynamoDBv2.DataModel;
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
    public class SettingsRepositoryService : IRepositoryService<Settings>
    {
        readonly IDynamoDBContext _dynamoDBContext;
        readonly IConfigurationService _configurationService;
        readonly ILoggingService _loggingService;

        public SettingsRepositoryService(IConfigurationService configurationService, ILoggingService loggingService, IDynamoDBContext dynamoContext)
        {
            _configurationService = configurationService;
            _loggingService = loggingService;
            _dynamoDBContext = dynamoContext;             
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
            try
            {
                await _loggingService.LogInformationAsync($"LocalDynamoDatabaseService GetByIdAsync {id} started");
                var settingDbRecord = await _dynamoDBContext.LoadAsync<SettingDbRecord>(id.ToString());

                if (settingDbRecord == null)
                {
                    settingDbRecord= await SaveNewSettings(id, new Settings() { CategoryA = new SettingGroup() });
                }

                var settings = JsonSerializer.Deserialize<Settings>(settingDbRecord.Settings, new JsonSerializerOptions() { MaxDepth = 0, PropertyNameCaseInsensitive = true });

                await _loggingService.LogInformationAsync($"LocalDynamoDatabaseService GetByIdAsync {id} finished");

                return settings;
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> SaveAsync(Guid id, Settings obj)
        {
            try
            {
                await SaveNewSettings(id, obj);
                return true;
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace);
                throw;
            }
        }

        private async Task<SettingDbRecord> SaveNewSettings(Guid id, Settings obj)
        {
            var settingDbRecord = new SettingDbRecord()
            {
                TenantId = id.ToString(),
                Settings = JsonSerializer.Serialize(obj)
            };

            await _dynamoDBContext.SaveAsync<SettingDbRecord>(settingDbRecord);

            return settingDbRecord;
        }
    }
}
