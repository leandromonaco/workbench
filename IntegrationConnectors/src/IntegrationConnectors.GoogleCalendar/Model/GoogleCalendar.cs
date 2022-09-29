using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationConnectors.GoogleCalendar.Model
{
    public class GoogleCalendar
    {
        public string summary { get; set; }
        public string timeZone { get; set; }
        public DateTime updated { get; set; }
        public List<GoogleCalendarEvent> items { get; set; }

    }
}
