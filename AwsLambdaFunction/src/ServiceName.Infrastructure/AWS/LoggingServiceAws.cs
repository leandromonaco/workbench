using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.CloudWatchLogs;
using Serilog;
using Settings.Application.Common.Interfaces;

namespace Settings.Infrastructure.AWS
{
    internal class LoggingServiceAws : ILoggingService
    {
        public LoggingServiceAws()
        {
            //// remove default logging providers
            //builder.Logging.ClearProviders();

            //var awsConfig = new AmazonCloudWatchLogsConfig() { ServiceURL = "http://host.docker.internal:4566" };
            //awsConfig.UseHttp = true;
            //awsConfig.RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1;
            //var client = new AmazonCloudWatchLogsClient("aws", "aws", awsConfig);

            //// Serilog configuration        
            //var logger = new LoggerConfiguration()
            //                    .MinimumLevel.Verbose()
            //                    .WriteTo.AmazonCloudWatch(
            //                                                logGroup: "/logs",
            //                                                logStreamPrefix: DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"),
            //                                                cloudWatchClient: client
            //                                             ).CreateLogger();
            //// Register Serilog
            //builder.Logging.AddSerilog(logger);

            //Serilog.Debugging.SelfLog.Enable(Console.Error);
        }
        
        public void LogError(string message)
        {
            throw new NotImplementedException();
        }

        public void LogInformation(string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message)
        {
            throw new NotImplementedException();
        }
    }
}
