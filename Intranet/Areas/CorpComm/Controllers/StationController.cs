using Intranet.Classes;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Models.ViewModels.CorpComm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[action]/{id?}")]
    public class StationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public StationController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
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
            StationVM stationVM = new StationVM()
            {
                Station = new Station(),
                StationTypeList = _unitOfWork.StationType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                // for create
                return View(stationVM);
            }

            stationVM.Station = _unitOfWork.Station.Get(id.GetValueOrDefault());
            if (stationVM.Station == null)
            {
                return NotFound();
            }
            return View(stationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(StationVM stationVM)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (stationVM.Station.Id == 0)
                {
                    _unitOfWork.Station.Add(stationVM.Station);
                }
                else
                {
                    _unitOfWork.Station.Update(stationVM.Station);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(stationVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Station.GetAll(includeProperties: "StationType");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Station.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Station.Remove(objFromDb);
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