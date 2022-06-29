using MediatR;
using ServiceName.Core.Model;

namespace ServiceName.Core.CQRS.Queries
{
    public record GetSettingsQueryRequest : IRequest<Settings>
    {
        public Guid TenantId { get; set; }
    }
}