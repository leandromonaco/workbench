using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ServiceName.API.Extensions.HealthCheck;

namespace ServiceName.API.Extensions
{
    /// <summary>
    /// https://docs.steeltoe.io/api/v3/management/health.html
    /// https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health
    /// https://blog.zhaytam.com/2020/04/30/health-checks-aspnetcore/

    //AspNetCore.HealthChecks.UI which adds the UI.
    //AspNetCore.HealthChecks.UI.Client which turns our old response(e.g.Healthy) into a more detailed response.
    //AspNetCore.HealthChecks.UI.InMemory.Storage which saves the results in memory for the UI to use.

    /// </summary>
    public static class HealthCheckExtensions
    {
        public static void AddHealthCheckSupport(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var sqlServerName = configurationManager["ModuleConfiguration:Infrastructure:SqlServer:Server"];
            var sqlPort = configurationManager["ModuleConfiguration:Infrastructure:SqlServer:Port"];
            var sqlDatabase = configurationManager["ModuleConfiguration:Infrastructure:SqlServer:Database"];
            var sqlUsername = configurationManager["ModuleConfiguration:Infrastructure:SqlServer:Username"];
            var sqlPassword = configurationManager["ModuleConfiguration:Infrastructure:SqlServer:Password"];

            var sqlConnectionString = $"Data Source={sqlServerName},{sqlPort};Initial Catalog={sqlDatabase};User ID={sqlUsername};Password={sqlPassword}";

            var redisServerName = configurationManager["ModuleConfiguration:Infrastructure:Redis:Server"];
            var redisPort = configurationManager["ModuleConfiguration:Infrastructure:Redis:Port"];

            var redisConnectionString = $"{redisServerName}:{redisPort}";

            services.AddHealthChecks()
                    .AddCheck<DynamoDbHealthCheck>("dynamodb")
                    .AddSqlServer(sqlConnectionString)
                    .AddRedis(redisConnectionString);

            services.AddHealthChecksUI(s =>
            {
                s.AddHealthCheckEndpoint("ServiceName", "/health");
            }).AddInMemoryStorage();
        }

        public static void ConfigureHealthCheck(this WebApplication app)
        {
            app.UseHealthChecksUI(config => config.UIPath = "/health-ui");

            app.MapHealthChecksUI();

            app.MapHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                AllowCachingResponses = false
            });
        }
    }
}