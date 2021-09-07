namespace IntegrationConnectors.Twilio.Model
{
    public class TwilioMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
    }
}