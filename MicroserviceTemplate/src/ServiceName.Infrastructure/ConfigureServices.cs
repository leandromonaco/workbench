using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using AWS.Logger;
using AWS.Logger.SeriLog;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Repositories;

namespace ServiceName.Infrastructure
{
    public static class ConfigureServices
    {
        static ConfigurationManager _configurationManager;
        
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            
            services.AddSingleton<IConfiguration>(GetConfiguration(configurationManager));
            services.AddScoped<IRepositoryService<Settings>, SettingsRepositoryService>();

            var loggingSink = _configurationManager["ModuleConfiguration:Logging:Sink"];

            switch (loggingSink)
            {
                case "Seq":
                    services.AddSingleton<ILogger>(GetCloudSeqLogger());
                    break;
                default:
                    services.AddSingleton<ILogger>(GetCloudWatchLogger());
                    break;
            }

            services.AddSingleton<IDistributedCache>(GetRedisCache());
            services.AddSingleton<IDynamoDBContext>(GetDynamoDBContext());

            return services;
        }

        private static IDistributedCache GetRedisCache()
        {
            var cache = new RedisCache(new RedisCacheOptions
            {
                Configuration = _configurationManager["ModuleConfiguration:ConnectionStrings:Redis"]
            });

            return cache;
        }

        private static ILogger GetCloudSeqLogger()
        {
            var serverUrl = _configurationManager["ModuleConfiguration:Logging:Seq:ServerUrl"];
            var apiKey = _configurationManager["ModuleConfiguration:Logging:Seq:ApiKey"];

            Log.Logger = new LoggerConfiguration()
                               .WriteTo.Console()
                               .WriteTo.Seq(serverUrl: serverUrl,
                                            apiKey: apiKey)
                               .CreateLogger();

            return Log.Logger;
        }

        private static ILogger GetCloudWatchLogger()
        {
            var accessKey = _configurationManager["ModuleConfiguration:Logging:AwsCloudWatch:AccessKey"];
            var secretKey = _configurationManager["ModuleConfiguration:Logging:AwsCloudWatch:SecretKey"];
            var regionEndpoint = _configurationManager["ModuleConfiguration:Logging:AwsCloudWatch:RegionEndpoint"];
            var localTestEndpoint = _configurationManager["ModuleConfiguration:Logging:AwsCloudWatch:LocalTestEndpoint"];
            var logGroupName = _configurationManager["ModuleConfiguration:Logging:AwsCloudWatch:LogGroupName"];

            AWSLoggerConfig configuration;

            //If logGroupName is empty it uses the default AWS Lambad Log Group
            if (!string.IsNullOrEmpty(logGroupName))
            {
                configuration = new(logGroupName)
                {
                    Region = regionEndpoint,
                    Credentials = new BasicAWSCredentials(accessKey, secretKey)
                };

                //used for local testing only
                if (!string.IsNullOrEmpty(localTestEndpoint))
                {
                    configuration.ServiceUrl = localTestEndpoint;
                }

                return new LoggerConfiguration().WriteTo.AWSSeriLog(configuration)
                                                  .WriteTo.Console()
                                                  .CreateLogger();
            }

            return new LoggerConfiguration().WriteTo.AWSSeriLog()
                                                  .WriteTo.Console()
                                                  .CreateLogger();            
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

        private static DynamoDBContext GetDynamoDBContext()
        {
            var accessKey = _configurationManager["ModuleConfiguration:ConnectionStrings:DynamoDb:AccessKey"];
            var secretKey = _configurationManager["ModuleConfiguration:ConnectionStrings:DynamoDb:SecretKey"];
            var regionEndpoint = RegionEndpoint.GetBySystemName(_configurationManager["ModuleConfiguration:ConnectionStrings:DynamoDb:RegionEndpoint"]);
            var localTestEndpoint = _configurationManager["ModuleConfiguration:ConnectionStrings:DynamoDb:LocalTestEndpoint"];
            
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = regionEndpoint
            };

            if (!string.IsNullOrEmpty(localTestEndpoint))
            {
                clientConfig.UseHttp = true;
                clientConfig.ServiceURL = localTestEndpoint;
            }

            var amazonDynamoDBClient = new AmazonDynamoDBClient(accessKey, secretKey, clientConfig);
            var dynamoDBContextConfig = new DynamoDBContextConfig() { ConsistentRead = true };
            return new DynamoDBContext(amazonDynamoDBClient, dynamoDBContextConfig);
        }
    }
}
