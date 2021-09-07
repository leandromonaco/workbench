using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TeamAlignment.Core.Common.Http
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-circuit-breaker-pattern
    /// </summary>
    public class HttpService : IHttpService
    {
        HttpClient _client;

        public HttpService(IHttpClientFactory httpClient)
        {
           
        }

        private void Authenticate()
        {
            if (AuthenticationToken == null)
            {
                _client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            }
            else
            {
                _client = new HttpClient(new HttpClientHandler());
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthenticationToken);
                _client.DefaultRequestHeaders.Add("X-Octopus-ApiKey", AuthenticationToken);
            }
        }


        public string AuthenticationToken { get; set; }

        public async Task<string> GetAsync(string requestUri)
        {
            Authenticate();
            var result = await _client.GetAsync(requestUri);
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string requestUri, string stringContent)
        {
            Authenticate();
            //var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var content = new StringContent(stringContent);
            var result = await _client.PostAsync(requestUri, content);
            return await result.Content.ReadAsStringAsync();
        }
    }
}
