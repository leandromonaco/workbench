using System.Collections.Generic;
using System.Net.Mail;

namespace TeamAlignment.Core.Common.Utilities
{
    public class EmailService
    {
        private string _smtpServer;
        private int _port;

        public EmailService(string smtpServer, int port = 25)
        {
            _smtpServer = smtpServer;
            _port = port;
        }
        public void SendEmailToIndividual(string from, string to, string subject, string body)
        {
            var mail = new MailMessage(from, to) { IsBodyHtml = true };
            var client = new SmtpClient
            {
                Port = _port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = _smtpServer
            };
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
        }

        public void SendEmailToGroup(string from, List<string> recipients, string subject, string body)
        {
            var mail = new MailMessage() { IsBodyHtml = true };

            mail.From = new MailAddress(from);

            foreach (var recipient in recipients)
            {
                mail.To.Add(new MailAddress(recipient));
            }

            var client = new SmtpClient
            {
                Port = _port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = _smtpServer
            };
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
        }
    }
}
