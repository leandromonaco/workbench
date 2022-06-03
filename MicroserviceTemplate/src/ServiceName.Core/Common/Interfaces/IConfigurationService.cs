using Microsoft.Extensions.Configuration;

namespace ServiceName.Core.Common.Interfaces
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration
    /// </summary>
    public interface IConfigurationService
    {
        void LoadFromSource(params string[] configurationSources);
        public IConfiguration Configuration { get; }
    }
}
