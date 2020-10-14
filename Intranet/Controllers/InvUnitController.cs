using Intranet.Data.QSHE;
using Intranet.Models.QSHE;
using Intranet.Uti;
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
    public class InvUnitController : Controller
    {
        private readonly InvUnitContext _context;
        private readonly IToastNotification _toastNotification;

        public InvUnitController(InvUnitContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        // GET: InvUnit
        [Authorize(Roles = "Office of the Chief Information Officer,QSHE QtyAdmin,SSHE Admin")]
        public async Task<IActionResult> Index()
        {
            UserDetails();
            return View(await _context.invUnits.ToListAsync());
        }

        // GET: InvUnit/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            UserDetails();
            if (id == 0)
                return View(new InvUnit());
            else
                return View(_context.invUnits.Find(id));
        }

        // POST: InvUnit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("UnitId,UnitName,UserName,UserIP,UserDate")] InvUnit invUnit)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (!InvUnitExists(invUnit.UnitName))
                {
                    if (invUnit.UnitId == 0)
                    {
                        _context.Add(invUnit);
                        addToast(); // triggers addToast
                    }
                    else
                    {
                        _context.Update(invUnit);
                        editToast(); // triggers editToast
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    warnToast(); // triggers warnToast
                }
            }
            return View(invUnit);
        }

        // GET: InvUnit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetails();
            var invunit = await _context.invUnits.FindAsync(id);
            _context.invUnits.Remove(invunit);
            delToast(); // triggers delToastr
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvUnitExists(string unitname)
        {
            return _context.invUnits.Any(e => e.UnitName == unitname);
        }

        #region UserDetails function

        public void UserDetails()
        {
            var username = User.Identity.Name;
            var domain = SD.OfficeDomain;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
            }
        }

        #endregion UserDetails function

        #region Toastr function

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

        #endregion Toastr function
    }
}