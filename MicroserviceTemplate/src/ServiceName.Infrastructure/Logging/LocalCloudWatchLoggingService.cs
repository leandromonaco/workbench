using AWS.Logger;
using AWS.Logger.SeriLog;
using Serilog;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Logging
{
    internal class LocalCloudWatchLoggingService : ILoggingService
    {
        /// <summary>
        /// https://awscli.amazonaws.com/v2/documentation/api/latest/reference/logs/tail.html
        /// https://docs.aws.amazon.com/cli/latest/reference/logs/create-log-group.html
        /// awslocal logs describe-log-groups
        /// awslocal logs tail "/LocalStack/Microservice/Logs"
        /// https://docs.aws.amazon.com/lambda/latest/dg/csharp-logging.html (LambdaLogger)
        /// https://github.com/aws/aws-logging-dotnet
        /// https://github.com/aws/aws-logging-dotnet/tree/master/samples/Serilog
        /// </summary>
        public LocalCloudWatchLoggingService()
        {
            AWSLoggerConfig configuration = new("/LocalStack/Microservice/Logs")
            {
                Region = "ap-southeast-2",
                ServiceUrl = "http://localhost:4566"
            };

            Log.Logger = new LoggerConfiguration().WriteTo.AWSSeriLog(configuration)
                                                  .WriteTo.Console()
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
