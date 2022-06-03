using System.Text.Json;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Infrastructure.Repositories
{
    public class MockSettingsRepository : ISettingsRepository
    {
        public async Task<Settings> GetSettingsAsync(Guid tenantId)
        {
            var settings = new Settings() { 
                Group1 = new SettingGroup() { 
                    Setting1 = "a", 
                    Setting2 = "b" } 
            };
            return await Task.FromResult(settings);
        }

        public async Task<bool> SaveSettingsAsync(Guid tenantId, Settings settings)
        {
            return await Task.FromResult(true);
        }
    }
}