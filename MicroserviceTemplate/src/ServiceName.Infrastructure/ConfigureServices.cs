using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AWS.Logger;
using AWS.Logger.SeriLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Caching;
using ServiceName.Infrastructure.Repositories;

namespace ServiceName.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddSingleton<IConfiguration>(GetConfiguration(configurationManager));
            services.AddScoped<IRepositoryService<Settings>, SettingsRepositoryService>();
            services.AddSingleton<ILogger>(GetCloudWatchLogger());
            //services.AddSingleton<ILogger>(GetCloudSeqLogger());

            services.AddSingleton<ICachingService, InMemoryCachingService>();
            services.AddSingleton<IDynamoDBContext>(GetLocalDynamoDB());

            return services;
        }

        private static ILogger GetCloudSeqLogger()
        {
            Log.Logger = new LoggerConfiguration()
                               .WriteTo.Console()
                               .WriteTo.Seq("http://localhost:5341")
                               .CreateLogger();

            return Log.Logger;
        }

        private static ILogger GetCloudWatchLogger()
        {
            AWSLoggerConfig configuration = new("/LocalStack/Microservice/Logs")
            {
                Region = "ap-southeast-2",
                ServiceUrl = "http://localhost:4566"
            };

            Log.Logger = new LoggerConfiguration().WriteTo.AWSSeriLog(configuration)
                                                  .WriteTo.Console()
                                                  .CreateLogger();

            return Log.Logger;
        }

        private static IConfiguration GetConfiguration(ConfigurationManager configurationManager)
        {
            configurationManager
                         .SetBasePath(Environment.CurrentDirectory)
                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                         .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables()
                         .Build();
    
            return configurationManager;
        }

        private static DynamoDBContext GetLocalDynamoDB()
        {
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName("ap-southeast-2"),
                UseHttp = true,
                ServiceURL = "http://localhost:8000"
            };

            var amazonDynamoDBClient = new AmazonDynamoDBClient("test", "test", clientConfig);
            var dynamoDBContextConfig = new DynamoDBContextConfig() { ConsistentRead = true };
            return new DynamoDBContext(amazonDynamoDBClient, dynamoDBContextConfig);
        }
    }
}
