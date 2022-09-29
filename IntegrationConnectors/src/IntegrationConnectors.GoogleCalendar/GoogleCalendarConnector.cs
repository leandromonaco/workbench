using System.Text.Json;
using IntegrationConnectors.Common;
using IntegrationConnectors.GoogleCalendar.Model;

namespace IntegrationConnectors.GoogleCalendar
{
    public class GoogleCalendarConnector : HttpConnector
    {
        readonly string _apiKey;
        public GoogleCalendarConnector(string apiKey) : base(string.Empty, string.Empty, AuthenticationType.None)
        {
            _apiKey = apiKey;
        }

        public async Task<List<GoogleCalendarEvent>> GetPublicHolidaysAsync(Country country)
        {
            var calendarJson = await GetAsync($"https://www.googleapis.com/calendar/v3/calendars/{GetCountryCode(country)}%23holiday%40group.v.calendar.google.com/events?key={_apiKey}");
            var calendar = JsonSerializer.Deserialize<Model.GoogleCalendar>(calendarJson)?.items;
            return calendar;
        }

        public async Task<List<GoogleCalendarEvent>> GetCalendarEventsAsync(string calendarId)
        {
            var calendarJson = await GetAsync($"https://www.googleapis.com/calendar/v3/calendars/{calendarId}%40group.calendar.google.com/events?key={_apiKey}");
            var calendar = JsonSerializer.Deserialize<Model.GoogleCalendar>(calendarJson)?.items;
            return calendar;
        }

        private string GetCountryCode(Country country)
        {
            switch (country)
            {
                case Country.Australia:
                    return "en.australian";
                    break;
                case Country.Pakistan:
                    return "en.pk";
                    break;
                case Country.Russia:
                    return "en.russian";
                    break;
                case Country.Philippines:
                    return "en.philippines";
                    break;
                default:
                    return string.Empty;
                    break;
            }
        }


        /*
            Here is updated on January 2020 list ISO 3166-2 to google calendar name:
            IMPORTANT! For all not listed countries ISO 3166-2 is used so i.e. for Albania it is al.

        {"au", "australian"},
        {"at", "austrian"},
        {"br", "brazilian"},
        {"bg", "bulgarian"},
        {"ca", "canadian"},
        {"cn", "china"},
        {"hr", "croatian"},
        {"cz", "czech"},
        {"dk", "danish"},
        {"fi", "finnish"},
        {"fr", "french"},
        {"de", "german"},
        {"gr", "greek"},
        {"hk", "hong_kong"},
        {"hu", "hungarian"},
        {"in", "indian"},
        {"id", "indonesian"},
        {"ie", "irish"},
        {"il", "jewish"},
        {"it", "italian"},
        {"jp", "japanese"},
        {"lv", "latvian"},
        {"lt", "lithuanian"},
        {"my", "malaysia"},
        {"mx", "mexican"},
        {"nl", "dutch"},
        {"nz", "new_zealand"},
        {"no", "norwegian"},
        {"ph", "philippines"},
        {"pl", "polish"},
        {"pt", "portuguese"},
        {"ro", "romanian"},
        {"ru", "russian"},
        {"sa", "saudiarabian"},
        {"sg", "singapore"},
        {"sk", "slovak"},
        {"si", "slovenian"},
        {"kr", "south_korea"},
        {"es", "spain"},
        {"se", "swedish"},
        {"tw", "taiwan"},
        {"tl", "thai"},
        {"tr", "turkish"},
        {"ua", "ukrainian"},
        {"us", "usa"},
        {"vn", "vietnamese"}

         
         */
    }
}