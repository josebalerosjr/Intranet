﻿using Intranet.Classes;
using Intranet.DataAccess.Repository.CorpComm.IRepository;
using Intranet.Models.CorpComm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[Action]/{id?}")]
    public class StatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public StatusController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
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
            Status status = new Status();

            if (id == null)
            {
                // for create
                return View(status);
            }

            status = _unitOfWork.Status.Get(id.GetValueOrDefault());
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Status status)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (status.Id == 0)
                {
                    _unitOfWork.Status.Add(status);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.Status.Update(status);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(status);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Status.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Status.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Status.Remove(objFromDb);
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