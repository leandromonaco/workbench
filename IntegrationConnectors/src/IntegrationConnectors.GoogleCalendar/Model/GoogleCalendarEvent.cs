
namespace IntegrationConnectors.GoogleCalendar.Model
{
    public class GoogleCalendarEvent
    {
        public string status { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public StartDate start { get; set; }
        public EndDate end { get; set; }
        public Creator creator { get; set; }

    }
}