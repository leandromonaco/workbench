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
            app.MapGet("/settings/{tenantId}", async (IMediator mediator, string tenantId) => await mediator.Send(new GetSettingsQueryRequest() { TenantId = Guid.Parse(tenantId) }));
            app.MapPost("/settings/{tenantId}", async (IMediator mediator, [FromBody] Settings settings, string tenantId) => await mediator.Send(new SaveSettingsCommandRequest() { TenantId = Guid.Parse(tenantId), Settings = settings }));
        }
    }
}
