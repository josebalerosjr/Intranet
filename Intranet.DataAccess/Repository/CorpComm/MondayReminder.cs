using Intranet.DataAccess.Repository.IRepository;
using Intranet.DataAccess.Repository.IRepository.CorpComm;
using Intranet.Models.ViewModels.CorpComm;
using Intranet.Utilities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System;

namespace Intranet.DataAccess.Repository.CorpComm
{
    public class MondayReminder : IMondayReminder
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Emailer _emailer;

        [BindProperty]
        public OrderDetailsVM OrderVM { get; set; }

        public MondayReminder(
            IUnitOfWork unitOfWork,
            Emailer emailer)
        {
            _unitOfWork = unitOfWork;
            _emailer = emailer;
        }

        [Obsolete]
        public void SendEmail()
        {
            var orderHeader = _unitOfWork.OrderHeader.GetAll(
                c => c.OrderStatus == SD.StatusForAcknowledgement ||
                c.OrderStatus == SD.StatusForRating);

            foreach (var order in orderHeader)
            {
                var subject = "PTT COLLATERALS: You have a pending request!";
                var subject2 = order.OrderStatus.ToUpper();
                var loginUser = order.LoginUser;
                var datetime = String.Format(DateTime.Now.ToShortDateString());
                var orderId = order.Id;
                var requestorEmail = order.RequestorEmail;
                var ShippingDate = order.ShippingDate.ToShortDateString();
                var PickUpPoints = order.PickUpPoints;
                var RequestorEmail = order.RequestorEmail;

                #region get order details for email

                OrderVM = new OrderDetailsVM()
                {
                    OrderHeader = _unitOfWork.OrderHeader
                    .GetFirstOrDefault(u => u.Id == orderId),
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(
                        o => o.OrderId == orderId, includeProperties: "Collateral")
                };
                string itemlist = "";
                var itemname = "";
                int itemcount;

                foreach (var item in OrderVM.OrderDetails)
                {
                    itemname = item.Collateral.Name;
                    itemcount = item.Count;
                    itemlist +=
                        "<tr>" +
                        "   <td align='center'>" + itemname + "</td>" +
                        "   <td align='center'>" + itemcount + "</td>" +
                        "</tr>";
                }

                string listfinal =
                "   <table align='center' border='1' width='100%'>" +
                "       <tr>" +
                "           <td colspan='2' align='center'><strong> COLLATERALS </strong></td>" +
                "       </tr>" +
                "       <tr>" +
                "           <td align='center'> " +
                "                <strong>ITEM</strong>        " +
                "           </td>               " +
                "           <td align='center'> " +
                "               <strong>QUANTITY</strong>     " +
                "           </td>               " +
                "       </tr>                   " +
                        itemlist +
                "   </table> ";

                #endregion get order details for email

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
                    "                   <td align='left' valign='top' bgcolor='#564319' style='background-color:darkcyan; " +
                    "                       font-family:Arial, Helvetica, sans-serif; padding:10px;'>" +
                    "                       <div style='font-size:36px; color:#ffffff;'>" +
                    "                           <b>" + subject2 + "</b>" +
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
                    "                           <td align='center' valign='middle' style='padding:10px; color:#564319; font-size:28px; " +
                    "                               font-family:Georgia, 'Times New Roman', Times, serif;'>" +
                    "                           </td>" +
                    "                       </tr>" +
                    "                   </table>" +
                    "                   <table width='95%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                    "                       <tr>" +
                    "                           <td width='100%' " +
                    "                               style='color:darkslategrey; font-family:Arial, Helvetica, sans-serif; padding:10px;'>" +
                    "                               <div style='font-size:16px;'>" +
                    "                               </div>" +
                    "                               <div style='font-size:14px;'>" +
                    "                               <p>" +
                    "                                   Dear <strong>" + loginUser + "</strong>," +
                    "                               </p>" +
                    "                               <p style='text-align:center'>" +
                    "                                   <strong> You have a pending request " + order.OrderStatus.ToLower() + ", complete your request by clicking " +
                    "                                           <a href='" + SD.IntranetLink + "CorpComm/Order/Details/" + orderId + "'>here</a>.</strong>" +
                    "                               </p>    " +
                    "                               <p>" +
                    "                                   Please see request details below:" +
                    "                               </p>" +
                    "                               <p>" +
                    "                                   Request Number: <strong>" + orderId + "</strong> <br />" +
                    "                                   Shipping Date: <strong>" + ShippingDate + "</strong> <br />" +
                    "                                   Drop-off location: <strong>" + PickUpPoints + "</strong> <br />" +
                    "                               </p>" +
                    "                               <p>" + listfinal + "</p>" +
                    "                               <p>" +
                    "                                    Thank you!" +
                    "                               </p>" +
                    "                                    Best Regards," +
                    "                               </p>" +
                    "                                    <strong> CORPORATE COMMUNICATIONS DEPARTMENT</strong>" +
                    "                               </p>" +
                    "                           </div>" +
                    "                       </td>" +
                    "                   </tr> " +
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

                _emailer.SendMail(
                    SD.CormCommEmail,
                    requestorEmail,
                    messageBody,
                    subject,
                    SD.CormCommEmail,
                    SD.CormCommPass
                );
            }
        }
    }
}