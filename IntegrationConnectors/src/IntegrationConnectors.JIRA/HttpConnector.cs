using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using IntegrationConnectors.Common.JsonConverters;

namespace IntegrationConnectors.JIRA
{
    public class HttpConnector
    {
        private HttpClient _httpClient;
        protected string _url;
        protected JsonSerializerOptions _jsonSerializerOptions;

        public HttpConnector(string url, string user, string key)
        {
            _url = url;
            var atlassianAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{key}"));
            _httpClient = new HttpClient(new HttpClientHandler());
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {atlassianAuth}");

            _jsonSerializerOptions = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                MaxDepth = 0,
                Converters = {
                                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                                new Int64Converter(),
                                new DoubleConverter(),
                                new DateTimeConverter()
                             }
            };
        }

        private TimeSpan _timeout;
        public TimeSpan Timeout
        {
            get => _timeout;
            set
            {
                _timeout = value;
                _httpClient.Timeout = _timeout;
            }
        }

        public async Task<string> GetAsync(string requestUri)
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var result = await _httpClient.GetAsync(requestUri);
            return await result.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Used for REST Services
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="jsonContent"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string requestUri, string jsonContent)
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(requestUri, content);
            return await result.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Used for WCF Services
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="xmlContent"></param>
        /// <param name="soapAction"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string requestUri, string xmlContent, string soapAction)
        {
            var content = new StringContent(xmlContent, Encoding.UTF8, "text/xml");
            _httpClient.DefaultRequestHeaders.Add("SOAPAction", soapAction);
            var result = await _httpClient.PostAsync(requestUri, content);
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string requestUri, Dictionary<string, string> parameters)
        {
            var formUrlEncoded = new FormUrlEncodedContent(parameters);
            var result = await _httpClient.PostAsync(requestUri, formUrlEncoded);
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> PutAsync(string requestUri, string jsonContent)
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync(requestUri, content);
            return await result.Content.ReadAsStringAsync();
        }
    }
}
