using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Net.Smtp;

namespace Intranet.Utilities
{
    public class EmailSender
    {
        public readonly EmailOptions emailOptions;

        public EmailSender(IOptions<EmailOptions> options)
        {
            emailOptions = options.Value;
        }

        //public void SendMail(string emailfrom, string email, string subject, string htmlMessage, string username, string password, string domain, int port, bool smtpBool)
        //{
        //    var message = new MimeMessage();
        //    var builder = new BodyBuilder();
        //    message.From.Add(new MailboxAddress(emailfrom));
        //    message.To.Add(new MailboxAddress(email));
        //    message.Subject = subject;
        //    builder.HtmlBody = htmlMessage;
        //    message.Body = builder.ToMessageBody();
        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect(domain, port, smtpBool);
        //        client.AuthenticationMechanisms.Remove("XOAUTH2");
        //        // Note: only needed if the SMTP server requires authentication
        //        client.Authenticate(username, password);
        //        client.Send(message);
        //        client.Disconnect(true);
        //    }
        //}
    }
}
