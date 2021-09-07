using IntegrationConnectors.Common.JsonConverters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntegrationConnectors.Common
{
    public class HttpConnector
    {
        private HttpClient _httpClient;
        protected string _url;
        protected JsonSerializerOptions _jsonSerializerOptions;

        public HttpConnector(string url, string key, AuthenticationType authType)
        {
            _url = url;
            _httpClient = new HttpClient(new HttpClientHandler());
            if (!authType.Equals(AuthenticationType.None))
            {
                AddAuthenticantionHeader(key, authType);
            }

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

        private void AddAuthenticantionHeader(string key, AuthenticationType authType)
        {
            switch (authType)
            {
                case AuthenticationType.Basic:
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {key}");
                    break;
                case AuthenticationType.Bearer:
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
                    break;
                case AuthenticationType.DefaultCredentials:
                    _httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
                    break;
                case AuthenticationType.Octopus:
                    _httpClient.DefaultRequestHeaders.Add("X-Octopus-ApiKey", key);
                    break;
                case AuthenticationType.Proget:
                    _httpClient.DefaultRequestHeaders.Add("X-ApiKey", key);
                    break;
                case AuthenticationType.Fortify:
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"FortifyToken {key}");
                    break;
                default:
                    throw new Exception("AuthenticationType is not implemented");
                    break;
            }
        }

        public async Task<string> GetAsync(string requestUri)
        {
            var result = await _httpClient.GetAsync(requestUri);
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string requestUri, string jsonContent)
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
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
