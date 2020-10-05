using Intranet.Classes;
using Intranet.Data;
using Intranet.Models;
using Intranet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NToastNotify;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Controllers
{
    public class InvLocationController : Controller
    {
        private readonly InvLocationContext _context;
        private readonly EmailOptions _emailOptions;
        private readonly IToastNotification _toastNotification;

        public InvLocationController(
            InvLocationContext context,
            IOptions<EmailOptions> emailOptions,
            IToastNotification toastNotification)
        {
            _context = context;
            _emailOptions = emailOptions.Value;
            _toastNotification = toastNotification;
        }

        // GET: InvLocation
        [Authorize(Roles = "Office of the Chief Information Officer,QSHE QtyAdmin,SSHE Admin")]
        public async Task<IActionResult> Index()
        {
            UserDetails();
            return View(await _context.invLocations.ToListAsync());
        }

        // GET: InvLocation/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            UserDetails();
            if (id == 0)
                return View(new InvLocation());
            else
                return View(_context.invLocations.Find(id));
        }

        // POST: InvLocation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("LocId,LocName,UserName,UserIP,UserDate")] InvLocation invLocation)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (!InvLocationExists(invLocation.LocName))
                {
                    string locname = invLocation.LocName;

                    if (invLocation.LocId == 0)
                    {
                        _context.Add(invLocation);
                        addToast(); // Triggers addToast
                    }
                    else
                    {
                        _context.Update(invLocation);
                        editToast(); // Triggers editToast
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    warnToast(); // Triggers warnToast
                }
            }
            return View(invLocation);
        }

        // GET: InvLocation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetails();
            var locname = await _context.invLocations.FindAsync(id);
            _context.invLocations.Remove(locname);
            delToast(); // Triggers delToast
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvLocationExists(string locname)
        {
            return _context.invLocations.Any(e => e.LocName == locname);
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

        #region Toastr Functions

        public void delToast()
        {
            _toastNotification.AddErrorToastMessage("The record has been deleted successfully.", new ToastrOptions()
            {
                Title = ""
            });
        }

        public void addToast()
        {
            _toastNotification.AddSuccessToastMessage("Successfully added record!", new ToastrOptions()
            {
                Title = ""
            });
        }

        public void editToast()
        {
            _toastNotification.AddInfoToastMessage("The record has been updated successfully.", new ToastrOptions()
            {
                Title = ""
            });
        }

        public void warnToast()
        {
            _toastNotification.AddWarningToastMessage("The record already exists", new ToastrOptions()
            {
                Title = ""
            });
        }

        #endregion Toastr Functions
    }
}