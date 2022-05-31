namespace Settings.Application.Interfaces
{
    public interface ISettingsRepository
    {
        Task<Domain.Model.Settings> GetSettingsAsync();
    }
}
