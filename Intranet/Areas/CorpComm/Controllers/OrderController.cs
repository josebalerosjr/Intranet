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
        public IActionResult StartProcessing(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusInProcess;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = SD.CIOAdmin)]
        public IActionResult ShipOrder(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.ShippingDate = DateTime.Now;
            orderHeader.OrderStatus = SD.StatusShipped;

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = SD.CIOAdmin)]
        public IActionResult CancelOrder(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusInProcess;
            
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
                case "pending":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusPending);
                    break;
                case "inprocess":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted);
                    break;
                case "rejected":
                    orderHeadersList = orderHeadersList.Where(
                        o => o.OrderStatus == SD.StatusReject || o.OrderStatus == SD.StatusCancelled);
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