using System;

namespace IntegrationConnectors.Confluence.Model
{
    public class ConfluencePageLastUpdated
    {
        public ConfluencePageUpdatedBy By { get; set; }
        public DateTime When { get; set; }
    }
}
