using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Authorization;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.API.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapEndpoints(this WebApplication app, ConfigurationManager configuration)
        {
            // define a 'version set' that applies to an API group
            var versionSet = app.NewApiVersionSet()
                                .HasApiVersion(1.0)
                                //.HasApiVersion(2.0)
                                .ReportApiVersions()
                                .Build();

            app.MapPost("token/generate", [AllowAnonymous] async (IJwtAuthenticationService authService, string tenantId, string issuer, string audience) => await authService.GenerateTokenAsync(new ModuleIdentity() { InstanceGuid = tenantId }, 60, issuer, audience))
                   .WithApiVersionSet(versionSet)
                   .MapToApiVersion(1.0);

            app.MapPost("token/validate", [AllowAnonymous] async (IJwtAuthenticationService authService, string token) => await authService.ValidateTokenAsync(token))
               .WithApiVersionSet(versionSet)
               .MapToApiVersion(1.0);
        }
    }
}