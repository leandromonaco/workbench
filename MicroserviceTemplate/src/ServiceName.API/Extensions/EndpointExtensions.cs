using Asp.Versioning.Conventions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceName.Core.CQRS.Commands;
using ServiceName.Core.CQRS.Queries;
using ServiceName.Core.Model;

namespace ServiceName.API.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapEndpoints(this WebApplication app)
        {
            // define a 'version set' that applies to an API group
            var versionSet = app.NewApiVersionSet()
                                .HasApiVersion(1.0)
                                .HasApiVersion(2.0)
                                .ReportApiVersions()
                                .Build();


            app.MapGet("/settings/{tenantId}", async (IMediator mediator, string tenantId) => await mediator.Send(new GetSettingsQueryRequest() { TenantId = Guid.Parse(tenantId) }))
               .WithApiVersionSet(versionSet)
               .MapToApiVersion(1.0).HasApiVersion(1.0);

            app.MapGet("/settings/{tenantId}", async (string tenantId) => await Task.FromResult(tenantId))
              .WithApiVersionSet(versionSet)
              .MapToApiVersion(2.0).HasApiVersion(2.0);

            app.MapPost("/settings/{tenantId}", async (IMediator mediator, [FromBody] Settings settings, string tenantId) => await mediator.Send(new SaveSettingsCommandRequest() { TenantId = Guid.Parse(tenantId), Settings = settings }))
               .WithApiVersionSet(versionSet)
               .MapToApiVersion(1.0).HasApiVersion(1.0);
        }
    }
}
