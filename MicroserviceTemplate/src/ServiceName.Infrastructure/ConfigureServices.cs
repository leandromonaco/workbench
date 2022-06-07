using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Authentication;
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
                services.AddScoped<ISettingsRepository, LocalDynamoDbSettingsRepository>();
                services.AddSingleton<ILoggingService, LocalCloudWatchLoggingService>();
                //services.AddSingleton<ILoggingService, SeqLoggingService>();
                services.AddSingleton<IConfigurationService, FileConfigurationService>();
                services.AddSingleton<ICachingService, InMemoryCachingService>();
            }
            else
            {
                services.AddScoped<ISettingsRepository, DynamoDbSettingsRepository>();
                services.AddSingleton<ILoggingService, CloudWatchLoggingService>();
                services.AddSingleton<IConfigurationService, FileConfigurationService>();
            }

            return services;
        }
    }
}
