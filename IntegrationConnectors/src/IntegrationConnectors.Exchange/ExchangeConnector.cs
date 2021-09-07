using IntegrationConnectors.Common;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace IntegrationConnectors.Exchange
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/exchange/client-developer/exchange-web-services/get-started-with-ews-managed-api-client-applications
    /// </summary>
    public class ExchangeConnector
    {
        ExchangeService exchangeService;

        public ExchangeConnector(string baseUrl, string apiKey, AuthenticationType authType)
        {
            byte[] data = Convert.FromBase64String(apiKey);
            string decodedToken = Encoding.UTF8.GetString(data);
            var tokenSplit = decodedToken.Split(":");
            var email = tokenSplit[0];
            var password = tokenSplit[1];

            //Connect to exchange
            exchangeService = new ExchangeService(ExchangeVersion.Exchange2013)
            {
                Url = new Uri(baseUrl),
                Credentials = new NetworkCredential(email, password)
            };
        }


        /// <summary>
        /// https://docs.microsoft.com/en-us/exchange/client-developer/exchange-web-services/how-to-get-appointments-and-meetings-by-using-ews-in-exchange
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<Appointment> GetAppointments(DateTime startDate, DateTime endDate)
        {
            //Set the ImpersonatedUserId property of the ExchangeService object to identify the impersonated user (target account).
            //This example uses the user's SMTP email address.
            //exchangeService.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, mailBox);

            // Initialize values for the start and end times, and the number of appointments to retrieve.
            const int NUM_APPTS = 50;
            // Initialize the calendar folder object with only the folder ID. 
            CalendarFolder calendar = CalendarFolder.Bind(exchangeService, WellKnownFolderName.Calendar, new PropertySet());
            // Set the start and end time and number of appointments to retrieve.
            CalendarView cView = new CalendarView(startDate, endDate, NUM_APPTS);
            // Limit the properties returned to the appointment's subject, start time, and end time.
            cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End);
            // Retrieve a collection of appointments by using the calendar view.

            FindItemsResults<Appointment> appointments = null;
            List<Appointment> allAppointments = new List<Appointment>();
            do
            {
                appointments = calendar.FindAppointments(cView);
                cView.StartDate = appointments.ToList()[appointments.Count() - 1].Start;
                allAppointments.AddRange(appointments.ToList());

            } while (appointments.MoreAvailable);

            //Set it back to null so that any actions that will be taken using the exchange service
            //applies to impersonating account (i.e.account used in network credentials)
            exchangeService.ImpersonatedUserId = null;

            return allAppointments;
        }


        public void SendAppointment(string emailAddress, string subject, string body, string location, DateTime datetime)
        {
            //Create the meeting
            var meeting = new Appointment(exchangeService);

            Attendee attendee = new Attendee();
            attendee.Address = emailAddress;
            meeting.RequiredAttendees.Add(attendee);

            // Set the properties on the meeting object to create the meeting.
            meeting.Subject = subject;
            meeting.Body = body;
            meeting.Start = datetime;
            meeting.End = meeting.Start.AddMinutes(5);
            meeting.Location = location;
            meeting.ReminderMinutesBeforeStart = 1;

            // Save the meeting to the Calendar folder and send the meeting request.
            meeting.Save(SendInvitationsMode.SendToAllAndSaveCopy);
        }
    }
}
