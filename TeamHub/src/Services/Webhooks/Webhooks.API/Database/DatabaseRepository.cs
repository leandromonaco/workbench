using System;

namespace Webhooks.API.Database
{
    internal class DatabaseRepository
    {
        internal static void SaveEvent(string json)
        {
            WebhooksContext context = new WebhooksContext();
            context.Events.Add(new Event() { EventId = Guid.NewGuid(), EventData = json });
            context.SaveChanges();
        }
    }
}