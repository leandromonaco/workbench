using MediatR;
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
        IRepositoryService<Settings> _settingsRepository;
        IConfiguration _configuration;
        ICachingService _cachingService;

        public CreateTodoListCommandHandler(IRepositoryService<Settings> settingsRepository, IConfiguration configuration, ICachingService cachingService)
        {
            _settingsRepository = settingsRepository;
            _configuration = configuration;
            _cachingService = cachingService;
        }

        public async Task<bool> Handle(SaveSettingsCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _settingsRepository.SaveAsync(request.TenantId, request.Settings);
            _cachingService.Set(request.TenantId.ToString(), request.Settings);
            return result;
        }
    }
}
