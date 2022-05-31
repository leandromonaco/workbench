using MediatR;
using Settings.Application.Interfaces;

namespace Settings.Application.SettingItems.Queries
{
    public record GetSettingsQueryRequest :IRequest<Domain.Model.Settings>
    {
        
    }

    public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQueryRequest, Domain.Model.Settings>
    {
        ISettingsRepository _settingsRepository;
            
        public GetSettingsQueryHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }
        
        public async Task<Domain.Model.Settings> Handle(GetSettingsQueryRequest request, CancellationToken cancellationToken)
        {
            return await _settingsRepository.GetSettingsAsync();
        }
    }
}
