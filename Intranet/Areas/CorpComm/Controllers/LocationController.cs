using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Uti;
using Intranet.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[Action]/{id?}")]
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailOptions _appSettings;

        public LocationController(IUnitOfWork unitOfWork, IOptions<EmailOptions> emailOptions)
        {
            _unitOfWork = unitOfWork;
            _appSettings = emailOptions.Value;
        }

        public IActionResult Index()
        {
            UserDetails();
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            UserDetails();
            Location location = new Location();

            if (id == null)
            {
                // for create
                return View(location);
            }

            location = _unitOfWork.Location.Get(id.GetValueOrDefault());
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Location location)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (location.Id == 0)
                {
                    _unitOfWork.Location.Add(location);
                }
                else
                {
                    _unitOfWork.Location.Update(location);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Location.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Location.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Location.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion API CALLS

        #region UserDetails function

        public void UserDetails()
        {
            var username = User.Identity.Name;
            var domain = _appSettings.AuthDomain;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
            }
        }

        #endregion UserDetails function
    }
}