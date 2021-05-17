using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Application.Senders
{
    public interface IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = true);
    }

    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = true)
        {
            using var client = new SmtpClient();

            var credentials = new NetworkCredential()
            {
                UserName = "esmaeilemami.dev", // without @gmail.com
                Password = "S0313167321E"
            };

            client.Credentials = credentials;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;

            using var emailMessage = new MailMessage()
            {
                To = { new MailAddress(toEmail) },
                From = new MailAddress("esmaeilemami.dev@gmail.com"), // with @gmail.com
                Subject = subject,
                Body = message,
                IsBodyHtml = isMessageHtml
            };

            client.Send(emailMessage);

            return Task.CompletedTask;
        }
    }
}
