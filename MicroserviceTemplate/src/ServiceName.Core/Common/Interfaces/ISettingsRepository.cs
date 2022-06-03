using ServiceName.Core.Model;

namespace ServiceName.Core.Common.Interfaces
{
    public interface ISettingsRepository
    {
        Task<Settings> GetSettingsAsync(Guid tenantId);
        Task<bool> SaveSettingsAsync(Guid tenantId, Settings settings);
    }
}
