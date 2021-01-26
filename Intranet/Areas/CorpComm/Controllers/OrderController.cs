using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Models.ViewModels.CorpComm;
using Intranet.Uti;
using Intranet.Utilities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[action]/{id?}")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public OrderDetailsVM OrderVM { get; set; }

        [BindProperty]
        public OrderDetailsVM OrderVMEmail { get; set; }

        //[BindProperty]
        //public ShoppingCartVM ShoppingCartVM { get; set; }

        public OrderController(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment hostEnvironment
        )
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            UserDetails();
            CartCount();
            return View();
        }

        public IActionResult Failed()
        {
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
            orderHeader.OrderStatus = SD.StatusApproved;
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // Done Reject
        [Authorize(Roles = SD.CIOAdmin + "," + SD.CorpCommAdmin)]
        [Obsolete]
        public IActionResult RejectRequest(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            // set the status to "Rejected"
            orderHeader.OrderStatus = SD.StatusRejected;
            orderHeader.RejectReason = OrderVM.OrderHeader.RejectReason;

            // get the email template
            var PathToFile = _hostEnvironment.WebRootPath +
                Path.DirectorySeparatorChar.ToString() + "Templates" +
                Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString() +
                "Email_Notifications_ForRejection.html";

            string hereurl;

            SD.Template = null;
            SD.Subject = null;
            SD.Subject2 = null;
            SD.OrderId = null;
            SD.ShippingDate = null;
            SD.PickUpPoints = null;
            SD.LoginUser = null;
            SD.RequestorEmail = null;
            SD.RejectReason = null;
            SD.clickhere = null;
            SD.items = null;


            SD.Template = "4";
            SD.Subject = "PTT COLLATERALS: Your request has been rejected!";
            SD.Subject2 = "REQUEST REJECTED";
            SD.OrderId = OrderVM.OrderHeader.Id.ToString();
            SD.ShippingDate = "";
            SD.PickUpPoints = "";
            SD.LoginUser = OrderVM.OrderHeader.LoginUser;
            SD.RequestorEmail = OrderVM.OrderHeader.RequestorEmail;
            SD.RejectReason = OrderVM.OrderHeader.RejectReason;
            hereurl = SD.IntranetLink + "CorpComm/Collateral/Catalog";
            SD.clickhere = "You can send another request by clicking <a href='" + hereurl + "'>here</a>";
            SD.items = "";

            _unitOfWork.Save();
            return RedirectToAction(nameof(SendEmail));
        }

        public IActionResult CancelRequest(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

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
            OrderHeader orderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            // change status from "Approved" to "For Deliver"
            orderHeader.OrderStatus = SD.StatusForDelivery;
            orderHeader.RequestType = OrderVM.OrderHeader.RequestType;
            orderHeader.ShippingDate = OrderVM.OrderHeader.ShippingDate;
            orderHeader.PickUpPoints = OrderVM.OrderHeader.PickUpPoints;
            _unitOfWork.Save();

            #region Email Process

            var OrderId = OrderVM.OrderHeader.Id;

            #region get order details for email

            OrderVMEmail = new OrderDetailsVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderId),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == OrderId, includeProperties: "Collateral")
            };

            string itemlist = "";
            var itemname = "";
            int itemcount;

            foreach (var item in OrderVMEmail.OrderDetails)
            {
                itemname = item.Collateral.Name;
                itemcount = item.Count;
                itemlist +=
                    "<tr>" +
                    "   <td align='center'>" + itemname + "</td>" +
                    "   <td align='center'>" + itemcount + "</td>" +
                    "</tr>";
            }

            #endregion get order details for email

            _unitOfWork.Save();

            #endregion Email Process

            #region History Process

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

            #endregion History Process

            //string hereurl;

            SD.Template = null;
            SD.Subject = null;
            SD.Subject2 = null;
            SD.OrderId = null;
            SD.ShippingDate = null;
            SD.PickUpPoints = null;
            SD.LoginUser = null;
            SD.RequestorEmail = null;
            SD.RejectReason = null;
            SD.clickhere = null;
            SD.items = null;

            SD.Template = "2";
            SD.Subject = "PTT COLLATERALS: Your request is now for delivery!";
            SD.Subject2 = "FOR DELIVERY";
            SD.OrderId = OrderId.ToString();
            SD.ShippingDate = OrderVM.OrderHeader.ShippingDate.ToShortDateString();
            SD.PickUpPoints = OrderVM.OrderHeader.PickUpPoints;
            SD.LoginUser = OrderVM.OrderHeader.LoginUser;
            SD.RequestorEmail = OrderVM.OrderHeader.RequestorEmail;
            SD.RejectReason = "";
            //hereurl = SD.IntranetLink + "CorpComm/Collateral/Catalog";
            SD.clickhere = "Your request has been approved and is now for delivery.";
            SD.items = itemlist;

            return RedirectToAction(nameof(SendEmail));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForAcknowledgementRequest()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = SD.StatusForAcknowledgement;

            var OrderId = OrderVM.OrderHeader.Id;

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

            #endregion get order details for email

            _unitOfWork.Save();

            string hereurl;

            SD.Template = null;
            SD.Subject = null;
            SD.Subject2 = null;
            SD.OrderId = null;
            SD.ShippingDate = null;
            SD.PickUpPoints = null;
            SD.LoginUser = null;
            SD.RequestorEmail = null;
            SD.RejectReason = null;
            SD.clickhere = null;
            SD.items = null;

            SD.Template = "2";
            SD.Subject = "PTT COLLATERALS: Your request is now for acknowledgement!";
            SD.Subject2 = "FOR ACKNOWLEDGEMENT";
            SD.OrderId = OrderVM.OrderHeader.Id.ToString();
            SD.ShippingDate = orderHeader.ShippingDate.ToShortDateString();
            SD.PickUpPoints = OrderVM.OrderHeader.PickUpPoints;
            SD.LoginUser = OrderVM.OrderHeader.LoginUser;
            SD.RequestorEmail = OrderVM.OrderHeader.RequestorEmail;
            SD.RejectReason = "";
            hereurl = SD.IntranetLink + "CorpComm/Order/Details/" + OrderId;
            SD.clickhere = "Kindly acknowledge receipt of your collaterals by clicking <a href=" + hereurl + ">here</a>";
            SD.items = itemlist;

            return RedirectToAction(nameof(SendEmail));
        }

        //public IActionResult RequesDelivered(int id)
        //{
        //    OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
        //    orderHeader.OrderStatus = SD.StatusForRating;

        //    var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
        //        + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
        //        + Path.DirectorySeparatorChar.ToString() + "Email_Notifications_ForRating.html";

        //    var subject = "PTT COLLATERALS: Your request is now for rating!";
        //    var subject2 = "FOR RATING";
        //    var datetime = String.Format(DateTime.Now.ToShortDateString());
        //    var OrderId = orderHeader.Id;
        //    var LoginUser = orderHeader.LoginUser;
        //    var RequestorEmail = orderHeader.RequestorEmail;
        //    var clickhere = SD.IntranetLink + "CorpComm/Order/Details/" + OrderId;

        //    string HtmlBody = "";

        //    using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
        //    {
        //        HtmlBody = streamReader.ReadToEnd();
        //    }

        //    // [0] subject
        //    // [1] date
        //    // [2] requestor
        //    // [3] url

        //    string messageBody = string.Format(HtmlBody,
        //        subject2,
        //        datetime,
        //        LoginUser,
        //        clickhere);

        //    EmailSender(
        //        RequestorEmail,
        //        subject,
        //        messageBody
        //        );

        //    _unitOfWork.Save();
        //    return RedirectToAction(nameof(Index));
        //}

        [Obsolete]
        public IActionResult AcknowledgeReceipt()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            // change the status from "For Acknowledgement " to "For Rating"
            orderHeader.OrderStatus = SD.StatusForRating;

            var OrderId = OrderVM.OrderHeader.Id;

            _unitOfWork.Save();

            string hereurl;

            SD.Template = null;
            SD.Subject = null;
            SD.Subject2 = null;
            SD.OrderId = null;
            SD.ShippingDate = null;
            SD.PickUpPoints = null;
            SD.LoginUser = null;
            SD.RequestorEmail = null;
            SD.RejectReason = null;
            SD.clickhere = null;
            SD.items = null;

            SD.Template = "3";
            SD.Subject = "PTT COLLATERALS: Your request is now for rating!";
            SD.Subject2 = "FOR RATING";
            SD.OrderId = OrderVM.OrderHeader.Id.ToString();
            SD.ShippingDate = orderHeader.ShippingDate.ToShortDateString();
            SD.PickUpPoints = OrderVM.OrderHeader.PickUpPoints;
            SD.LoginUser = OrderVM.OrderHeader.LoginUser;
            SD.RequestorEmail = OrderVM.OrderHeader.RequestorEmail;
            SD.RejectReason = "";
            hereurl = SD.IntranetLink + "CorpComm/Order/Details/" + OrderId;
            SD.clickhere = "You can send another request by clicking <a href='" + hereurl + "'>here</a>";
            SD.items = "Kindly rate the service rendered to you by clicking <a href='" + hereurl + "' target='_blank'>here</a>. <br/> We look forward to serving you again.";

            SendEmail();

            return RedirectToAction(nameof(Details), new { id = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OrderRating()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = SD.StatusCompleted;

            orderHeader.OrderRating = OrderVM.OrderHeader.OrderRating;

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SendEmail()
        {

            using (var client = new HttpClient())
            {
                // Create new instance of Person
                EmailSend emailSend = new EmailSend
                {
                    Template = SD.Template,
                    Subject = SD.Subject,
                    Subject2 = SD.Subject2,
                    OrderId = SD.OrderId,
                    ShippingDate = SD.ShippingDate,
                    PickUpPoints = SD.PickUpPoints,
                    LoginUser = SD.LoginUser,
                    RequestorEmail = SD.RequestorEmail,
                    RejectReason = SD.RejectReason,
                    clickhere = SD.clickhere,
                    items = SD.items

                };

                var emailSendJSON = JsonConvert.SerializeObject(emailSend);   // convert string array to JSON string
                var buffer = System.Text.Encoding.UTF8.GetBytes(emailSendJSON);    // convert string array to byte
                var byteContent = new ByteArrayContent(buffer); // create new instance of byte array context
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // sets a header to 'application/json'

                client.BaseAddress = new Uri(SD.ApiUri);    // create new instance of Uri and set the HttpClient
                var response = await client.PostAsync(SD.ApiUri, byteContent);  // send a post request to the specified Uri

                if (response.IsSuccessStatusCode)   // condition for response status
                {
                    return RedirectToAction(nameof(Index), Json(response)); // if 'success' the return response to HTTP Request, redirect to success page
                }
                else
                {
                    return RedirectToAction(nameof(Failed), Json(response)); // if 'failed' the return response to HTTP Request, redirect to failed page
                }
            }
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
            using (var context = new PrincipalContext(ContextType.Domain, SD.OfficeDomain))
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
    }
}