using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmsTwilioController : TwilioController
    {
        [HttpPost]
        public TwiMLResult Post(SmsRequest incomingMessage)
        {
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message("The copy cat says: " +
                                      incomingMessage.Body);

            return TwiML(messagingResponse);
        }
    }
}
