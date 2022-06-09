using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Caching;
using ServiceName.Infrastructure.Configuration;
using ServiceName.Infrastructure.Logging;
using ServiceName.Infrastructure.Repositories;

namespace ServiceName.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, Action<ModuleOptions>? infrastructureOptions = null)
        {
            var options = new ModuleOptions();
            services.Configure(infrastructureOptions);
            infrastructureOptions?.Invoke(options);
            
            if (options.IsDevelopment)
            {
                services.AddScoped<IRepositoryService<Settings>, SettingsRepositoryService>();
                services.AddSingleton<ILoggingService, LocalCloudWatchLoggingService>();
                //services.AddSingleton<ILoggingService, SeqLoggingService>();
                services.AddSingleton<IConfigurationService, FileConfigurationService>();
                services.AddSingleton<ICachingService, InMemoryCachingService>();
                services.AddSingleton<IDynamoDBContext>(GetLocalDynamoDB());
            }
            else
            {
                services.AddScoped<IRepositoryService<Settings>, SettingsRepositoryService>();
                services.AddSingleton<ILoggingService, CloudWatchLoggingService>();
                services.AddSingleton<IConfigurationService, FileConfigurationService>();
                services.AddSingleton<IDynamoDBContext>(GetLocalDynamoDB());
            }

            return services;
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
