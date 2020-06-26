using Intranet.DataAccess.Repository.IRepository;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Utilities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class MondayReminder : IMondayReminder
    {
        private readonly EmailOptions _emailOptions;
        private readonly IUnitOfWork _unitOfWork;

        public MondayReminder(
            IOptions<EmailOptions> emailOptions,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _emailOptions = emailOptions.Value;
        }

        public void SendEmail()
        {
            var orderHeader = _unitOfWork.OrderHeader.GetAll(c => c.OrderStatus == SD.StatusForAcknowledgement || c.OrderStatus == SD.StatusForRating);

            foreach (var order in orderHeader)
            {
                var subject = order.OrderStatus;
                var loginUser = order.LoginUser;
                var datetime = String.Format(DateTime.Now.ToShortDateString());
                var orderId = order.Id;
                var requestorEmail = order.RequestorEmail;

                #region HTML Body

                string HtmlBody =
                    "<html xmlns='http://www.w3.org/1999/xhtml'> " +
                    "<head>" +
                    "   <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" +
                    "   <title>Collateral Request App</title>" +
                    "</head>" +
                    "<body>" +
                    "   <table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                    "       <tr>" +
                    "           <td align='center' valign='top' bgcolor='#fff' style='background-color:lightgray;'>" +
                    "           <br>" +
                    "           <br>" +
                    "           <table width='600' border='0' cellspacing='0' cellpadding='0'>" +
                    "               <tr>" +
                    "                   <td height='70' align='left' valign='middle'></td>" +
                    "               </tr>" +
                    "               <tr>" +
                    "                   <td align='left' valign='top' bgcolor='#564319' style='background-color:darkcyan; font-family:Arial, Helvetica, sans-serif; padding:10px;'>" +
                    "                       <div style='font-size:36px; color:#ffffff;'>" +
                    "                           <b>" + subject + "</b>" +
                    "                       </div>" +
                    "                   <div style='font-size:13px; color:lightcyan;'>" +
                    "                       <b>" + DateTime.Now.ToShortDateString() + " : Collateral Request Application </b>" +
                    "                   </div>" +
                    "               </td>" +
                    "           </tr>" +
                    "           <tr>" +
                    "               <td align='left' valign='top' bgcolor='#ffffff' style='background-color:#ffffff;'>" +
                    "                   <table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                    "                       <tr>" +
                    "                           <td align='center' valign='middle' style='padding:10px; color:#564319; font-size:28px; font-family:Georgia, 'Times New Roman', Times, serif;'>" +
                    "                               Reminder! <br />" +
                    "                           </td>" +
                    "                       </tr>" +
                    "                   </table>" +
                    "                   <table width='95%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                    "                       <tr>" +
                    "                           <td width='100%' style='color:darkslategrey; font-family:Arial, Helvetica, sans-serif; padding:10px;'>" +
                    "                               <div style='font-size:16px;'>" +
                    "                               </div>" +
                    "                               <div style='font-size:12px;'>" +
                    "                                   <hr>" +
                    "                                   <b>" +
                    "                                       <center style='font-size: medium'>" +
                    "                                           Hi " + loginUser + ", you have a pending " + subject + ", complete it by clicking" +
                    "                                           <a href='http://qas.intranet.pttphils.com/CorpComm/Order/Details/'" + orderId + "'>Here</a>" +
                    "                                       </center>" +
                    "                                   </br>" +
                    "                                   <hr>" +
                    "                               </div>" +
                    "                           </td>" +
                    "                       </tr>" +
                    "                   </table>" +
                    "                   <table width='100%' border='0' align='center' cellpadding='0' cellspacing='0' style='margin-bottom:15px;'>" +
                    "                       <tr>" +
                    "                           <td align='left' valign='middle' style='padding:15px; font-family:Arial, Helvetica, sans-serif;'>" +
                    "                               <div style='font-size:20px; color:#564319;'>" +
                    "                                   <b></b>" +
                    "                               </div>" +
                    "                               <div style='font-size:16px; color:#525252;'>" +
                    "                               </div>" +
                    "                           </td>" +
                    "                       </tr>" +
                    "                   </table>" +
                    "                   <table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                    "                       <tr>" +
                    "                           <td align='left' valign='middle' style='padding:15px; background-color:darkcyan; font-family:Arial, Helvetica, sans-serif;'>" +
                    "                               <div style='font-size:20px; color:#fff;'>" +
                    "                                   <b></b>" +
                    "                               </div>" +
                    "                               <br>" +
                    "                               <div style='font-size:13px; color:aliceblue;'>" +
                    "                                   <br>" +
                    "                               </div>" +
                    "                           </td>" +
                    "                       </tr>" +
                    "                   </table>" +
                    "               </td>" +
                    "           </tr>" +
                    "       </table>" +
                    "       <br>" +
                    "       <br>" +
                    "   </td>" +
                    "   </tr>" +
                    "   </table>" +
                    "   </body>" +
                    "</html>";

                #endregion HTML Body

                string messageBody = string.Format(HtmlBody);

                var message = new MimeMessage();
                var builder = new BodyBuilder();
                message.From.Add(new MailboxAddress(_emailOptions.AuthEmail));
                message.To.Add(new MailboxAddress(requestorEmail));
                message.Subject = subject;
                builder.HtmlBody = messageBody;
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(_emailOptions.SMTPHostClient, _emailOptions.SMTPHostPort, _emailOptions.SMTPHostBool);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(_emailOptions.AuthEmail, _emailOptions.AuthPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }           
        }
    }
}