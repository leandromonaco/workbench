using Microsoft.Extensions.Configuration;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Configuration
{
    internal class EnvironmentVariableConfigurationService : IConfigurationService
    {
        IConfiguration _configuration;
        public IConfiguration Configuration => _configuration;

        public void LoadFromSource(params string[] configurationSources)
        {
           _configuration = new ConfigurationBuilder()
                         .AddEnvironmentVariables()
                         .Build();
        }

    }
}
