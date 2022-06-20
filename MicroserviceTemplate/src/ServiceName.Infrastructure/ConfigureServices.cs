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
            var accessKey = _configurationManager["ModuleConfiguration:Infrastructure:Kms:AccessKey"];
            var secretKey = _configurationManager["ModuleConfiguration:Infrastructure:Kms:SecretKey"];
            var regionEndpoint = RegionEndpoint.GetBySystemName(_configurationManager["ModuleConfiguration:Infrastructure:Kms:RegionEndpoint"]);
            var localTestEndpoint = _configurationManager["ModuleConfiguration:Infrastructure:Kms:LocalTestEndpoint"];
            
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
            var redisServerName = _configurationManager["ModuleConfiguration:Infrastructure:Redis:Server"];
            var redisPort = _configurationManager["ModuleConfiguration:Infrastructure:Redis:Port"];

            var redisConnectionString = $"{redisServerName}:{redisPort}";
            
            var cache = new RedisCache(new RedisCacheOptions
            {
                Configuration = redisConnectionString
            });

            return cache;
        }


        private static ILogger GetLogger()
        {

            var loggingSink = _configurationManager["Logging:Sink"];

            switch (loggingSink)
            {
                case "Seq":
                    
                    var serverUrl = _configurationManager["ModuleConfiguration:Infrastructure:Seq:ServerUrl"];
                    var apiKey = _configurationManager["ModuleConfiguration:Infrastructure:Seq:ApiKey"];

                    return new LoggerConfiguration()
                                       .WriteTo.Console()
                                       .WriteTo.Seq(serverUrl: serverUrl,
                                                    apiKey: apiKey)
                                       .CreateLogger();

                case "CloudWatchLogs":
                    
                    var accessKey = _configurationManager["ModuleConfiguration:Infrastructure:CloudWatchLogs:AccessKey"];
                    var secretKey = _configurationManager["ModuleConfiguration:Infrastructure:CloudWatchLogs:SecretKey"];
                    var regionEndpoint = _configurationManager["ModuleConfiguration:Infrastructure:CloudWatchLogs:RegionEndpoint"];
                    var localTestEndpoint = _configurationManager["ModuleConfiguration:Infrastructure:CloudWatchLogs:LocalTestEndpoint"];
                    var logGroupName = _configurationManager["ModuleConfiguration:Infrastructure:CloudWatchLogs:LogGroupName"];

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
            var accessKey = _configurationManager["ModuleConfiguration:Infrastructure:DynamoDb:AccessKey"];
            var secretKey = _configurationManager["ModuleConfiguration:Infrastructure:DynamoDb:SecretKey"];
            var regionEndpoint = RegionEndpoint.GetBySystemName(_configurationManager["ModuleConfiguration:Infrastructure:DynamoDb:RegionEndpoint"]);
            var localTestEndpoint = _configurationManager["ModuleConfiguration:Infrastructure:DynamoDb:LocalTestEndpoint"];

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
