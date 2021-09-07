using System;

namespace IntegrationConnectors.AzDevOps.Model.Build
{
    public class AzDevOpsLatestBuild
    {
        public DateTime QueueTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
    }
}