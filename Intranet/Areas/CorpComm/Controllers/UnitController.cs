﻿using Intranet.DataAccess.Repository.IRepository;
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
    public class UnitController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            UserDetails();
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            UserDetails();
            Unit unit = new Unit();

            if (id == null)
            {
                // for create
                return View(unit);
            }

            unit = _unitOfWork.Unit.Get(id.GetValueOrDefault());
            if (unit == null)
            {
                return NotFound();
            }
            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Unit unit)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (unit.Id == 0)
                {
                    _unitOfWork.Unit.Add(unit);
                }
                else
                {
                    _unitOfWork.Unit.Update(unit);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(unit);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Unit.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Unit.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Unit.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion API CALLS

        #region UserDetails function

        public void UserDetails()
        {
            var username = User.Identity.Name;
            using (var context = new PrincipalContext(ContextType.Domain, SD.OfficeDomain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
            }
        }

        #endregion UserDetails function
    }
}