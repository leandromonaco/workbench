using System.Text;
using Microsoft.Extensions.Configuration;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Configuration
{
    public class JsonStringConfigurationService : IConfigurationService
    {
        IConfiguration _configuration;
        public IConfiguration Configuration => _configuration;

        public void LoadFromSource(params string[] configurationSources)
        {
            _configuration = new ConfigurationBuilder()
                          .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(configurationSources[0])))
                          .Build();  //https://github.com/dotnet/runtime/issues/36018
        }
    }
}
