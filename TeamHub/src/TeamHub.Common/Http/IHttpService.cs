using System.Net.Http;
using System.Threading.Tasks;

namespace TeamAlignment.Core.Common.Http
{
    public interface IHttpService
    {
        Task<string> GetAsync(string requestUri);
        Task<string> PostAsync(string requestUri, string jsonContent);

        string AuthenticationToken { get; set; }
    }
}