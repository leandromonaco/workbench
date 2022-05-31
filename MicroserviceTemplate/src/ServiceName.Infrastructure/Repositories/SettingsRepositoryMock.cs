using Settings.Application.Interfaces;

namespace Settings.Infrastructure.Repositories
{
    public class SettingsRepositoryMock : ISettingsRepository
    {
        public async Task<Domain.Model.Settings> GetSettingsAsync()
        {
            return await Task.FromResult(new Domain.Model.Settings() { Setting1 = "a", Setting2 = "b" });
        }
    }
}