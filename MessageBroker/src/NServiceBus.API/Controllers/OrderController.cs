using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NServiceBusAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task Post(OrderSubmitted orderSubmitted)
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.Sql.Sender");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            //Sender Configuration Begins

            var connection = @"Data Source=host.docker.internal,1433;Initial Catalog=NServiceBusDB;Persist Security Info=True;User ID=sa;Password=NS3rv1c3Bus";
            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(connection);
            transport.DefaultSchema("sender");
            transport.UseSchemaForQueue("error", "dbo");
            transport.UseSchemaForQueue("audit", "dbo");

            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            var subscriptions = transport.SubscriptionSettings();
            subscriptions.SubscriptionTableName(
                tableName: "Subscriptions",
                schemaName: "dbo");

            //Sender Configuration Finishes
            
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            await endpointInstance.Publish(orderSubmitted)
                .ConfigureAwait(false);

            _logger.LogInformation("Published OrderSubmitted message");
        }
    }
}
