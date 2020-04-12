using Intranet.Classes;
using Intranet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Intranet.Controllers
{
    [Authorize(Roles = "Office of the Chief Information Officer, QSHE QtyAdmin")]
    public class ReportController : Controller
    {
        private readonly ItemRegContext _context;
        private readonly AppSettings _appSettings;
        //private readonly IHostingEnvironment _hostingEnvironment;

        public ReportController(ItemRegContext context, IOptions<AppSettings> appSettings/*, IHostingEnvironment hostingEnvironment*/)
        {
            _context = context;
            _appSettings = appSettings.Value;
            //_hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(string searchColumn)
        {
            UserDetails();
            var items = from item in _context.ItemRegs select item;

            #region search function

            switch (searchColumn)
            {
                case "Consumable":
                    if (!string.IsNullOrEmpty(searchColumn))
                        items = items.Where(item => item.TypeName.Contains(searchColumn));
                    break;

                case "Base Asset":
                    items = items.Where(item => item.TypeName.Contains(searchColumn));
                    break;
            }

            #endregion search function

            return View(items);
        }

        #region UserDetails Function

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

        #endregion UserDetails Function
    }
}