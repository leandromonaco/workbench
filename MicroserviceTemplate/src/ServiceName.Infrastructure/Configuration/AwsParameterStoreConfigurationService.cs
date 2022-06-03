using Microsoft.Extensions.Configuration;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Configuration
{
    /// <summary>
    /// https://github.com/aws/aws-dotnet-extensions-configuration
    /// </summary>
    public class AwsParameterStoreConfigurationService : IConfigurationService
    {
        IConfiguration _configuration;
        public IConfiguration Configuration => _configuration;

        public void LoadFromSource(params string[] configurationSources)
        {
            throw new NotImplementedException();
        }
    }
}
