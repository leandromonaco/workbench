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
            app.MapGet("/settings", (IMediator mediator) => mediator.Send(new GetSettingsQueryRequest() { TenantId = Guid.Parse("2e861859-1be6-4c0d-bfce-1e9d9d31a1c9") }));
            app.MapPost("/settings", (IMediator mediator, [FromBody] Settings settings) => mediator.Send(new SaveSettingsCommandRequest() { TenantId = Guid.Parse("2e861859-1be6-4c0d-bfce-1e9d9d31a1c9"), Settings = settings }));
        }
    }
}
