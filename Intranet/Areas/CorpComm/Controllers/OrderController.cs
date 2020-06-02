using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Intranet.Classes;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Models.ViewModels.CorpComm;
using Intranet.Utilities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[action]/{id?}")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        private readonly EmailSender _emailSender;
        public readonly EmailOptions _emailOptions;

        [BindProperty]
        public OrderDetailsVM OrderVM { get; set; }  

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IOptions<EmailOptions> emailOptions)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
            _emailOptions = emailOptions.Value;
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

        [Authorize(Roles = SD.CIOAdmin)]
        public IActionResult ReceiveRequestAndForApproval(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusForApproval;

            string mailFrom = orderHeader.TrackingNumber;

                var message = new MimeMessage();
                var builder = new BodyBuilder();
                message.From.Add(new MailboxAddress(_emailOptions.AuthEmail));
                message.To.Add(new MailboxAddress(mailFrom));
                message.Subject = "Collateral Request Approval";
                builder.HtmlBody =  "<p>Request No." + orderHeader.Id.ToString() + " Has been approved!</p> <br />" +
                                    "You can check the order details and status, <a href='http://localhost:44301/CorpComm/Order/Details/" + orderHeader.Id + "'>Click here.</a>";
                message.Body = builder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.Connect(_emailOptions.SMTPHostClient, _emailOptions.SMTPHostPort);
                    //client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(_emailOptions.AuthEmail, _emailOptions.AuthPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = SD.CIOAdmin)]
        public IActionResult ReceiveRequestAndReject(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusRejected;

            string mailFrom = orderHeader.TrackingNumber;

            var message = new MimeMessage();
            var builder = new BodyBuilder();
            message.From.Add(new MailboxAddress(_emailOptions.AuthEmail));
            message.To.Add(new MailboxAddress(mailFrom));
            message.Subject = "Collateral Request Rejected";
            builder.HtmlBody = "<p>Request No." + orderHeader.Id.ToString() + " Has been Rejected!</p>" +
                                "<p> You can check the order details and status, <a href='http://localhost:44301/CorpComm/Order/Details/" + orderHeader.Id + "'>Click here.</a> </p>";
            message.Body = builder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect(_emailOptions.SMTPHostClient, _emailOptions.SMTPHostPort, _emailOptions.SMTPHostBool);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_emailOptions.AuthEmail, _emailOptions.AuthPassword);
                client.Send(message);
                client.Disconnect(true);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = SD.CIOAdmin)]
        public IActionResult ProcessOrderAndForDelivery(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusForDelivery;
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

        #region SendMail
        
        #endregion
    }
}