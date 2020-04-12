using Intranet.Classes;
using Intranet.Models;
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
    [Authorize(Roles = "Office of the Chief Information Officer")] // TODO: this line authorize the role for accessing Email Controller
    public class EmailController : Controller
    {
        private readonly EmailContext _context;
        private readonly AppSettings _appSettings;
        private readonly IToastNotification _toastNotification;

        public EmailController(EmailContext context, IOptions<AppSettings> appSettings, IToastNotification toastNotification)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _toastNotification = toastNotification;
        }

        // GET: Email
        public async Task<IActionResult> Index()
        {
            UserDetails();  // TODO: calls UserDetails method
            return View(await _context.Emails.ToListAsync());
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            UserDetails();  // TODO: calls UserDetails method

            if (id == 0)
                return View(new Email());
            else
                return View(_context.Emails.Find(id));
        }

        // POST: Email/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("EmailId,EmailAddress,UserName,UserIP,UserDate")] Email email)
        {
            UserDetails(); // TODO: Triggers userDetails

            if (ModelState.IsValid)
            {
                if (!EmailExists(email.EmailAddress))   // TODO: checks if Email is already exist
                {
                    if (email.EmailId == 0)
                    {
                        _context.Add(email);
                        addToast(); // TODO: Triggers addToast
                    }
                    else
                    {
                        _context.Update(email);
                        editToast(); // TODO: Triggers editToast
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    warnToast();    // TODO: Triggers warnToast
                }
            }
            return View(email);
        }

        // GET: Email/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetails(); // TODO: triggers UserDetails function
            var email = await _context.Emails.FindAsync(id);
            _context.Emails.Remove(email);
            delToast(); // TODO: triggers delToasters
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailExists(string address)
        {
            return _context.Emails.Any(e => e.EmailAddress == address);
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

        #endregion UserDetails function

        /**
        * ********************************************************************************
        * *                        START OF TOASTR                                       *
        * ********************************************************************************
        */

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

        /**
        * ********************************************************************************
        * *                        END OF TOASTR                                         *
        * ********************************************************************************
        */
    }
}