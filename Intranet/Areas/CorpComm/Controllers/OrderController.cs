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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.CIOAdmin)]
        public IActionResult ReceiveRequestAndForApproval()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = SD.StatusForApproval;
            orderHeader.RequestType = OrderVM.OrderHeader.RequestType;

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = SD.CIOAdmin)]
        public IActionResult ReceiveRequestAndReject(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusRejected;

            // email process here

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.CIOAdmin)]
        public IActionResult ProcessOrderAndForDelivery()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = SD.StatusForDelivery;

            orderHeader.ShippingDate = OrderVM.OrderHeader.ShippingDate;
            orderHeader.PickUpPoints = OrderVM.OrderHeader.PickUpPoints;

            #region EmailTemplate
            var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString() + "Email_Notifications.html";

            var subject = "For delivery";
            var datetime = String.Format(DateTime.Now.ToShortDateString());
            var OrderId = OrderVM.OrderHeader.Id;
            var ShippingDate = OrderVM.OrderHeader.ShippingDate.ToShortDateString();
            var PickUpPoints = OrderVM.OrderHeader.PickUpPoints;
            var LoginUser = OrderVM.OrderHeader.LoginUser;
            var RequestorEmail = OrderVM.OrderHeader.RequestorEmail;

            string HtmlBody = "";

            using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
            {
                HtmlBody = streamReader.ReadToEnd();
            }

            // {0} : for Delivery
            // {1} : DateTime
            // {2} : Request Number
            // {3} : Shipping Date
            // {4} : Drop-off Location
            // {5} : Requestor Name
            // {6} : Email

            string messageBody = string.Format(HtmlBody,
                subject,
                datetime,
                OrderId,
                ShippingDate,
                PickUpPoints,
                LoginUser,
                RequestorEmail
                );
            #endregion

            EmailSender(
                RequestorEmail,
                subject,
                messageBody
                );

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ReceiveItemAndForAcknowledgement(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusForAcknowledgement;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AcknowledgeAndConfirmReceipt(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusForRating;
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

            if (User.IsInRole(SD.CIOAdmin))
            {
                orderHeadersList = _unitOfWork.OrderHeader.GetAll();
            }
            else
            {
                orderHeadersList = _unitOfWork.OrderHeader.GetAll(u => u.LoginUser == loginUser);
            }

            switch (status)
            {
                case "requestsent":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusRequestSent);
                    break;

                case "forapproval":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusForApproval);
                    break;

                case "fordelivery":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusForDelivery);
                    break;

                case "foracknowledge":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusForAcknowledgement);
                    break;

                case "forrating":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusForRating);
                    break;

                case "rejected":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusRejected);
                    break;

                default:
                    break;
            }

            //orderHeadersList = _unitOfWork.OrderHeader.GetAll();
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
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.LoginUser == UserCart, includeProperties: "Collateral").ToList().Count();
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