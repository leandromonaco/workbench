﻿using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Core.CQRS.Queries
{
    public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQueryRequest, Settings>
    {
        private IRepositoryService<Settings> _settingsRepository;
        private IConfiguration _configuration;
        private IDistributedCache _cache;

        public GetSettingsQueryHandler(IRepositoryService<Settings> settingsRepository, IConfiguration configuration, IDistributedCache cache)
        {
            _settingsRepository = settingsRepository;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<Settings> Handle(GetSettingsQueryRequest request, CancellationToken cancellationToken)
        {
            Settings cachedSettings;

            var cachedSettingsJson = await _cache.GetStringAsync(request.TenantId.ToString());//TODO: If it fails, just log error

            if (string.IsNullOrEmpty(cachedSettingsJson))
            {
                var settings = await _settingsRepository.GetByIdAsync(request.TenantId); //TODO: If it fails, log error and throw
                await _cache.SetStringAsync(request.TenantId.ToString(), JsonSerializer.Serialize(settings));
                cachedSettings = settings;
            }
            else
            {
                cachedSettings = JsonSerializer.Deserialize<Settings>(cachedSettingsJson);
            }

            return cachedSettings;
        }
    }
}