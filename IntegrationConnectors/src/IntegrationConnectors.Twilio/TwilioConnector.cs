using IntegrationConnectors.Common;
using IntegrationConnectors.Twilio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationConnectors.Twilio
{
    public class TwilioConnector : HttpConnector
    {
        string _accountSid;
        public TwilioConnector(string baseUrl, string apiKey, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {
            //Account SID
            byte[] data = Convert.FromBase64String(apiKey);
            string decodedString = Encoding.UTF8.GetString(data);
            _accountSid = decodedString.Split(":")[0];
        }

        public async Task<string> SendSMSAsync(TwilioMessage twilioMessage) 
        {
            var parameters = new Dictionary<string, string>
            {
                { "From", twilioMessage.From },
                { "To", twilioMessage.To },
                { "Body", twilioMessage.Body }
            };
            
            var response = await PostAsync($"{_url}/2010-04-01/Accounts/{_accountSid}/Messages.json", parameters);

            return response;
        }

        public async Task<string> SendWhatsappAsync(TwilioMessage twilioMessage)
        {
            var parameters = new Dictionary<string, string>
            {
                { "From", $"whatsapp:{twilioMessage.From}" },
                { "To", $"whatsapp:{twilioMessage.To}" },
                { "Body", twilioMessage.Body }
            };

            var response = await PostAsync($"{_url}/2010-04-01/Accounts/{_accountSid}/Messages.json", parameters);

            return response;
        }
    }
}
