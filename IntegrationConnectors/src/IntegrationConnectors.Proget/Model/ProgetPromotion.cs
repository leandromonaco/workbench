using System;

namespace IntegrationConnectors.Proget.Model
{
    public class ProgetPromotion
    {
        public string FromFeed { get; set; }
        public string ToFeed { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
        public int Id { get; set; }
    }
}