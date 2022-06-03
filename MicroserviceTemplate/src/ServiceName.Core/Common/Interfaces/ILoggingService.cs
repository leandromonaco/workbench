namespace ServiceName.Core.Common.Interfaces
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Log Message</param>
        /// <param name="propertyValues">Allows Structure Logging: https://softwareengineering.stackexchange.com/questions/312197/benefits-of-structured-logging-vs-basic-logging</param>
        /// <returns></returns>
        Task LogInformationAsync(string message, params object[] propertyValues);
        Task LogWarningAsync(string message, params object[] propertyValues);
        Task LogErrorAsync(string message, params object[] propertyValues);
    }
}
