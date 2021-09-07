using IntegrationConnectors.Common;
using IntegrationConnectors.Twilio;
using IntegrationConnectors.Twilio.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationConnectors.Test
{
    public class TwilioTest
    {
        IConfiguration _configuration;
        TwilioConnector _twilioConnector;

        public TwilioTest()
        {
            _configuration = new ConfigurationBuilder()
                            //.SetBasePath(outputPath)
                            .AddJsonFile("appsettings.json", optional: true)
                            .AddUserSecrets("cf9af699-d3c2-4090-8231-fd3a1cb45a5f")
                            .AddEnvironmentVariables()
                            .Build();


            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_configuration["Twilio:AccountSid"]}:{_configuration["Twilio:AuthToken"]}"));

            _twilioConnector = new TwilioConnector("https://api.twilio.com", key, AuthenticationType.Basic);
        }

        [Fact]
        async Task SendMessage()
        {
            var to = "+64279632374";

            var twilioMessage = new TwilioMessage() 
            { 
                                        From = "+16788330195",
                                        To = to,
                                        Body = "SMS Test"
            };

            var smsResult = await _twilioConnector.SendSMSAsync(twilioMessage);
            smsResult = await _twilioConnector.SendSMSAsync(twilioMessage);

            twilioMessage = new TwilioMessage()
            {
                From = "+14155238886",
                To = to,
                Body = "Whatsapp Test"
            };

            var waResult = await _twilioConnector.SendWhatsappAsync(twilioMessage);
        }
    }
}
