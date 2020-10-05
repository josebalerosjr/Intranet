using Intranet.Classes;
using Intranet.Data;
using Intranet.Models;
using Intranet.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NToastNotify;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Controllers
{
    public class ImageCarouselController : Controller
    {
        private readonly ImageCarouselContext _context;
        private readonly EmailOptions _emailOptions;
        private readonly IToastNotification _toastNotification;

        public ImageCarouselController(ImageCarouselContext context, IOptions<EmailOptions> emailOptions, IToastNotification toastNotification)
        {
            _context = context;
            _emailOptions = emailOptions.Value;
            _toastNotification = toastNotification;
        }

        // GET: ImageCarousel
        public async Task<IActionResult> Index()
        {
            UserDetails();
            return View(await _context.imageCarousels.ToListAsync());
        }

        // GET: ImageCarousel/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            UserDetails();
            if (id == 0)
                return View(new ImageCarousel());
            else
                return View(_context.imageCarousels.Find(id));
        }

        // POST: ImageCarousel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,ImageName,ImageLink,UserName,UserIP,UserDate,isActive")] ImageCarousel imageCarousel)
        {
            UserDetails();

            if (ModelState.IsValid)
            {
                if (imageCarousel.Id == 0)
                {
                    _context.Add(imageCarousel);
                    addToast(); // TODO: Triggers addToast
                }
                else
                {
                    _context.Update(imageCarousel);
                    editToast(); // TODO: Triggers editToast
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                warnToast();    // TODO: Triggers warnToast
            }
            return View(imageCarousel);
        }

        // POST: ImageCarousel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageName,ImageLink,UserName,UserIP,UserDate")] ImageCarousel imageCarousel)
        {
            UserDetails();
            if (id != imageCarousel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageCarousel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageCarouselExists(imageCarousel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(imageCarousel);
        }

        // GET: Email/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetails(); // TODO: triggers UserDetails function
            var email = await _context.imageCarousels.FindAsync(id);
            _context.imageCarousels.Remove(email);
            delToast(); // TODO: triggers delToasters
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageCarouselExists(int id)
        {
            return _context.imageCarousels.Any(e => e.Id == id);
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