using IntegrationConnectors.Common;
using IntegrationConnectors.Proget.Model;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntegrationConnectors.Proget
{
    public class ProgetConnector : HttpConnector
    {
        public ProgetConnector(string baseUrl, string apiKey, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {
        }

        public async Task<List<ProgetPackage>> GetPromotionsAsync(string sourceFeed, string targetFeed)
        {
            var response = await GetAsync($"{_url}/promotions/list?fromFeed={sourceFeed}&toFeed={targetFeed}");
            var progetPackages = JsonSerializer.Deserialize<List<ProgetPackage>>(response, _jsonSerializerOptions);
            return progetPackages;
        }

        public async Task PromotePackagesAsync(List<ProgetPackagePromotion> packagePromotions)
        {
            foreach (var packagePromotion in packagePromotions)
            {
                var packagePromotionJson = JsonSerializer.Serialize(packagePromotion);
                await PostAsync($"{_url}/promotions/promote", packagePromotionJson);
            }
        }
    }
}
