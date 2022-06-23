using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ServiceName.Core.Common.Behaviours;

namespace ServiceName.Core
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            return services;
        }
    }
}