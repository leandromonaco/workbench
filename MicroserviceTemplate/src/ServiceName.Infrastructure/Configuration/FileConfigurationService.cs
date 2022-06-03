using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Configuration
{
    public class FileConfigurationService : IConfigurationService
    {
        IConfiguration _configuration;
        public IConfiguration Configuration => _configuration;

        public void LoadFromSource(params string[] configurationSources)
        {
             _configuration = new ConfigurationBuilder()
                          .SetBasePath(Assembly.GetExecutingAssembly().Location)
                          .AddJsonFile(configurationSources[0], optional: false, reloadOnChange: true)
                          .Build();
        }

    }
}
