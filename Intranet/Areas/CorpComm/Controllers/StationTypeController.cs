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
    public class StationTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailOptions _appSettings;

        public StationTypeController(IUnitOfWork unitOfWork, IOptions<EmailOptions> emailOptions)
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
            StationType stationType = new StationType();

            if (id == null)
            {
                // for create
                return View(stationType);
            }

            stationType = _unitOfWork.StationType.Get(id.GetValueOrDefault());
            if (stationType == null)
            {
                return NotFound();
            }
            return View(stationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(StationType stationType)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (stationType.Id == 0)
                {
                    _unitOfWork.StationType.Add(stationType);
                }
                else
                {
                    _unitOfWork.StationType.Update(stationType);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(stationType);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.StationType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.StationType.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.StationType.Remove(objFromDb);
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