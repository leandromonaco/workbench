using IntegrationConnectors.ForeignExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationConnectors.Test
{
    

    public class EuropeanCentralBankTest
    {
        EuropeanCentralBankConnector _europeanCentralBankConnector;
        public EuropeanCentralBankTest()
        {
            _europeanCentralBankConnector = new EuropeanCentralBankConnector("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml?ddef5e0e245f85e45877c5687aa06a96");
        }


        [Fact]
        public async Task TestForeignExchange() 
        {
            var convertedCurrency = await _europeanCentralBankConnector.ConvertCurrencyAsync(5d, "NZD", "AUD");
        }
    }
}
