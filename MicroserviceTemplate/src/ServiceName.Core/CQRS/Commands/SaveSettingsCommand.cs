﻿using MediatR;
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
        ISettingsRepository _settingsRepository;
        IConfigurationService _configurationService;
        ICachingService _cachingService;

        public CreateTodoListCommandHandler(ISettingsRepository settingsRepository, IConfigurationService configurationService, ICachingService cachingService)
        {
            _settingsRepository = settingsRepository;
            _configurationService = configurationService;
            _cachingService = cachingService;
        }

        public async Task<bool> Handle(SaveSettingsCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _settingsRepository.SaveSettingsAsync(request.TenantId, request.Settings);
            _cachingService.Set(request.TenantId.ToString(), request.Settings);
            return result;
        }
    }
}