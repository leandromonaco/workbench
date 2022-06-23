using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Core.CQRS.Commands
{
    public record SaveSettingsCommandRequest : IRequest<bool>
    {
        public Guid TenantId { get; set; }
        public Settings Settings { get; set; }
    }

    public class CreateTodoListCommandHandler : IRequestHandler<SaveSettingsCommandRequest, bool>
    {
        private IRepositoryService<Settings> _settingsRepository;
        private IConfiguration _configuration;
        private IDistributedCache _cache;

        public CreateTodoListCommandHandler(IRepositoryService<Settings> settingsRepository, IConfiguration configuration, IDistributedCache cache)
        {
            _settingsRepository = settingsRepository;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<bool> Handle(SaveSettingsCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _settingsRepository.SaveAsync(request.TenantId, request.Settings);
            await _cache.SetStringAsync(request.TenantId.ToString(), JsonSerializer.Serialize(request.Settings));
            return result;
        }
    }
}