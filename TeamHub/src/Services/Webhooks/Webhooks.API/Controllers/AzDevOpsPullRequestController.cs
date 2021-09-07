using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Webhooks.API.Database;
using Webhooks.API.Model.PullRequest;

/// <summary>
/// https://docs.microsoft.com/en-us/azure/devops/service-hooks/events?view=azure-devops#git.pullrequest.updated
/// </summary>
namespace AzDevOpsServiceHooks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzDevOpsPullRequestController : ControllerBase
    {
        private readonly ILogger<AzDevOpsPullRequestController> _logger;

        public AzDevOpsPullRequestController(ILogger<AzDevOpsPullRequestController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void Post([FromBody] AzDevOpsPullRequestEvent value)
        {
            var json = JsonSerializer.Serialize(value);
            DatabaseRepository.SaveEvent(json);
        }
    }
}
