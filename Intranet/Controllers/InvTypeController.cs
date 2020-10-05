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
    public class InvTypeController : Controller
    {
        private readonly InvTypeContext _context;
        private readonly EmailOptions _emailOptions;
        private readonly IToastNotification _toastNotification;

        public InvTypeController(InvTypeContext context, IOptions<EmailOptions> emailOptions, IToastNotification toastNotification)
        {
            _context = context;
            _emailOptions = emailOptions.Value;
            _toastNotification = toastNotification;
        }

        // GET: InvType
        [Authorize(Roles = "Office of the Chief Information Officer,QSHE QtyAdmin,SSHE Admin")]
        public async Task<IActionResult> Index()
        {
            UserDetails();
            return View(await _context.invTypes.ToListAsync());
        }

        // GET: InvType/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            UserDetails();
            if (id == 0)
                return View(new InvType());
            else
                return View(_context.invTypes.Find(id));
        }

        // POST: InvType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TypeId,TypeName,UserName,UserIP,UserDate")] InvType invType)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (!InvTypeExists(invType.TypeName))
                {
                    string typename = invType.TypeName;
                    if (invType.TypeId == 0)
                    {
                        _context.Add(invType);
                        addToast(); // triggers addToast
                    }
                    else
                    {
                        _context.Update(invType);
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
            return View(invType);
        }

        // GET: InvType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetails();
            var invtype = await _context.invTypes.FindAsync(id);
            _context.invTypes.Remove(invtype);
            delToast(); // triggers delToast
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvTypeExists(string typename)
        {
            return _context.invTypes.Any(e => e.TypeName == typename);
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