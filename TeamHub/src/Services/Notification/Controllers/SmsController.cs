using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Model;
using System.Text.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly ILogger<SmsController> _logger;
        private readonly IConfiguration _configuration;

        public SmsController(ILogger<SmsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post(SmsMessage sms)
        {
            _logger.LogInformation(1, "new sms request has been received", JsonSerializer.Serialize(sms));

            try
            {
                string accountSid = _configuration["Twilio:AccountSid"];
                string authToken = _configuration["Twilio:AuthToken"];

                TwilioClient.Init(accountSid, authToken);

                //Whatsapp Format -> whatsapp:+64279632374
                //SMS Format -> +64279632374

                var message = MessageResource.Create(
                                                        body: sms.Message,
                                                        from: new Twilio.Types.PhoneNumber(sms.From),
                                                        to: new Twilio.Types.PhoneNumber(sms.To)
                                                    );
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "sms could not be sent.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            _logger.LogInformation(1, "sms has been sent successfully", JsonSerializer.Serialize(sms));

            return StatusCode(StatusCodes.Status200OK);
        }


    }
}
