namespace IntegrationConnectors.AzDevOps.Model.Backlog
{
    internal class AzDevOpsWorkItem
    {
        public int Id { get; set; }
        public int Rev { get; set; }
        public AzDevOpsWorkItemFields Fields { get; set; }
    }
}