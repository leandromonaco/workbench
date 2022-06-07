using System.Text;
using Microsoft.Extensions.Configuration;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Configuration
{
    public class JsonStringConfigurationService : IConfigurationService
    {
        IConfiguration _configuration;
        public IConfiguration Configuration => _configuration;

        public void LoadFromSource()
        {
            throw new NotImplementedException();
            //_configuration = new ConfigurationBuilder()
            //              .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            //              .Build();  //https://github.com/dotnet/runtime/issues/36018
        }
    }
}
