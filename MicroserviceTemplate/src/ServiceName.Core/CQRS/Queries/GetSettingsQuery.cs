using MediatR;
using Microsoft.Extensions.Configuration;
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
        IConfiguration _configuration;
        ICachingService _cachingService;

        public GetSettingsQueryHandler(IRepositoryService<Settings> settingsRepository, IConfiguration configuration, ICachingService cachingService)
        {
            _settingsRepository = settingsRepository;
            _configuration = configuration;
            _cachingService = cachingService;
        }

        public async Task<Settings> Handle(GetSettingsQueryRequest request, CancellationToken cancellationToken)
        {
            var cachedSettings = _cachingService.Get<Settings>(request.TenantId.ToString());
            if (cachedSettings == null)
            {
                var settings = await _settingsRepository.GetByIdAsync(request.TenantId);
                _cachingService.Set(request.TenantId.ToString(), settings);
                cachedSettings = settings;
            }
            
            return cachedSettings;
        }
    }
}
