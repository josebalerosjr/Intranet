using Intranet.Classes;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Models.ViewModels.CorpComm;
using Intranet.Utilities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[action]/{id?}")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        public readonly EmailOptions _emailOptions;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public OrderDetailsVM OrderVM { get; set; }

        //[BindProperty]
        //public ShoppingCartVM ShoppingCartVM { get; set; }

        public OrderController(
            IUnitOfWork unitOfWork,
            IOptions<AppSettings> appSettings,
            IOptions<EmailOptions> emailOptions,
            IWebHostEnvironment hostEnvironment
        )
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
            _emailOptions = emailOptions.Value;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            UserDetails();
            CartCount();
            return View();
        }

        public IActionResult Details(int id)
        {
            OrderVM = new OrderDetailsVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == id, includeProperties: "Collateral")
            };
            return View(OrderVM);
        }

        // Step 01
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.CIOAdmin + "," + SD.CorpCommAdmin)]
        public IActionResult ApproveRequest()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            // change the status from "For Approval" to "Approved"
            orderHeader.OrderStatus = SD.StatusApproved;
            orderHeader.RequestType = OrderVM.OrderHeader.RequestType;
            _unitOfWork.Save();

            var orderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderId == OrderVM.OrderHeader.Id);

            
            foreach (var order in orderDetails)
            {
                OrderHeader orderHeader2 = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

                // taken the stocks after approval
                Collateral collateral = _unitOfWork.Collateral.Get(order.CollateralId.Value);
                collateral.Count -= order.Count;

                // insert details to history
                History history = new History();
                history.RequestId = orderHeader.Id;
                history.RequestDate = orderHeader.OrderDate;
                history.LoginUser = orderHeader.LoginUser;
                history.CollateralName = collateral.Name;
                history.CollateralId = collateral.Id;
                history.EventType = orderHeader.EventName;
                history.Quantity = order.Count;
                history.StationEvent = orderHeader.StationEvent;
                history.EventDate = orderHeader.EventDate;
                history.ShippingDate = orderHeader.ShippingDate;
                history.DropOffPoint = orderHeader.PickUpPoints;
                history.rating = orderHeader.OrderRating;
                _unitOfWork.History.Add(history);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = SD.CIOAdmin + "," + SD.CorpCommAdmin)]
        public IActionResult RejectRequest(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            
            // set the status to "Rejected"
            orderHeader.OrderStatus = SD.StatusRejected;
            orderHeader.RejectReason = OrderVM.OrderHeader.RejectReason;

            // get the email template
            var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString() + "Email_Notifications_ForRejection.html";

            var subject = "PTT COLLATERALS: Your request has been rejected!";
            var subject2 = "REQUEST REJECTED";
            var datetime = String.Format(DateTime.Now.ToShortDateString());
            var OrderId = OrderVM.OrderHeader.Id;
            var ShippingDate = OrderVM.OrderHeader.ShippingDate.ToShortDateString();
            var PickUpPoints = OrderVM.OrderHeader.PickUpPoints;
            var LoginUser = OrderVM.OrderHeader.LoginUser;
            var RequestorEmail = OrderVM.OrderHeader.RequestorEmail;
            var RejectReason = OrderVM.OrderHeader.RejectReason;
            var clickhere = SD.IntranetLink + "CorpComm/Collateral/Catalog";

            string HtmlBody = "";

            using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
            {
                HtmlBody = streamReader.ReadToEnd();
            }

            // [0] subject
            // [1] date
            // [2] requestor
            // [3] reason
            // [4] url

            string messageBody = string.Format(HtmlBody, 
                subject2, 
                datetime, 
                LoginUser, 
                RejectReason, 
                clickhere);

            EmailSender(
                RequestorEmail,
                subject,
                messageBody
                );

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CancelRequest(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            // change the status from "For Approval" to "Canceled"
            orderHeader.OrderStatus = SD.StatusCancelled;

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // Step 02
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.CIOAdmin + "," + SD.CorpCommAdmin)]
        public IActionResult ForDeliveryRequest()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            
            // change status from "Approved" to "For Deliver"
            orderHeader.OrderStatus = SD.StatusForDelivery;

            orderHeader.ShippingDate = OrderVM.OrderHeader.ShippingDate;
            orderHeader.PickUpPoints = OrderVM.OrderHeader.PickUpPoints;

            var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString() + "Email_Notifications_ForDelivery.html";

            var subject = "PTT COLLATERALS: Your request is now for delivery!";
            var subject2 = "FOR DELIVERY";
            var datetime = String.Format(DateTime.Now.ToShortDateString());
            var OrderId = OrderVM.OrderHeader.Id;
            var ShippingDate = OrderVM.OrderHeader.ShippingDate.ToShortDateString();
            var PickUpPoints = OrderVM.OrderHeader.PickUpPoints;
            var LoginUser = OrderVM.OrderHeader.LoginUser;
            var RequestorEmail = OrderVM.OrderHeader.RequestorEmail;

            #region get order details for email

            OrderVM = new OrderDetailsVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderId),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == OrderId, includeProperties: "Collateral")
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

            string HtmlBody = "";

            using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
            {
                HtmlBody = streamReader.ReadToEnd();
            }

            // [0] subject
            // [1] date
            // [2] request number
            // [3] shipping date
            // [4] drop - off location
            // [5] requestor name
            // [6] item and count

            string messageBody = string.Format(HtmlBody, subject2, datetime, OrderId, ShippingDate, PickUpPoints, LoginUser, listfinal);

            EmailSender(
                RequestorEmail,
                subject,
                messageBody
                );

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // Step 03
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForAcknowledgementRequest()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = SD.StatusForAcknowledgement;

            var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString() + "Email_Notifications_ForAcknowledment.html";

            var subject = "PTT COLLATERALS: Your request is now for acknowledgement!";
            var subject2 = "FOR ACKNOWLEDGEMENT";
            var datetime = String.Format(DateTime.Now.ToShortDateString());
            var OrderId = OrderVM.OrderHeader.Id;
            var LoginUser = OrderVM.OrderHeader.LoginUser;
            var RequestorEmail = OrderVM.OrderHeader.RequestorEmail;
            var ShippingDate = orderHeader.ShippingDate.ToShortDateString();
            var PickUpPoints = OrderVM.OrderHeader.PickUpPoints;
            var clickhere = SD.IntranetLink + "CorpComm/Order/Details/" + OrderId;

            #region get order details for email

            OrderVM = new OrderDetailsVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderId),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == OrderId, includeProperties: "Collateral")
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

            string HtmlBody = "";

            using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
            {
                HtmlBody = streamReader.ReadToEnd();
            }

            // [0] subject
            // [1] date
            // [2] request number
            // [3] shipping date
            // [4] drop - off location
            // [5] requestor name
            // [6] item and count
            // [7] click here

            //string messageBody = string.Format(HtmlBody, subject,datetime,LoginUser,OrderVM.OrderHeader.Id);

            string messageBody = string.Format(HtmlBody, subject2, datetime, OrderId, ShippingDate, PickUpPoints, LoginUser, listfinal, clickhere);

            EmailSender(
                RequestorEmail,
                subject,
                messageBody
                );

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RequesDelivered(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusForRating;

            var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString() + "Email_Notifications_ForRating.html";

            var subject = "PTT COLLATERALS: Your request is now for rating!";
            var subject2 = "FOR RATING";
            var datetime = String.Format(DateTime.Now.ToShortDateString());
            var OrderId = orderHeader.Id;
            var LoginUser = orderHeader.LoginUser;
            var RequestorEmail = orderHeader.RequestorEmail;
            var clickhere = SD.IntranetLink + "CorpComm/Order/Details/" + OrderId;

            string HtmlBody = "";

            using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
            {
                HtmlBody = streamReader.ReadToEnd();
            }

            // [0] subject
            // [1] date
            // [2] requestor
            // [3] url

            string messageBody = string.Format(HtmlBody, 
                subject2, 
                datetime, 
                LoginUser, 
                clickhere);

            EmailSender(
                RequestorEmail,
                subject,
                messageBody
                );

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        ////[Route("")]
        ////[Route("Details/{id:int}")]
        //[Route("Order/Details/{id:int}", Name = "OrderForRating")]
        ////[Route("{id:int}")]
        public IActionResult AcknowledgeReceipt()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            // change the status from "For Acknowledgement " to "For Rating"
            orderHeader.OrderStatus = SD.StatusForRating;
            _unitOfWork.Save();

            return RedirectToAction(nameof(Details), new { id = OrderVM.OrderHeader.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OrderRating()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = SD.StatusCompleted;

            //string strRate = OrderVM.OrderHeader.OrderRating;

            orderHeader.OrderRating = OrderVM.OrderHeader.OrderRating;

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            UserDetails();

            string loginUser = ViewBag.DisplayName;

            IEnumerable<OrderHeader> orderHeadersList;

            if (User.IsInRole(SD.CIOAdmin) || User.IsInRole(SD.CorpCommAdmin))
            {
                orderHeadersList = _unitOfWork.OrderHeader.GetAll();
            }
            else
            {
                orderHeadersList = _unitOfWork.OrderHeader.GetAll(u => u.LoginUser == loginUser);
            }

            switch (status)
            {
                case "completed":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted);
                    break;

                case "forapproval":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusForApproval);
                    break;

                case "fordelivery":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusForDelivery);
                    break;

                case "foracknowledgement":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusForAcknowledgement);
                    break;

                case "forrating":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusForRating);
                    break;

                case "rejected":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusRejected);
                    break;

                case "approved":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusApproved);
                    break;

                case "canceled":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCancelled);
                    break;

                default:
                    break;
            }
            return Json(new { data = orderHeadersList });
        }

        #endregion API CALLS

        #region UserDetails function

        public void UserDetails()
        {
            var username = User.Identity.Name;
            var domain = _appSettings.appDomain;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
            }
        }

        public void CartCount()
        {
            string UserCart = ViewBag.DisplayName;
            if (UserCart != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.LoginUser == UserCart,
                    includeProperties: "Collateral").ToList().Count();
                HttpContext.Session.SetObject(SD.ssShoppingCart, count);
                ViewBag.ItemCount = count;
            }
        }

        #endregion UserDetails function

        #region EmailSender

        private void EmailSender(
            string RequestorEmail,
            string subject,
            string messageBody
        )
        {
            var message = new MimeMessage();
            var builder = new BodyBuilder();
            message.From.Add(new MailboxAddress(_emailOptions.AuthEmail));
            message.To.Add(new MailboxAddress(RequestorEmail));
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

        #endregion EmailSender
    }
}