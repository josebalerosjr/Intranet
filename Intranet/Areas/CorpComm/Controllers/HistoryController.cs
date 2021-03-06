﻿using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Uti;
using Intranet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[Action]/{id?}")]
    [Authorize(Roles = SD.CIOAdmin + ", " + SD.CorpCommAdmin)]
    public class HistoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CorpCommDbContext _context;

        public HistoryController(IUnitOfWork unitOfWork,
            CorpCommDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IActionResult Index()
        {
            UserDetails();
            CartCount();

            return View();
        }

        public IActionResult ItemAdjustment(int qty)
        {
            UserDetails();
            CartCount();

            var ColDetails = _unitOfWork.Collateral
                .GetFirstOrDefault(u => u.Id == SD.historyid);

            SD.histLoginName = ViewBag.DisplayName;
            SD.histColName = ColDetails.Name;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ItemAdjustment(History history)
        {
            UserDetails();
            CartCount();

            var action = Request.Form["actionz"];

            if (action == "Plus")
            {
                History hist = new History();
                if (SD.historyid != 0)
                {
                    var collateral = _unitOfWork.Collateral
                        .GetFirstOrDefault(u => u.Id == SD.historyid);

                    hist.LoginUser = ViewBag.DisplayName;
                    hist.CollateralId = collateral.Id;
                    hist.CollateralName = collateral.Name;
                    hist.Quantity = history.Quantity;
                    hist.RequestDate = history.RequestDate;
                    hist.ReconRemarks = history.ReconRemarks;
                    _unitOfWork.History.Add(hist);

                    Collateral minuscollateral = _unitOfWork
                        .Collateral.Get(SD.historyid);
                    collateral.Count += history.Quantity;

                    _unitOfWork.Save();
                }
            }

            if (action == "Minus")
            {
                History hist = new History();
                if (SD.historyid != 0)
                {
                    var collateral = _unitOfWork.Collateral
                        .GetFirstOrDefault(u => u.Id == SD.historyid);

                    hist.LoginUser = ViewBag.DisplayName;
                    hist.CollateralId = collateral.Id;
                    hist.CollateralName = collateral.Name;
                    hist.Quantity = history.Quantity;
                    hist.RequestDate = history.RequestDate;
                    hist.ReconRemarks = history.ReconRemarks;
                    _unitOfWork.History.Add(hist);

                    Collateral minuscollateral = _unitOfWork
                        .Collateral.Get(SD.historyid);
                    collateral.Count -= history.Quantity;

                    _unitOfWork.Save();
                }
            }
            return RedirectToAction(nameof(GetAllItemHistory));
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            CartCount();
            var allObj = _unitOfWork.History.GetAll();
            return Json(new { data = allObj });
        }

        public IActionResult GetAllId(int id)
        {
            SD.historyid = id;
            return RedirectToAction(nameof(GetAllItemHistory));
        }

        [HttpGet]
        public IActionResult GetAllItemHistory()
        {
            UserDetails();
            CartCount();
            var allObj = _unitOfWork.History
                .GetAll(u => u.CollateralId == SD.historyid);

            //return Json(new { data = allObj });
            return View(allObj);
        }

        public IActionResult GetAllIdAddHistory(int id)
        {
            SD.historyid = id;
            return RedirectToAction(nameof(ItemAdjustment));
        }

        #endregion API CALLS

        #region UserDetails function

        public void UserDetails()
        {
            var username = User.Identity.Name;
            using (var context = new PrincipalContext(
                ContextType.Domain, SD.OfficeDomain))
            {
                var user = UserPrincipal
                    .FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
            }
        }

        public void CartCount()
        {
            string UserCart = ViewBag.DisplayName;
            if (UserCart != null)
            {
                var count = _unitOfWork.ShoppingCart
                    .GetAll(c => c.LoginUser == UserCart).ToList().Count();
                HttpContext.Session.SetObject(SD.ssShoppingCart, count);
                ViewBag.ItemCount = count;
            }
        }

        #endregion UserDetails function
    }
}