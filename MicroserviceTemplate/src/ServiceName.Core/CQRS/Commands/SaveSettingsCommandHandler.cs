using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Core.CQRS.Commands
{
    public class SaveSettingsCommandHandler : IRequestHandler<SaveSettingsCommandRequest, bool>
    {
        private IRepositoryService<Settings> _settingsRepository;
        private IConfiguration _configuration;
        private IDistributedCache _cache;

        public SaveSettingsCommandHandler(IRepositoryService<Settings> settingsRepository, IConfiguration configuration, IDistributedCache cache)
        {
            _settingsRepository = settingsRepository;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<bool> Handle(SaveSettingsCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _settingsRepository.SaveAsync(request.TenantId, request.Settings); //TODO: If it fails, log error and throw
            await _cache.SetStringAsync(request.TenantId.ToString(), JsonSerializer.Serialize(request.Settings)); //TODO: If it fails, just log error
            return result;
        }
    }
}