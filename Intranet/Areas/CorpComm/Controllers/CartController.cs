using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Models.ViewModels.CorpComm;
using Intranet.Uti;
using Intranet.Utilities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using System;
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
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Failed()
        {
            return View();
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
            UserDetails();
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Collateral");
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            UserDetails();
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
            UserDetails();
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

        [Obsolete]
        public IActionResult OrderConfirmation(int id)
        {
            UserDetails();
            #region New Request Notification

            // get the email template
            var PathToFile = _hostEnvironment.WebRootPath +
                Path.DirectorySeparatorChar.ToString() + "Templates" +
                Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString() +
                "Email_Notifications_NewRequest.html";

            var AdminEmail = SD.RenzEmail;

            #endregion New Request Notification

            string hereurl;

            SD.Template = null;
            SD.Subject = null;
            SD.Subject2 = null;
            SD.OrderId = null;
            SD.PickUpPoints = null;
            SD.LoginUser = null;
            SD.RequestorEmail = null;
            SD.RejectReason = null;
            SD.clickhere = null;
            SD.items = null;

            SD.Template = "1";
            SD.Subject = "PTT COLLATERALS: New Collateral Request!";
            SD.Subject2 = "COLLATERAL REQUEST";
            SD.OrderId = "";
            SD.ShippingDate = "";
            SD.PickUpPoints = "";
            SD.LoginUser = ViewBag.DisplayName;
            SD.RequestorEmail = SD.RenzEmail;
            SD.RejectReason = "";
            hereurl = SD.IntranetLink + "CorpComm/Order/Details/" + id;
            SD.clickhere = "There's a new collateral request. Click <a href='" + hereurl + "'>here</a> to view the request.";
            SD.items = "";

            SD.NewOrderId = id;
            SendEmail();
            return View(id);
            //return RedirectToAction(nameof(SendEmail));
        }

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
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.LoginUser == UserCart, includeProperties: "Collateral").ToList().Count();
                HttpContext.Session.SetObject(SD.ssShoppingCart, count);
                ViewBag.ItemCount = count;
            }
        }

        #endregion UserDetails function

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
                    return Json(response);
                    //return Redirect(SD.IntranetLink + "CorpComm/Cart/OrderConfirmation/" + SD.NewOrderId); // if 'success' the return response to HTTP Request, redirect to success page
                    //return RedirectToAction("OrderConfirmation", new { id = SD.NewOrderId }); // if 'success' the return response to HTTP Request, redirect to success page
                }
                else
                {
                    return RedirectToAction(nameof(Failed), Json(response)); // if 'failed' the return response to HTTP Request, redirect to failed page
                }
            }
        }
    }
}