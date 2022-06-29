using MediatR;
using ServiceName.Core.Model;

namespace ServiceName.Core.CQRS.Commands
{
    public record SaveSettingsCommandRequest : IRequest<bool>
    {
        public Guid TenantId { get; set; }
        public Settings Settings { get; set; }
    }
}