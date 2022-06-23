using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning.Conventions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            app.MapGet("/settings", [Authorize] async (IMediator mediator, [FromHeader] string authorization) => await mediator.Send(new GetSettingsQueryRequest() { TenantId = GetTenantIdFromJwt(authorization) }))
               .WithApiVersionSet(versionSet)
               .MapToApiVersion(1.0);

            //app.MapGet("/settings/{tenantId}", [Authorize] async (string tenantId) => await Task.FromResult(tenantId))
            //  .WithApiVersionSet(versionSet)
            //  .MapToApiVersion(2.0);

            app.MapPost("/settings", [Authorize] async (IMediator mediator, [FromHeader] string authorization, [FromBody] Settings settings) => await mediator.Send(new SaveSettingsCommandRequest() { TenantId = GetTenantIdFromJwt(authorization), Settings = settings }))
               .WithApiVersionSet(versionSet)
               .MapToApiVersion(1.0);
        }

        private static Guid GetTenantIdFromJwt(string token)
        {
            token = token.Replace("Bearer ", "");
            var jwtToken = new JwtSecurityToken(token);
            var tenantId = jwtToken.Payload.FirstOrDefault(p => p.Key.Equals("custom:tenantId")).Value.ToString();
            return Guid.Parse(tenantId);
        }
    }
}