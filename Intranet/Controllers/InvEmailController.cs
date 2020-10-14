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
    public class InvEmailController : Controller
    {
        private readonly InvEmailContext _context;
        private readonly IToastNotification _toastNotification;

        public InvEmailController(
            InvEmailContext context,
            IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        // GET: InvEmail
        [Authorize(Roles = "Office of the Chief Information Officer,QSHE QtyAdmin,SSHE Admin")]
        public async Task<IActionResult> Index()
        {
            UserDetails();
            return View(await _context.invEmails.ToListAsync());
        }

        // GET: InvEmail/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            UserDetails();

            if (id == 0)
                return View(new InvEmail());
            else
                return View(_context.invEmails.Find(id));
        }

        // POST: InvEmail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("InvEmailId,InvEmailAddress,UserName,UserIP,UserDate")] InvEmail invEmail)
        {
            UserDetails();

            if (ModelState.IsValid)
            {
                if (!InvEmailExists(invEmail.InvEmailAddress))
                {
                    if (invEmail.InvEmailId == 0)
                    {
                        _context.Add(invEmail);
                        addToast(); // Triggers addToast
                    }
                    else
                    {
                        _context.Update(invEmail);
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
            return View(invEmail);
        }

        // GET: InvEmail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetails(); // triggers UserDetails function
            var email = await _context.invEmails.FindAsync(id);
            _context.invEmails.Remove(email);
            delToast(); // triggers delToasters
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvEmailExists(string emails)
        {
            return _context.invEmails.Any(e => e.InvEmailAddress == emails);
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

        #region Toastr functions

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

        #endregion Toastr functions
    }
}