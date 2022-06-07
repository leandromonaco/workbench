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
        IRepositoryService<Settings> _settingsRepository;
        IConfigurationService _configurationService;
        ICachingService _cachingService;

        public GetSettingsQueryHandler(IRepositoryService<Settings> settingsRepository, IConfigurationService configurationService, ICachingService cachingService)
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
                var settings = await _settingsRepository.GetAsync(request.TenantId);
                _cachingService.Set(request.TenantId.ToString(), settings);
                cachedSettings = settings;
            }
            
            return cachedSettings;
        }
    }
}
