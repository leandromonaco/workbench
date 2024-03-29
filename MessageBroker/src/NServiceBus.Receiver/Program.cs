﻿using System;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;

public static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Sql.Receiver";

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Sql.Receiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
        var connection = @"Data Source=host.docker.internal,1433;Initial Catalog=NServiceBusDB;Persist Security Info=True;User ID=sa;Password=NS3rv1c3Bus";

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        transport.DefaultSchema("receiver");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");
        transport.UseSchemaForQueue("Samples.Sql.Sender", "sender");
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        var subscriptions = transport.SubscriptionSettings();
        subscriptions.CacheSubscriptionInformationFor(TimeSpan.FromMinutes(1));
        subscriptions.SubscriptionTableName(tableName: "Subscriptions", schemaName: "dbo");

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(OrderAccepted), "Samples.Sql.Sender");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("receiver");
        persistence.ConnectionBuilder(() => new SqlConnection(connection));
        persistence.TablePrefix("");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop().ConfigureAwait(false);
    }
}