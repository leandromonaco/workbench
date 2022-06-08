using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Core.Repository
{
    /// <summary>
    //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/CodeSamples.DotNet.html
    //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LowLevelDotNetItemCRUD.html
    //aws --endpoint-url=http://localhost:8000 dynamodb create-table --table-name ServiceName_Setting --attribute-definitions AttributeName=TenantId,AttributeType=S --key-schema AttributeName=TenantId,KeyType=HASH --billing-mode PAY_PER_REQUEST
    //awslocal dynamodb create-table --table-name ServiceName_Setting --attribute-definitions AttributeName=TenantId,AttributeType=S --key-schema AttributeName=TenantId,KeyType=HASH --billing-mode PAY_PER_REQUEST
    /// </summary>
    public class SettingsRepository : IRepositoryService<Settings>
    {
        IDatabaseService _databaseService;

        public SettingsRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<Settings> GetByIdAsync(Guid tenantId)
        {
            QueryRead query = new QueryRead() {
                
                TableName = "ServiceName_Setting",
                SelectFields = new List<string>() { "Settings" },
                FilterFields = new List<QueryField>() { new QueryField(){ 
                                                                            Name = "TenantId", 
                                                                            Value = tenantId 
                                                                        }
                                                      }
            };

            query.DefaultResult = new Settings()
                                        {
                                            Group1 = new SettingGroup()
                                            {
                                                Setting1 = "a",
                                                Setting2 = "b"
                                            },
                                        };


            var settings = await _databaseService.ExecuteQuery(query);
            return (Settings)settings;
        }

        public async Task<bool> SaveAsync(Guid tenantId, Settings settings)
        {
            QueryWrite query = new QueryWrite()
            {
                TableName = "ServiceName_Setting",
                FilterFields = new List<QueryField>() { new QueryField(){
                                                                            Name = "TenantId",
                                                                            Value = tenantId
                                                                        }
                                                      },
                UpdateFields = new List<QueryField>() {
                                                        new QueryField(){
                                                                            Name = "Settings",
                                                                            Value = settings
                                                                        }
                                                      }
            };
            
            var result = await _databaseService.ExecuteQuery(query);
            return (bool)result;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Settings>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
