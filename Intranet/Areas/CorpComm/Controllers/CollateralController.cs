using Intranet.DataAccess.Data;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Models.ViewModels.CorpComm;
using Intranet.Uti;
using Intranet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[action]/{id?}")]
    public class CollateralController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly CorpCommDbContext _context;
        private readonly Emailer _emailer;

        public CollateralController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, CorpCommDbContext context, Emailer emailer)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _context = context;
            _emailer = emailer;
        }

        public IActionResult Index()
        {
            UserDetails();
            CartCount();

            return View();
        }

        [Authorize(Roles = SD.CIOAdmin + ", " + SD.CorpCommAdmin)]
        public IActionResult Upsert(int? id)
        {
            UserDetails();
            CartCount();
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
        [Authorize(Roles = SD.CIOAdmin + ", " + SD.CorpCommAdmin)]
        public IActionResult Upsert(CollateralVM collateralVM)
        {
            UserDetails();
            CartCount();
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
            UserDetails();
            CartCount();

            IEnumerable<Collateral> collateralList1 = _unitOfWork.Collateral.GetAll(s => s.isActive == true);
            return View(collateralList1);
        }

        public IActionResult Details(int id)
        {
            UserDetails();

            CartCount();

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
            CartCount();

            CartObject.Id = 0;

            if (ModelState.IsValid)
            {
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
                CartCount();
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

        [Authorize(Roles = SD.CIOAdmin + ", " + SD.CorpCommAdmin)]
        public IActionResult Transfer(int? id)
        {
            UserDetails();
            CartCount();

            CollateralVM collateralVM = new CollateralVM()
            {
                LocationList = _unitOfWork.Location.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            collateralVM.Collateral = _unitOfWork.Collateral.Get(id.GetValueOrDefault());
            SD.collateralName = collateralVM.Collateral.Location.Name;
            SD.collateralLoc = collateralVM.Collateral.Name;
            SD.collateralLocId = collateralVM.Collateral.LocationId;
            return View(collateralVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.CIOAdmin + ", " + SD.CorpCommAdmin)]
        public IActionResult Transfer(CollateralVM collateralVM, int qtyfrom)
        {
            Collateral objMinusItem = _unitOfWork.Collateral.Get(collateralVM.Collateral.Id);
            objMinusItem.Count -= qtyfrom;

            History histMinus = new History();
            if (collateralVM.Collateral.Id != 0)
            {
                histMinus.LoginUser = ViewBag.DisplayName;
                histMinus.CollateralId = objMinusItem.Id;
                histMinus.CollateralName = objMinusItem.Name;
                histMinus.Quantity = qtyfrom;
                histMinus.RequestDate = DateTime.Now; ;

                if (SD.collateralName == SD.DO_Edsa)
                {
                    histMinus.ReconRemarks = "Transfered (" + qtyfrom + ") to " + SD.DO_LKG;
                }

                if (SD.collateralName == SD.DO_LKG)
                {
                    histMinus.ReconRemarks = "Transfered (" + qtyfrom + ") to " + SD.DO_Edsa;
                }

                _unitOfWork.History.Add(histMinus);
                _unitOfWork.Save();
            }

            if (SD.collateralName == SD.DO_LKG)
            {
                Collateral collateral = _unitOfWork.Collateral.GetFirstOrDefault(c => c.Name == SD.collateralLoc && c.LocationId != SD.collateralLocId);
                collateral.Count += qtyfrom;

                History histPlusEdsa = new History();
                if (collateralVM.Collateral.Id != 0)
                {
                    histPlusEdsa.LoginUser = ViewBag.DisplayName;
                    histPlusEdsa.CollateralId = collateral.Id;
                    histPlusEdsa.CollateralName = collateral.Name;
                    histPlusEdsa.Quantity = qtyfrom;
                    histPlusEdsa.RequestDate = DateTime.Now; ;
                    histPlusEdsa.ReconRemarks = "Transfered (" + qtyfrom + ") from " + SD.DO_LKG;

                    _unitOfWork.History.Add(histPlusEdsa);
                    _unitOfWork.Save();
                }
            }

            if (SD.collateralName == SD.DO_Edsa)
            {
                Collateral collateral = _unitOfWork.Collateral.GetFirstOrDefault(c => c.Name == SD.collateralLoc && c.LocationId != SD.collateralLocId);
                collateral.Count += qtyfrom;

                History histPlusLKG = new History();
                if (collateralVM.Collateral.Id != 0)
                {
                    histPlusLKG.LoginUser = ViewBag.DisplayName;
                    histPlusLKG.CollateralId = collateral.Id;
                    histPlusLKG.CollateralName = collateral.Name;
                    histPlusLKG.Quantity = qtyfrom;
                    histPlusLKG.RequestDate = DateTime.Now; ;
                    histPlusLKG.ReconRemarks = "Transfered (" + qtyfrom + ") from " + SD.DO_Edsa;
                    _unitOfWork.History.Add(histPlusLKG);

                    _unitOfWork.Save();
                }

                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            CartCount();
            var allObj = _unitOfWork.Collateral.GetAll(includeProperties: "Size,Unit,Brand,Location");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            CartCount();
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
            var domain = SD.OfficeDomain;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
            }
        }

        public void CartCount()
        {
            string UserCart = ViewBag.DisplayName;
            if (UserCart != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.LoginUser == UserCart).ToList().Count();
                HttpContext.Session.SetObject(SD.ssShoppingCart, count);
                ViewBag.ItemCount = count;
            }
        }

        #endregion UserDetails function
    }
}