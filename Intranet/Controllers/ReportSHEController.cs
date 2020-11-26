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
    [Authorize(Roles = SD.CIOAdmin + "," + SD.QSHESHEAdmin)]
    public class ReportSHEController : Controller
    {
        private readonly ItemRegSHEContext _context;
        //private readonly IHostingEnvironment _hostingEnvironment;

        public ReportSHEController(ItemRegSHEContext context /*, IHostingEnvironment hostingEnvironment*/)
        {
            _context = context;
            //_hostingEnvironment = hostingEnvironment;
        }

        // GET: ReportSHE
        public IActionResult Index(string searchColumn)
        {
            UserDetails();
            var items = from item in _context.ItemRegSHEs select item;

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
            using (var context = new PrincipalContext(ContextType.Domain, SD.OfficeDomain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
            }
        }

        #endregion UserDetails Function
    }
}