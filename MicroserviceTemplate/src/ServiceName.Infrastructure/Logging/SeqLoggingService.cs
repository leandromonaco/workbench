using Serilog;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Logging
{
    public class SeqLoggingService : ILoggingService
    {
        /// <summary>
        /// Note: Seq already performs asynchronous batching natively.
        /// https://github.com/serilog/serilog-sinks-async
        /// </summary>
        public SeqLoggingService()
        {
            Log.Logger = new LoggerConfiguration()
                               .WriteTo.Console()
                               .WriteTo.Seq("http://localhost:5341")
                               .CreateLogger();
        }
        
        public async Task LogErrorAsync(string message, params object[] propertyValues)
        {
            Log.Error(message, propertyValues);
        }
        
        public async Task LogInformationAsync(string message, params object[] propertyValues)
        {
            Log.Information(message, propertyValues);
        }

        public async Task LogWarningAsync(string message, params object[] propertyValues)
        {
            Log.Warning(message, propertyValues);
        }
    }
}
