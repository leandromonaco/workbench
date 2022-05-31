namespace Settings.Application.Common.Interfaces
{
    public interface ILoggingService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
