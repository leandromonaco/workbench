using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Infrastructure.Authentication;
using ServiceName.Infrastructure.Configuration;
using ServiceName.Infrastructure.Logging;
using ServiceName.Infrastructure.Repositories;

namespace ServiceName.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {

            //For Development Environment
            //services.AddScoped<ISettingsRepository, MockSettingsRepository>();
            services.AddScoped<ISettingsRepository, DynamoDbSettingsRepository>();
            services.AddSingleton<ILoggingService, SeqLoggingService>();
            services.AddSingleton<IConfigurationService, JsonStringConfigurationService>();
            //services.AddSingleton<IAuthenticationService, AuthenticationServiceMock>();

            //For Cloud Environment
            //services.AddScoped<ISettingsRepository, DynamoDbSettingsRepository>();
            //services.AddSingleton<ILoggingService, CloudWatchLoggingService>();

            // Authentication
            //services.AddAuthentication("BasicAuthentication")
            //        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            //services.AddAuthorization();

            return services;
        }
    }
}
