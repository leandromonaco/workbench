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
        ICachingService _cachingService;

        public GetSettingsQueryHandler(ISettingsRepository settingsRepository, IConfigurationService configurationService, ICachingService cachingService)
        {
            _settingsRepository = settingsRepository;
            _configurationService = configurationService;
            _cachingService = cachingService;
        }

        public async Task<Settings> Handle(GetSettingsQueryRequest request, CancellationToken cancellationToken)
        {
            var cachedSettings = _cachingService.Get<Settings>(request.TenantId.ToString());
            if (cachedSettings == null)
            {
                var settings = await _settingsRepository.GetSettingsAsync(request.TenantId);
                _cachingService.Set(request.TenantId.ToString(), settings);
                cachedSettings = settings;
            }
            
            return cachedSettings;
        }
    }
}
