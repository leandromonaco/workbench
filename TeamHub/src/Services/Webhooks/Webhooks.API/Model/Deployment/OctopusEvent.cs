using System;

namespace Webhooks.API.Model.Deployment
{
    public class OctopusEvent
    {
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
    }
}
