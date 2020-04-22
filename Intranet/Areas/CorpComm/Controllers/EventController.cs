using Intranet.Classes;
using Intranet.DataAccess.Repository.CorpComm.IRepository;
using Intranet.Models.CorpComm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[Action]/{id?}")]
    public class EventController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public EventController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            UserDetails();
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            UserDetails();
            Event @event = new Event();

            if (id == null)
            {
                // for create
                return View(@event);
            }

            @event = _unitOfWork.Event.Get(id.GetValueOrDefault());
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Event @event)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (@event.Id == 0)
                {
                    _unitOfWork.Event.Add(@event);
                }
                else
                {
                    _unitOfWork.Event.Update(@event);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Event.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Event.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Event.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion API CALLS

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
    }
}