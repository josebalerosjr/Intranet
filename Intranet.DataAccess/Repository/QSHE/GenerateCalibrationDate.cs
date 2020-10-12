using Intranet.Data.QSHE;
using Intranet.DataAccess.Repository.IRepository.QSHE;
using Intranet.Models.QSHE;
using Intranet.Utilities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Linq;

namespace Intranet.DataAccess.Repository.QSHE
{
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
        public void SendEmail()     // SendEmail function
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
                int counter = 0;

                message.From.Add(new MailboxAddress(_emailOptions.AuthEmailQshe));
                message.To.Add(new MailboxAddress(invemailadd));

                var items = _context.ItemRegs;      //  TODO: gets the list of ItemRegs in database and put in the items variable
                foreach (ItemReg item in items)     //  TODO: for each loop of items send a list of criticall items from inventory database
                {
                    if (items.Count() != 0)
                    {
                        if (item.CalDate <= datetimenow)
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
                            counter++;
                        }
                    }
                    else
                    {
                        msgFromDB += "<p> No Items for Calibratio </p>";
                    }
                }
                message.Subject = "Items to be calibrated";

                if (counter == 0)
                {
                    #region HTML Body

                    builder.HtmlBody =
                        "<html xmlns='http://www.w3.org/1999/xhtml'>																					" +
                        "<head>																															" +
                        "    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />														" +
                        "    <title>QSHE Inventory</title>																								" +
                        "</head>																														" +
                        "																																" +
                        "<body>																															" +
                        "    <table width='100%' border='0' cellspacing='0' cellpadding='0'>															" +
                        "        <tr>																													" +
                        "            <td align='center' valign='top' bgcolor='#fff' style='background-color:lightgray;'>								" +
                        "                <br>																											" +
                        "                <br>																											" +
                        "                <table width='600' border='0' cellspacing='0' cellpadding='0'>													" +
                        "                    <tr>																										" +
                        "                        <td height='70' align='left' valign='middle'></td>														" +
                        "                    </tr>																										" +
                        "                    <tr>																										" +
                        "                        <td align='left' valign='top' bgcolor='#564319'														" +
                        "                            style='background-color:darkcyan; font-family:Arial, Helvetica, sans-serif; padding:10px;'>		" +
                        "                            <div style='font-size:36px; color:#ffffff;'>														" +
                        "                                <b>For Calibration</b>																			" +
                        "                            </div>																								" +
                        "                            <div style='font-size:13px; color:lightcyan;'>														" +
                        "                                <b> " + DateTime.Now.ToShortDateString() + " : QSHE Inventory </b>								" +
                        "                            </div>																								" +
                        "                        </td>																									" +
                        "                    </tr>																										" +
                        "                    <tr>																										" +
                        "                        <td align='left' valign='top' bgcolor='#ffffff' style='background-color:#ffffff;'>						" +
                        "                            <table width='100%' border='0' cellspacing='0' cellpadding='0'>									" +
                        "                                <tr>																							" +
                        "                                    <td align='center' valign='middle'															" +
                        "                                        style='padding:10px; color:#564319; font-size:28px;									" +
                        "                                        font-family:Georgia, ' Times New Roman', Times, serif;'>								" +
                        "                                    </td>																						" +
                        "                                </tr>																							" +
                        "                            </table>																							" +
                        "                            <table width='95%' border='0' align='center' cellpadding='0' cellspacing='0'>						" +
                        "                               <tr>																							" +
                        "                                   <td width='100%'																			" +
                        "                                        style='color:darkslategrey; font-family:Arial, Helvetica, sans-serif; padding:10px;'>	" +
                        "                                        <div style='font-size:16px;'>															" +
                        "                                        </div>																					" +
                        "                                        <div style='font-size:14px;'>															" +
                        "                                            <!--<p>																			" +
                        "                                                <strong></strong>,																" +
                        "                                            </p>																				" +
                        "                                            <p style='text-align:center'>														" +
                        "                                                <strong></strong>																" +
                        "                                            </p>-->																			" +
                        "                                            <p>																				" +
                        "                                                <!--Please see details below:-->										        " +
                        "                                            </p>																				" +
                        "                                            <!--<p>																			" +
                        "                                                Request Number: <strong>{2}</strong> <br />									" +
                        "                                                Shipping Date: <strong>{3}</strong> <br />										" +
                        "                                                Drop-off location: <strong>{4}</strong>										" +
                        "                                            </p>-->																			" +
                        "                                            <p>																				" +
                        "                                                THERE ARE NO FOR CALIBRATION AS OF TODAY. 							            " +
                        "                                            </p>                                                                               " +
                        "                                            <br> 																				" +
                        "                                            <!--<p>																			" +
                        "                                                Thank you!																		" +
                        "                                            </p>																				" +
                        "                                            <p>																				" +
                        "                                                Best Regards,																	" +
                        "                                            </p>-->																			" +
                        "                                            <p>																				" +
                        "                                                <strong>QSHE DEPARTMENT</strong>												" +
                        "                                            </p>																				" +
                        "                                        </div>																					" +
                        "                                    </td>																						" +
                        "                                </tr>																							" +
                        "                            </table>																							" +
                        "																																" +
                        "                            <table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'						" +
                        "                                   style='margin-bottom:15px;'>																" +
                        "                                <tr>																							" +
                        "                                    <td align='left' valign='middle'															" +
                        "                                        style='padding:15px; font-family:Arial, Helvetica, sans-serif;'>						" +
                        "                                        <div style='font-size:20px; color:#564319;'>											" +
                        "                                            <b></b>																			" +
                        "                                        </div>																					" +
                        "                                        <div style='font-size:16px; color:#525252;'>											" +
                        "                                        </div>																					" +
                        "                                    </td>																						" +
                        "                                </tr>																							" +
                        "                            </table>																							" +
                        "                            <table width='100%' border='0' cellspacing='0' cellpadding='0'>									" +
                        "                                <tr>																							" +
                        "                                    <td align='left' valign='middle'															" +
                        "                                        style='padding:15px; background-color:darkcyan;										" +
                        "                                        font-family:Arial, Helvetica, sans-serif;'>											" +
                        "                                        <div style='font-size:20px; color:#fff;'>												" +
                        "                                            <b></b>																			" +
                        "                                        </div>																					" +
                        "                                        <br>																					" +
                        "                                        <div style='font-size:13px; color:aliceblue;'>											" +
                        "																																" +
                        "                                            <br>																				" +
                        "                                        </div>																					" +
                        "                                    </td>																						" +
                        "                                </tr>																							" +
                        "                            </table>																							" +
                        "                        </td>																									" +
                        "                    </tr>																										" +
                        "                </table>																										" +
                        "                <br>																											" +
                        "                <br>																											" +
                        "            </td>																												" +
                        "        </tr>																													" +
                        "    </table>																													" +
                        "</body>																														" +
                        "</html>																														";

                    #endregion HTML Body
                }
                else
                {
                    #region HTML Body

                    builder.HtmlBody =
                        "<html xmlns='http://www.w3.org/1999/xhtml'>																					" +
                        "<head>																															" +
                        "    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />														" +
                        "    <title>QSHE Inventory</title>																								" +
                        "</head>																														" +
                        "																																" +
                        "<body>																															" +
                        "    <table width='100%' border='0' cellspacing='0' cellpadding='0'>															" +
                        "        <tr>																													" +
                        "            <td align='center' valign='top' bgcolor='#fff' style='background-color:lightgray;'>								" +
                        "                <br>																											" +
                        "                <br>																											" +
                        "                <table width='600' border='0' cellspacing='0' cellpadding='0'>													" +
                        "                    <tr>																										" +
                        "                        <td height='70' align='left' valign='middle'></td>														" +
                        "                    </tr>																										" +
                        "                    <tr>																										" +
                        "                        <td align='left' valign='top' bgcolor='#564319'														" +
                        "                            style='background-color:darkcyan; font-family:Arial, Helvetica, sans-serif; padding:10px;'>		" +
                        "                            <div style='font-size:36px; color:#ffffff;'>														" +
                        "                                <b>For Calibration</b>																			" +
                        "                            </div>																								" +
                        "                            <div style='font-size:13px; color:lightcyan;'>														" +
                        "                                <b> " + DateTime.Now.ToShortDateString() + " : QSHE Inventory </b>								" +
                        "                            </div>																								" +
                        "                        </td>																									" +
                        "                    </tr>																										" +
                        "                    <tr>																										" +
                        "                        <td align='left' valign='top' bgcolor='#ffffff' style='background-color:#ffffff;'>						" +
                        "                            <table width='100%' border='0' cellspacing='0' cellpadding='0'>									" +
                        "                                <tr>																							" +
                        "                                    <td align='center' valign='middle'															" +
                        "                                        style='padding:10px; color:#564319; font-size:28px;									" +
                        "                                        font-family:Georgia, ' Times New Roman', Times, serif;'>								" +
                        "                                    </td>																						" +
                        "                                </tr>																							" +
                        "                            </table>																							" +
                        "                            <table width='95%' border='0' align='center' cellpadding='0' cellspacing='0'>						" +
                        "                               <tr>																							" +
                        "                                   <td width='100%'																			" +
                        "                                        style='color:darkslategrey; font-family:Arial, Helvetica, sans-serif; padding:10px;'>	" +
                        "                                        <div style='font-size:16px;'>															" +
                        "                                        </div>																					" +
                        "                                        <div style='font-size:14px;'>															" +
                        "                                            <!--<p>																			" +
                        "                                                <strong></strong>,																" +
                        "                                            </p>																				" +
                        "                                            <p style='text-align:center'>														" +
                        "                                                <strong></strong>																" +
                        "                                            </p>-->																			" +
                        "                                            <p>																				" +
                        "                                                Please see details below:												        " +
                        "                                            </p>																				" +
                        "                                            <!--<p>																			" +
                        "                                                Request Number: <strong>{2}</strong> <br />									" +
                        "                                                Shipping Date: <strong>{3}</strong> <br />										" +
                        "                                                Drop-off location: <strong>{4}</strong>										" +
                        "                                            </p>-->																			" +
                        "                                            <p>																				" +
                        "                                               <table border='1'>                                                              " +
                        "                                                   <thead>                                                                     " +
                        "                                                       <tr>                                                                    " +
                        "                                                           <th> Name </th>                                                     " +
                        "                                                           <th> Item Description </th>                                         " +
                        "                                                           <th> MFR </th>                                                      " +
                        "                                                           <th> Asset/SN </th>                                                 " +
                        "                                                           <th> P/N </th>                                                      " +
                        "                                                           <th> Type </th>                                                     " +
                        "                                                           <th> CAL Date </th>                                                 " +
                        "                                                           <th> QTY </th>                                                      " +
                        "                                                           <th> Unit </th>                                                     " +
                        "                                                           <th> Remarks </th>                                                  " +
                        "                                                           <th> Location </th>                                                 " +
                        "                                                       </tr>                                                                   " +
                        "                                                   </thead>                                                                    " +
                        "                                                   <tbody>" + msgFromDB + "</tbody>                                            " +
                        "                                               </table>                                                                        " +
                        "                                            </p>																				" +
                        "                                            <!--<p>																			" +
                        "                                                Thank you!																		" +
                        "                                            </p>																				" +
                        "                                            <p>																				" +
                        "                                                Best Regards,																	" +
                        "                                            </p>-->																			" +
                        "                                            <p>																				" +
                        "                                                <strong>QSHE DEPARTMENT</strong>												" +
                        "                                            </p>																				" +
                        "                                        </div>																					" +
                        "                                    </td>																						" +
                        "                                </tr>																							" +
                        "                            </table>																							" +
                        "																																" +
                        "                            <table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'						" +
                        "                                   style='margin-bottom:15px;'>																" +
                        "                                <tr>																							" +
                        "                                    <td align='left' valign='middle'															" +
                        "                                        style='padding:15px; font-family:Arial, Helvetica, sans-serif;'>						" +
                        "                                        <div style='font-size:20px; color:#564319;'>											" +
                        "                                            <b></b>																			" +
                        "                                        </div>																					" +
                        "                                        <div style='font-size:16px; color:#525252;'>											" +
                        "                                        </div>																					" +
                        "                                    </td>																						" +
                        "                                </tr>																							" +
                        "                            </table>																							" +
                        "                            <table width='100%' border='0' cellspacing='0' cellpadding='0'>									" +
                        "                                <tr>																							" +
                        "                                    <td align='left' valign='middle'															" +
                        "                                        style='padding:15px; background-color:darkcyan;										" +
                        "                                        font-family:Arial, Helvetica, sans-serif;'>											" +
                        "                                        <div style='font-size:20px; color:#fff;'>												" +
                        "                                            <b></b>																			" +
                        "                                        </div>																					" +
                        "                                        <br>																					" +
                        "                                        <div style='font-size:13px; color:aliceblue;'>											" +
                        "																																" +
                        "                                            <br>																				" +
                        "                                        </div>																					" +
                        "                                    </td>																						" +
                        "                                </tr>																							" +
                        "                            </table>																							" +
                        "                        </td>																									" +
                        "                    </tr>																										" +
                        "                </table>																										" +
                        "                <br>																											" +
                        "                <br>																											" +
                        "            </td>																												" +
                        "        </tr>																													" +
                        "    </table>																													" +
                        "</body>																														" +
                        "</html>																														";

                    #endregion HTML Body
                }

                //#region mail body

                //    string.Format(@"
                //        <p> Items to be calibrated " +
                //            DateTime.Now.ToString() + " </p>" +
                //        "<table border='1'> " +
                //        "<thead>" +
                //        "        <tr> " +
                //        "            <th> Name </th>" +
                //        "            <th> Item Description </th>" +
                //        "            <th> MFR </th>" +
                //        "            <th> Asset/SN </th>" +
                //        "            <th> P/N </th>" +
                //        "            <th> Type </th>" +
                //        "            <th> CAL Date </th>" +
                //        "            <th> QTY </th>" +
                //        "            <th> Unit </th>" +
                //        "            <th> Remarks </th>" +
                //        "            <th> Location </th>" +
                //        "        </tr>" +
                //        "    </thead>" +
                //        "    <tbody>" + msgFromDB + "</tbody> " +
                //        "</table>" +
                //        "<br />" +
                //        "<br />");
                message.Body = builder.ToMessageBody();

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