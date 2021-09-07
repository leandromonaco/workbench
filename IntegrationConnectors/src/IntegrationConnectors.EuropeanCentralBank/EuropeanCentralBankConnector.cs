using IntegrationConnectors.Common;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;

namespace IntegrationConnectors.ForeignExchange
{
    public class EuropeanCentralBankConnector : HttpConnector
    {
        readonly string _url;
        public EuropeanCentralBankConnector(string url) : base(string.Empty, string.Empty, AuthenticationType.None)
        {
            _url = url;
        }

        public async Task<double> ConvertCurrencyAsync(double amount, string conversionFrom, string conversionTo)
        {
            var xml = await GetAsync(_url);
            XDocument doc = XDocument.Parse(xml);
            var fromRate = doc.Descendants().FirstOrDefault(n => n.Attribute("currency") != null && n.Attribute("currency").Value.Equals(conversionFrom)).Attribute("rate").Value;
            var toRate = doc.Descendants().FirstOrDefault(n => n.Attribute("currency") != null && n.Attribute("currency").Value.Equals(conversionTo)).Attribute("rate").Value;
            var unitRate = double.Parse(toRate) / double.Parse(fromRate);
            return amount * unitRate;
        }
    }
}


