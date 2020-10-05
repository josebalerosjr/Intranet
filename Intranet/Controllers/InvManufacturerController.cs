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
    public class InvManufacturerController : Controller
    {
        private readonly InvManufacturerContext _context;
        private readonly EmailOptions _emailOptions;
        private readonly IToastNotification _toastNotification;

        public InvManufacturerController(
            InvManufacturerContext context,
            IOptions<EmailOptions> emailOptions,
            IToastNotification toastNotification)
        {
            _context = context;
            _emailOptions = emailOptions.Value;
            _toastNotification = toastNotification;
        }

        // GET: InvManufacturer
        [Authorize(Roles = "Office of the Chief Information Officer,QSHE QtyAdmin,SSHE Admin")]
        public async Task<IActionResult> Index()
        {
            UserDetails();
            return View(await _context.invManufacturers.ToListAsync());
        }

        // GET: InvManufacturer/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            UserDetails();
            if (id == 0)
                return View(new InvManufacturer());
            else
                return View(_context.invManufacturers.Find(id));
        }

        // POST: InvManufacturer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("MakerId,MakerName,UserName,UserIP,UserDate")] InvManufacturer invManufacturer)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (!InvManufacturerExists(invManufacturer.MakerName))
                {
                    if (invManufacturer.MakerId == 0)
                    {
                        _context.Add(invManufacturer);
                        addToast(); // triggers addToast
                    }
                    else
                    {
                        _context.Update(invManufacturer);
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
            return View(invManufacturer);
        }

        // GET: InvManufacturer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetails();
            var manufacturer = await _context.invManufacturers.FindAsync(id);
            _context.invManufacturers.Remove(manufacturer);
            delToast(); // triggers delToast
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvManufacturerExists(string manuf)
        {
            return _context.invManufacturers.Any(e => e.MakerName == manuf);
        }

        #region UserDetails function

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