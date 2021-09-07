using IntegrationConnectors.SonaType.Model;
using IntegrationConnectors.Common;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntegrationConnectors.SonaType
{
    public class SonaTypeConnector : HttpConnector
    {
        public SonaTypeConnector(string baseUrl, string apiKey, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {
        }

        public async Task<List<SonaTypeComponentScanResult>> ScanComponent(string coordinates)
        {
            var resultJson = await PostAsync($"{_url}/v3/component-report", coordinates);
            var result = JsonSerializer.Deserialize<List<SonaTypeComponentScanResult>>(resultJson, _jsonSerializerOptions);
            return result;
        }
    }
}
