using System.Text.Json;
using MediatR;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Core.CQRS.Queries
{
    public record GetSettingsQueryRequest : IRequest<Settings>
    {
        public Guid TenantId { get; set; }
    }
    public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQueryRequest, Settings>
    {
        ISettingsRepository _settingsRepository;
        IConfigurationService _configurationService;

        public GetSettingsQueryHandler(ISettingsRepository settingsRepository, IConfigurationService configurationService)
        {
            _settingsRepository = settingsRepository;
            _configurationService = configurationService;
        }

        public async Task<Settings> Handle(GetSettingsQueryRequest request, CancellationToken cancellationToken)
        {
            var settings = await _settingsRepository.GetSettingsAsync(request.TenantId);
            return settings;
        }
    }
}
