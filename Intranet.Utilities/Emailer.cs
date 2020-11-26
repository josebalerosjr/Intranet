using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace Intranet.Utilities
{
    public class Emailer
    {
        [Obsolete]
        public void SendMail(string emailfrom, string emailto, string messageBody, string subject, string username, string password)
        {
            var message = new MimeMessage();
            var builder = new BodyBuilder();
            message.From.Add(new MailboxAddress(emailfrom));
            message.To.Add(new MailboxAddress(emailto));
            message.Subject = subject;
            builder.HtmlBody = messageBody;
            message.Body = builder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect(SD.SMTPClient, SD.SMTPPort, SD.SMTPBool);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(username, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}