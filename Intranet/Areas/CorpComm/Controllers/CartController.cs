using Intranet.Classes;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Models.ViewModels.CorpComm;
using Intranet.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[action]/{id?}")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            UserDetails();
            CartCount();
            string UserCart = ViewBag.DisplayName;
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.LoginUser == UserCart, includeProperties: "Collateral,Event"),
                Event = new Event(),
                EventList = _unitOfWork.Event.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.BudgetLimit.ToString()
                })
            };

            ShoppingCartVM.OrderHeader.OrderTotal = 0;

            foreach (var list in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Count * list.Collateral.Price);
                list.Collateral.Description = SD.ConvertToRawHtml(list.Collateral.Description);
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Collateral");
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Collateral");

            if (cart.Count == 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
                CartCount();
            }
            else
            {
                cart.Count -= 1;
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Collateral");
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            CartCount();
            return RedirectToAction(nameof(Index)); ;
        }

        public IActionResult Summary()
        {
            UserDetails();
            CartCount();
            string loginUser = ViewBag.DisplayName;
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(c => c.LoginUser == loginUser, includeProperties: "Collateral,Event")
            };
            ShoppingCartVM.OrderHeader.LoginUser = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.LoginUser == loginUser, includeProperties: "Event").ToString();

            foreach (var list in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Count * list.Collateral.Price);
                list.Collateral.Description = SD.ConvertToRawHtml(list.Collateral.Description);
            }
            ShoppingCartVM.OrderHeader.LoginUser = loginUser;
            return View(ShoppingCartVM);
        }

        public IActionResult PendingOrder()
        {
            UserDetails();
            CartCount();
            return View();
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(ShoppingCartVM cartVM)
        {
            UserDetails();
            CartCount();
            string loginUser = ViewBag.DisplayName;

            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(c => c.LoginUser == loginUser, includeProperties: "Collateral,Event"),
                ListOrderHeader = _unitOfWork.OrderHeader.GetAll(c => c.LoginUser == loginUser &&
                                    (c.OrderStatus.Contains(SD.StatusForAcknowledgement) || c.OrderStatus.Contains(SD.StatusForRating)))
            };

            var pendingCount = 0;

            foreach (var stat in ShoppingCartVM.ListOrderHeader)
            {
                pendingCount = ShoppingCartVM.ListOrderHeader.Count();
            }

            if (pendingCount != 0)
            {
                return RedirectToAction(nameof(PendingOrder));
            }
            else
            {
                ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(c => c.LoginUser == loginUser);

                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusForApproval;
                ShoppingCartVM.OrderHeader.LoginUser = ViewBag.DisplayName;
                ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

                string UserEmail = User.Identity.Name.Substring(7);
                ShoppingCartVM.OrderHeader.RequestorEmail = UserEmail + "@pttphils.com";

                _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
                _unitOfWork.Save();

                foreach (var item in ShoppingCartVM.ListCart)
                {
                    OrderDetails orderDetails = new OrderDetails()
                    {
                        CollateralId = item.CollateralId,
                        OrderId = ShoppingCartVM.OrderHeader.Id,
                        Price = item.Collateral.Price,
                        Count = item.Count
                    };

                    ShoppingCartVM.OrderHeader.OrderTotal += orderDetails.Count * orderDetails.Price;
                    _unitOfWork.OrderDetails.Add(orderDetails);
                }

                #region Add EventName, StationEvent, EventDate

                OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == ShoppingCartVM.OrderHeader.Id);
                orderHeader.EventName = cartVM.SelectedEvent;
                orderHeader.StationEvent = cartVM.OrderHeader.StationEvent;
                orderHeader.EventDate = cartVM.OrderHeader.EventDate;
                orderHeader.EventName = cartVM.OrderHeader.EventName;

                #endregion Add EventName, StationEvent, EventDate

                _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, 0);

                return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
            }
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

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
    }
}