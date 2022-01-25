using IntegrationConnectors.Common;
using System;
using System.Threading.Tasks;

namespace IntegrationConnectors.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var wcfRepository = new HttpConnector("http://localhost:65530/Service.svc", "", AuthenticationType.None);

            var request = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"" >
                              <s:Body>
                                <GetData xmlns=""http://tempuri.org/"" >
                                  <value>3</value>
                                </GetData>
                              </s:Body>
                            </s:Envelope>";

            var result = await wcfRepository.PostAsync("http://localhost:65530/Service.svc", request, "http://tempuri.org/IService/GetData");
        }
    }
}
