using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.KeyManagementService;
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
using ServiceName.Infrastructure.Authentication;
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
            services.AddSingleton<ILogger>(GetLogger());
            services.AddSingleton<IDistributedCache>(GetRedisCache());
            services.AddSingleton<IDynamoDBContext>(GetDynamoDBContext());
            services.AddSingleton<IAmazonKeyManagementService>(GetAmazonKms());
            services.AddScoped<IRepositoryService<Settings>, SettingsRepositoryService>();
            services.AddScoped<IJwtAuthenticationService, AssymetricKmsJwtService>();

            return services;
        }

        private static IAmazonKeyManagementService GetAmazonKms()
        {
            var accessKey = _configurationManager["ModuleConfiguration:AwsServices:Kms:AccessKey"];
            var secretKey = _configurationManager["ModuleConfiguration:AwsServices:Kms:SecretKey"];
            var regionEndpoint = RegionEndpoint.GetBySystemName(_configurationManager["ModuleConfiguration:AwsServices:Kms:RegionEndpoint"]);
            var localTestEndpoint = _configurationManager["ModuleConfiguration:AwsServices:Kms:LocalTestEndpoint"];
            
            AmazonKeyManagementServiceConfig amazonKeyManagementServiceConfig = new()
            {
                RegionEndpoint = regionEndpoint,
            };

            if (!string.IsNullOrEmpty(localTestEndpoint))
            {
                amazonKeyManagementServiceConfig.UseHttp = true;
                amazonKeyManagementServiceConfig.ServiceURL = localTestEndpoint;
            }

            if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey))
            {
                return new AmazonKeyManagementServiceClient(accessKey, secretKey, amazonKeyManagementServiceConfig);
            }

            return new AmazonKeyManagementServiceClient(amazonKeyManagementServiceConfig);
        }

        private static IDistributedCache GetRedisCache()
        {
            var cache = new RedisCache(new RedisCacheOptions
            {
                Configuration = _configurationManager["ModuleConfiguration:ConnectionStrings:Redis"]
            });

            return cache;
        }


        private static ILogger GetLogger()
        {

            var loggingSink = _configurationManager["ModuleConfiguration:Logging:Sink"];

            switch (loggingSink)
            {
                case "Seq":
                    
                    var serverUrl = _configurationManager["ModuleConfiguration:Logging:Seq:ServerUrl"];
                    var apiKey = _configurationManager["ModuleConfiguration:Logging:Seq:ApiKey"];

                    return new LoggerConfiguration()
                                       .WriteTo.Console()
                                       .WriteTo.Seq(serverUrl: serverUrl,
                                                    apiKey: apiKey)
                                       .CreateLogger();

                case "CloudWatchLogs":
                    
                    var accessKey = _configurationManager["ModuleConfiguration:AwsServices:CloudWatchLogs:AccessKey"];
                    var secretKey = _configurationManager["ModuleConfiguration:AwsServices:CloudWatchLogs:SecretKey"];
                    var regionEndpoint = _configurationManager["ModuleConfiguration:AwsServices:CloudWatchLogs:RegionEndpoint"];
                    var localTestEndpoint = _configurationManager["ModuleConfiguration:AwsServices:CloudWatchLogs:LocalTestEndpoint"];
                    var logGroupName = _configurationManager["ModuleConfiguration:AwsServices:CloudWatchLogs:LogGroupName"];

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
                    
                default:
                    throw new Exception("Logger Sink is not supported.");

            }    
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
            var accessKey = _configurationManager["ModuleConfiguration:AwsServices:DynamoDb:AccessKey"];
            var secretKey = _configurationManager["ModuleConfiguration:AwsServices:DynamoDb:SecretKey"];
            var regionEndpoint = RegionEndpoint.GetBySystemName(_configurationManager["ModuleConfiguration:AwsServices:DynamoDb:RegionEndpoint"]);
            var localTestEndpoint = _configurationManager["ModuleConfiguration:AwsServices:DynamoDb:LocalTestEndpoint"];

            var dynamoDBContextConfig = new DynamoDBContextConfig() { ConsistentRead = true };
            
            AmazonDynamoDBConfig amazonDynamoDBConfig = new()
            {
                RegionEndpoint = regionEndpoint
            };

            if (!string.IsNullOrEmpty(localTestEndpoint))
            {
                amazonDynamoDBConfig.UseHttp = true;
                amazonDynamoDBConfig.ServiceURL = localTestEndpoint;
            }

            if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey))
            {
                var amazonDynamoDBClientWithCredentials = new AmazonDynamoDBClient(accessKey, secretKey, amazonDynamoDBConfig);
                return new DynamoDBContext(amazonDynamoDBClientWithCredentials, dynamoDBContextConfig);
            }

            var amazonDynamoDBClientWithoutCredentials = new AmazonDynamoDBClient(amazonDynamoDBConfig);
            return new DynamoDBContext(amazonDynamoDBClientWithoutCredentials, dynamoDBContextConfig);
        }
    }
}
