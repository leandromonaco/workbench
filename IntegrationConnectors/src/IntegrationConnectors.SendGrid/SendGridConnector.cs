using IntegrationConnectors.Common;
using System;

namespace IntegrationConnectors.SendGrid
{
    public class SendGridConnector: HttpConnector
    {
        public SendGridConnector(string baseUrl, string apiKey, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {

        }
    }
}
