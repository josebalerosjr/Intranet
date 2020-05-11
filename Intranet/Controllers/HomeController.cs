using Intranet.Classes;
using Intranet.Data;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models;
using Intranet.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Intranet.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly AppLinks _appLinks;
        private readonly OnlineImageLinks _onlineImageLinks;
        private readonly ImageCarouselContext _imagecarouselContext;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ImageCarouselContext imagecarouselContext, IOptions<AppSettings> appSettings, IOptions<AppLinks> appLinks, IOptions<OnlineImageLinks> onlineImageLinks, IUnitOfWork unitOfWork)
        {
            _imagecarouselContext = imagecarouselContext;
            _appSettings = appSettings.Value;
            _appLinks = appLinks.Value;
            _onlineImageLinks = onlineImageLinks.Value;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            UserDetails(); // TODO: calls UserDetails method
            AppLinks();
            OnlineImageLink();
            string GetUser = ViewBag.DisplayName;
            int count = _unitOfWork.ShoppingCart.GetAll(u => u.LoginUser == GetUser).Count();
            HttpContext.Session.SetObject(SD.ssShoppingCart, count);
            ViewBag.ItemCount = count;

            return View(_imagecarouselContext.imageCarousels.Where(c => c.isActive == true).ToList());
        }

        public IActionResult Policy()
        {
            AppLinks();
            return View();
        }

        public IActionResult Privacy()
        {
            AppLinks();
            return View();
        }

        public IActionResult CollateralRequest()
        {
            AppLinks();
            return View();
        }

        #region userDetails Function

        public void UserDetails() // TODO: Gets the user information
        {
            var domain = _appSettings.appDomain;
            var username = User.Identity.Name;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
            }

            string SPEMid = "";
            var DevSPEM = Startup.StaticConfig.GetConnectionString("DevSPEM");
            using (var sql = new SqlConnection(DevSPEM))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = "SELECT * " +
                                    "FROM [SPEM].[dbo].[tbl_users] " +
                                    "WHERE [tbl_users].[user_name] = @username",
                    CommandType = CommandType.Text,
                    Connection = sql
                })
                {
                    sql.Open();
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = ViewBag.DisplayName;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            SPEMid = reader[0].ToString();
                        }
                    }
                }
            }

            //ViewBag.SPEM = "http://192.168.10.42:9000/?id=" + SPEMid;
            ViewBag.SPEM = _appSettings.SPEMLink + SPEMid;
        }

        public void AppLinks()
        {
            ViewBag.ESS = _appLinks.ESS;
            ViewBag.CollateralRequest = _appLinks.CollateralRequest;
            ViewBag.JobOrder = _appLinks.JobOrder;
            ViewBag.VendorEvaluation = _appLinks.VendorEvaluation;
            ViewBag.DocumentMonitoring = _appLinks.DocumentMonitoring;
            ViewBag.ChangeRequestLog = _appLinks.ChangeRequestLog;
            ViewBag.Office365 = _appLinks.Office365;
            ViewBag.DxCare = _appLinks.DxCare;
            ViewBag.ManualDeliveryRequest = _appLinks.ManualDeliveryRequest;
            ViewBag.MISTicketing = _appLinks.MISTicketing;
            ViewBag.ICTTicketing = _appLinks.ICTTicketing;
            ViewBag.PhoneRefresh = _appLinks.PhoneRefresh;
            ViewBag.GoPro = _appLinks.GoPro;
            ViewBag.CustomerInquiry = _appLinks.CustomerInquiry;
            ViewBag.CustomerComplaints = _appLinks.CustomerComplaints;
            ViewBag.UserTicketing = _appLinks.UserTicketing;
        }

        public void OnlineImageLink()
        {
            ViewBag.vision = _onlineImageLinks.vision;
            ViewBag.goal = _onlineImageLinks.goal;
            ViewBag.mission = _onlineImageLinks.mission;
        }

        #endregion userDetails Function

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}