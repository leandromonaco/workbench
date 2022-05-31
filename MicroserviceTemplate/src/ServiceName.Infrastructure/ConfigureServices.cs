using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Settings.Application.Interfaces;
using Settings.Infrastructure.Handlers;
using Settings.Infrastructure.Mock;
using Settings.Infrastructure.Repositories;

namespace Settings.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging)
        {
            services.AddScoped<ISettingsRepository, SettingsRepositoryMock>();
            //services.AddScoped<ISettingsService, SettingsServiceAws>();

            services.AddSingleton<Application.Common.Interfaces.IAuthenticationService, AuthenticationServiceMock>();

            // Authentication
            services.AddAuthentication("BasicAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddAuthorization();
            
            return services;
        }
    }
}
