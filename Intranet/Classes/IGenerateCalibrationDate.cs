using Intranet.Data;
using Intranet.Models;
using Intranet.Utilities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Linq;

namespace Intranet.Classes
{
    internal interface IGenerateCalibrationDate
    {
        void SendMail();
    }

    public class GenerateCalibrationDate : IGenerateCalibrationDate
    {
        private ItemRegContext _context;
        private readonly EmailOptions _emailOptions;
        private readonly InvEmailContext _contextInvEmail;

        public GenerateCalibrationDate(ItemRegContext context, IOptions<EmailOptions> emailOptions, InvEmailContext contextInvEmail)
        {
            _context = context;
            _emailOptions = emailOptions.Value;
            _contextInvEmail = contextInvEmail;
        }

        [Obsolete]
        public void SendMail()     // SendEmail function
        {
            var invemails = _contextInvEmail.invEmails;
            //  TODO: gets the list of emails in database and put in the invemails variable
            foreach (InvEmail e in invemails)               //  TODO: for each loop of emails send a list of email
            {
                var invemailadd = e.InvEmailAddress;        //  TODO: changes the value every loop
                var datetimenow = DateTime.Today;
                var message = new MimeMessage();
                var builder = new BodyBuilder();
                string msgFromDB = string.Empty;

                message.From.Add(new MailboxAddress(_emailOptions.AuthEmailQshe));
                message.To.Add(new MailboxAddress(invemailadd));

                var items = _context.ItemRegs;      //  TODO: gets the list of ItemRegs in database and put in the items variable
                foreach (ItemReg item in items)     //  TODO: for each loop of items send a list of criticall items from inventory database
                {
                    if (items.Count() != 0)
                    {
                        if (item.CalDate == datetimenow)
                        {
                            DateTime? dates = item.CalDate;

                            msgFromDB += "<tr>" +
                                        "<td>" + item.ItemName + "</td>" +
                                        "<td>" + item.ItemDesc + "</td>" +
                                        "<td>" + item.ManufName + "</td>" +
                                        "<td>" + item.AsstSerial + "</td>" +
                                        "<td>" + item.PartNum + "</td>" +
                                        "<td>" + item.TypeName + "</td>" +
                                        "<td>" + String.Format("{0:MM/dd/yyyy}", dates) + "</td>" +
                                        "<td>" + item.Qty + "</td>" +
                                        "<td>" + item.UnitName + "</td>" +
                                        "<td>" + item.Remarks + "</td>" +
                                        "<td>" + item.LocName + "</td>" +
                                        "</tr>";
                        }
                    }
                    else
                    {
                        msgFromDB += "<p>No Items for Calibratio</p>";
                    }
                }
                message.Subject = "Items to be calibrated";
                builder.HtmlBody =

                #region mail body

                    string.Format(@"
                        <p> Items to be calibrated " +
                            DateTime.Now.ToString() + " </p>" +
                        "<table border='1'> " +
                        "<thead>" +
                        "        <tr> " +
                        "            <th> Name </th>" +
                        "            <th> Item Description </th>" +
                        "            <th> MFR </th>" +
                        "            <th> Asset/SN </th>" +
                        "            <th> P/N </th>" +
                        "            <th> Type </th>" +
                        "            <th> CAL Date </th>" +
                        "            <th> QTY </th>" +
                        "            <th> Unit </th>" +
                        "            <th> Remarks </th>" +
                        "            <th> Location </th>" +
                        "        </tr>" +
                        "    </thead>" +
                        "    <tbody>" + msgFromDB + "</tbody> " +
                        "</table>" +
                        "<br />" +
                        "<br />");
                message.Body = builder.ToMessageBody();

                #endregion mail body

                using (var client = new SmtpClient())
                {
                    client.Connect(
                        _emailOptions.SMTPHostClient,
                        _emailOptions.SMTPHostPort,
                        _emailOptions.SMTPHostBool);
                    client.Authenticate(
                        _emailOptions.AuthEmailQshe,
                        _emailOptions.AuthPasswordQshe);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
        }
    }
}