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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[action]/{id?}")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        [BindProperty]
        public OrderDetailsVM OrderVM { get; set; }  

        public OrderController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
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

        //[HttpPost]
        //[Authorize(Roles = SD.CIOAdmin)]
        //public IActionResult ShipOrder(int id)
        //{
        //    OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
        //    orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
        //    orderHeader.ShippingDate = DateTime.Now;
        //    orderHeader.OrderStatus = SD.StatusRequestSent;

        //    _unitOfWork.Save();
        //    return RedirectToAction(nameof(Index));
        //}

        //[Authorize(Roles = SD.CIOAdmin)]
        //public IActionResult CancelOrder(int id)
        //{
        //    OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
        //    orderHeader.OrderStatus = SD.StatusRequestSent;

        //    _unitOfWork.Save();
        //    return RedirectToAction(nameof(Index));
        //}

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

        #endregion UserDetails function
    }
}