using Intranet.Classes;
using Intranet.DataAccess.Repository.CorpComm.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Models.ViewModels;
using Intranet.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[action]/{id?}")]
    public class CollateralController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly AppSettings _appSettings;

        public CollateralController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            UserDetails();

            string CartObject = ViewBag.DisplayName;
            if (CartObject != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.LoginUser == CartObject).ToList().Count();
                HttpContext.Session.SetObject(SD.ssShoppingCart, count);
            }

            return View();
        }

        public IActionResult Upsert(int? id)
        {
            UserDetails();
            CollateralVM collateralVM = new CollateralVM()
            {
                Collateral = new Collateral(),
                SizeList = _unitOfWork.Size.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                UnitList = _unitOfWork.Unit.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                BrandList = _unitOfWork.Brand.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                LocationList = _unitOfWork.Location.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                // for create
                return View(collateralVM);
            }

            collateralVM.Collateral = _unitOfWork.Collateral.Get(id.GetValueOrDefault());
            if (collateralVM.Collateral == null)
            {
                return NotFound();
            }
            return View(collateralVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CollateralVM collateralVM)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"img\CorpComm\Collaterals");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (collateralVM.Collateral.ImgUrl != null)
                    {
                        // this is an edit and we need to remove old image
                        var imagePath = Path.Combine(webRootPath, collateralVM.Collateral.ImgUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    collateralVM.Collateral.ImgUrl = @"\img\CorpComm\Collaterals\" + fileName + extension;
                }
                else
                {
                    // only for update when they do not change the image
                    if (collateralVM.Collateral.Id != 0)
                    {
                        Collateral objFromDb = _unitOfWork.Collateral.Get(collateralVM.Collateral.Id);
                        collateralVM.Collateral.ImgUrl = objFromDb.ImgUrl;
                    }
                }

                if (collateralVM.Collateral.Id == 0)
                {
                    _unitOfWork.Collateral.Add(collateralVM.Collateral);
                }
                else
                {
                    _unitOfWork.Collateral.Update(collateralVM.Collateral);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                collateralVM.SizeList = _unitOfWork.Size.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                collateralVM.UnitList = _unitOfWork.Unit.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                collateralVM.BrandList = _unitOfWork.Brand.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                collateralVM.LocationList = _unitOfWork.Location.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                if (collateralVM.Collateral.Id != 0)
                {
                    collateralVM.Collateral = _unitOfWork.Collateral.Get(collateralVM.Collateral.Id);
                }
            }
            return View(collateralVM);
        }

        public IActionResult Catalog()
        {
            IEnumerable<Collateral> collateralList1 = _unitOfWork.Collateral.GetAll(s => s.isActive == true);
            return View(collateralList1);
        }

        public IActionResult Details(int id)
        {
            UserDetails();

            var collateralFromDb = _unitOfWork.Collateral.GetFirstOrDefault(u => u.Id == id, includeProperties: "Size,Unit,Brand,Location");
            ShoppingCart cartFromDB = new ShoppingCart()
            {
                Collateral = collateralFromDb,
                CollateralId = collateralFromDb.Id
            };

            return View(cartFromDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ShoppingCart CartObject)
        {
            UserDetails();

            CartObject.Id = 0;

            if (ModelState.IsValid)
            {
                //var claimsIdentity = (ClaimsIdentity)User.Identity.Name;
                //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObject.LoginUser = ViewBag.DisplayName;
                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    u => u.LoginUser == CartObject.LoginUser &&
                         u.CollateralId == CartObject.CollateralId, includeProperties: "Collateral");
                if (cartFromDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(CartObject);
                }
                else
                {
                    cartFromDb.Count += CartObject.Count;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                }
                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCart.GetAll(c=>c.LoginUser == CartObject.LoginUser).ToList().Count();
                HttpContext.Session.SetObject(SD.ssShoppingCart, count);
                var obj =  HttpContext.Session.GetObject<ShoppingCart>(SD.ssShoppingCart);

                return RedirectToAction(nameof(Catalog));
            }
            else
            {
                var collateralFromDb = _unitOfWork.Collateral.GetFirstOrDefault(u => u.Id == CartObject.CollateralId, includeProperties: "Size,Unit,Brand,Location");
                ShoppingCart cartFromDB = new ShoppingCart()
                {
                    Collateral = collateralFromDb,
                    CollateralId = collateralFromDb.Id
                };

                return View(cartFromDB);
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Collateral.GetAll(includeProperties: "Size,Unit,Brand,Location");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Collateral.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImgUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitOfWork.Collateral.Remove(objFromDb);
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