using Intranet.Data.QSHE;
using Intranet.Uti;
using Intranet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Intranet.Controllers
{
    [Authorize(Roles = SD.CIOAdmin + "," + SD.QSHEQtyAdmin)]
    public class ReportController : Controller
    {
        private readonly ItemRegContext _context;
        private readonly EmailOptions _emailOptions;
        //private readonly IHostingEnvironment _hostingEnvironment;

        public ReportController(ItemRegContext context, IOptions<EmailOptions> EmailOptions/*, IHostingEnvironment hostingEnvironment*/)
        {
            _context = context;
            _emailOptions = EmailOptions.Value;
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
            var domain = _emailOptions.AuthDomain;
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