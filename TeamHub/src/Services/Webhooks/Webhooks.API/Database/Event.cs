using System;

namespace Webhooks.API.Database
{
    public partial class Event
    {
        public Guid EventId { get; set; }
        public string EventData { get; set; }
    }
}
