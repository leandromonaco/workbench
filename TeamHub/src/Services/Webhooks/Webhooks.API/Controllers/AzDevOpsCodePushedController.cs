using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using Webhooks.API.Database;
using Webhooks.API.Model.Code;

/// <summary>
/// https://docs.microsoft.com/en-us/azure/devops/service-hooks/events?view=azure-devops#git.pullrequest.updated
/// </summary>
namespace AzDevOpsServiceHooks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzDevOpsCodePushedController : ControllerBase
    {
        private readonly ILogger<AzDevOpsCodePushedController> _logger;

        public AzDevOpsCodePushedController(ILogger<AzDevOpsCodePushedController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void Post([FromBody] AzDevOpsCodePushEvent value)
        {
            var json = JsonSerializer.Serialize(value);
            DatabaseRepository.SaveEvent(json);
        }
    }
}
