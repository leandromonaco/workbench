using Settings.Application.Interfaces;

namespace Settings.Infrastructure.Repositories
{
    public class SettingsRepositoryAws : ISettingsRepository
    {
        Task<Domain.Model.Settings> ISettingsRepository.GetSettingsAsync()
        {
            throw new NotImplementedException();
        }
    }
}