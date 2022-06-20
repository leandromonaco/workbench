using Asp.Versioning.Conventions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.CQRS.Commands;
using ServiceName.Core.CQRS.Queries;
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


            app.MapGet("/settings/{tenantId}", [Authorize] async (IMediator mediator, string tenantId) => await mediator.Send(new GetSettingsQueryRequest() { TenantId = Guid.Parse(tenantId) }))
               .WithApiVersionSet(versionSet)
               .MapToApiVersion(1.0);

            //app.MapGet("/settings/{tenantId}", [Authorize] async (string tenantId) => await Task.FromResult(tenantId))
            //  .WithApiVersionSet(versionSet)
            //  .MapToApiVersion(2.0);

            app.MapPost("/settings/{tenantId}", [Authorize] async (IMediator mediator, [FromBody] Settings settings, string tenantId) => await mediator.Send(new SaveSettingsCommandRequest() { TenantId = Guid.Parse(tenantId), Settings = settings }))
               .WithApiVersionSet(versionSet)
               .MapToApiVersion(1.0);

            //These endpoints are only for JWT Testing & Troubleshooting Purposes
            if (bool.Parse(configuration["ModuleConfiguration:Jwt:TestMode"]))
            {
                app.MapPost("/test/token/generate", [AllowAnonymous] async (IJwtAuthenticationService authService, string issuer, string audience) => await authService.GenerateTokenAsync(new ModuleIdentity(), 60, issuer, audience))
                   .WithApiVersionSet(versionSet)
                   .MapToApiVersion(1.0);

                app.MapPost("/test/token/validate", [AllowAnonymous] async (IJwtAuthenticationService authService, string token) => await authService.ValidateTokenAsync(token))
                   .WithApiVersionSet(versionSet)
                   .MapToApiVersion(1.0);
            }
        }
    }
}