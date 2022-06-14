using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Caching;
using ServiceName.Infrastructure.Logging;
using ServiceName.Infrastructure.Repositories;

namespace ServiceName.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddSingleton<IConfiguration>(GetConfiguration(configurationManager));
            services.AddScoped<IRepositoryService<Settings>, SettingsRepositoryService>();
            services.AddSingleton<ILoggingService, LocalCloudWatchLoggingService>();
            //services.AddSingleton<ILoggingService, SeqLoggingService>();

            services.AddSingleton<ICachingService, InMemoryCachingService>();
            services.AddSingleton<IDynamoDBContext>(GetLocalDynamoDB());

            return services;
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
