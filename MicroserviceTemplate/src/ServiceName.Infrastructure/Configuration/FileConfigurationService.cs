using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Infrastructure.Configuration
{
    public class FileConfigurationService : IConfigurationService
    {
        IConfiguration _configuration;
        
        IOptions<ModuleOptions> _infrastructureOptions;
        
        public IConfiguration Configuration => _configuration;

        public void LoadFromSource()
        {
            
            var configFileName = _infrastructureOptions.Value.IsDevelopment ? "appsettings.Development.json" : "appsettings.json";
            
             _configuration = new ConfigurationBuilder()
                          .SetBasePath(Assembly.GetExecutingAssembly().Location)
                          .AddJsonFile(configFileName, optional: false, reloadOnChange: true)
                          .Build();
        }

    }
}
