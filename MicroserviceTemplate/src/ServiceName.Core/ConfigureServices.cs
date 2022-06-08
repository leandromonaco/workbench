using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ServiceName.Core.Common.Behaviours;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;
using ServiceName.Core.Repository;

namespace ServiceName.Core
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddScoped<IRepositoryService<Settings>, SettingsRepository>();
            return services;
        }
    }
}
