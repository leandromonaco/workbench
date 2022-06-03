using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Logging
{
    internal class CloudWatchLoggingService : ILoggingService
    {

        public CloudWatchLoggingService()
        {
            //// remove default logging providers
            //builder.Logging.ClearProviders();

            var awsConfig = new AmazonCloudWatchLogsConfig() { ServiceURL = "http://host.docker.internal:4566" };
            awsConfig.UseHttp = true;
            awsConfig.RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1;
            var client = new AmazonCloudWatchLogsClient("aws", "aws", awsConfig);

            // Serilog configuration        
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Verbose()
                                .WriteTo.AmazonCloudWatch(
                                                            logGroup: "/logs",
                                                            logStreamPrefix: DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"),
                                                            cloudWatchClient: client
                                                         ).CreateLogger();

            //Serilog.Debugging.SelfLog.Enable(Console.Error);

            //// Register Serilog
            //builder.Logging.AddSerilog(logger);
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
