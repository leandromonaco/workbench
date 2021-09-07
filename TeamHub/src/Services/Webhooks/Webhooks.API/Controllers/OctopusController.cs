using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using Webhooks.API.Database;
using Webhooks.API.Model.Deployment;

/// <summary>
/// https://octopus.com/blog/notifications-with-subscriptions-and-webhooks#webhook
/// </summary>
namespace AzDevOpsServiceHooks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OctopusController : ControllerBase
    {
        private readonly ILogger<OctopusController> _logger;

        public OctopusController(ILogger<OctopusController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void Post([FromBody] OctopusEvent value)
        {
            var json = JsonSerializer.Serialize(value);
            DatabaseRepository.SaveEvent(json);
        }
    }
}
