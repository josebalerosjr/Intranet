using Intranet.Data;
using Intranet.Models;
using Intranet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Controllers
{
    [Authorize(Roles = SD.CIOAdmin + "," + SD.QSHESHEAdmin + "," + SD.QSHESHEUser)]
    public class ItemRegSHEController : Controller
    {
        private readonly ItemRegSHEContext _context;
        private readonly InvManufacturerContext _contextManuf;
        private readonly InvLocationContext _contextLoc;
        private readonly InvTypeContext _contextType;
        private readonly InvUnitContext _contextUnit;
        private readonly EmailOptions _emailOptions;
        private readonly IToastNotification _toastNotification;
        private readonly EmailContext _contextEmail;

        public ItemRegSHEController(
            ItemRegSHEContext context,
            InvManufacturerContext contextManuf,
            InvLocationContext contextLoc,
            InvTypeContext contextType,
            InvUnitContext contextUnit,
            EmailContext contextEmail,
            IOptions<EmailOptions> emailOptions,
            IToastNotification toastNotification)
        {
            _context = context;
            _contextManuf = contextManuf;
            _contextLoc = contextLoc;
            _contextType = contextType;
            _contextUnit = contextUnit;
            _contextEmail = contextEmail;
            _emailOptions = emailOptions.Value;
            _toastNotification = toastNotification;
        }

        // GET: ItemRegSHE
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, string searchColumn, int? pageNumber)
        {
            UserDetails();

            #region Declaration of variables

            //  Declaration of variable
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DescSortParam"] = sortOrder == "Item Desc" ? "item_desc" : "Item Desc";
            ViewData["MfrSortParam"] = sortOrder == "Mfr" ? "mfr_desc" : "Mfr";
            ViewData["SnSortParam"] = sortOrder == "Asset/SN" ? "sn_desc" : "Asset/SN";
            ViewData["PnSortParam"] = sortOrder == "P/N" ? "pn_desc" : "P/N";
            ViewData["TypeSortParam"] = sortOrder == "Type" ? "type_desc" : "Type";
            ViewData["CALSortParam"] = sortOrder == "CAL Date" ? "cal_desc" : "CAL Date";
            ViewData["QtySortParam"] = sortOrder == "Qty" ? "qty_desc" : "Qty";
            ViewData["UnitSortParam"] = sortOrder == "Unit" ? "unit_desc" : "Unit";
            ViewData["RemSortParam"] = sortOrder == "REM" ? "rem_desc" : "REM";
            ViewData["LocSortParam"] = sortOrder == "Loc" ? "loc_desc" : "Loc";
            ViewData["currentFilter"] = searchString;
            ViewData["searchColumn"] = searchColumn;
            ViewData["CurrentSort"] = sortOrder;

            #endregion Declaration of variables

            if (searchString != null) pageNumber = 1;
            else searchString = currentFilter;

            var items = from item in _context.ItemRegSHEs select item;

            #region search function

            switch (searchColumn)
            {
                case "Name":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.ItemName.Contains(searchString));
                    break;

                case "Item Description":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.ItemDesc.Contains(searchString));
                    break;

                case "Manufacturer":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.ManufName.Contains(searchString));
                    break;

                case "Asset/SN":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.AsstSerial.Contains(searchString));
                    break;

                case "P/N":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.PartNum.Contains(searchString));
                    break;

                case "Type":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.TypeName.Contains(searchString));
                    break;

                case "CAL Date":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.CalDate.Equals(DateTime.Parse(searchString)));
                    break;

                case "Quantity":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.Qty.Equals(searchString));
                    break;

                case "Unit":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.UnitName.Contains(searchString));
                    break;

                case "Remarks":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.Remarks.Contains(searchString));
                    break;

                case "Location":
                    if (!string.IsNullOrEmpty(searchString))
                        items = items.Where(item => item.LocName.Contains(searchString));
                    break;
            }

            #endregion search function

            #region sort function

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(item => item.ItemName);
                    break;

                case "Item Desc":
                    items = items.OrderBy(item => item.ItemDesc);
                    break;

                case "item_desc":
                    items = items.OrderByDescending(item => item.ItemDesc);
                    break;

                case "Mfr":
                    items = items.OrderBy(item => item.ManufName);
                    break;

                case "mfr_desc":
                    items = items.OrderByDescending(item => item.ManufName);
                    break;

                case "Asset/SN":
                    items = items.OrderBy(item => item.AsstSerial);
                    break;

                case "sn_desc":
                    items = items.OrderByDescending(item => item.AsstSerial);
                    break;

                case "P/N":
                    items = items.OrderBy(item => item.PartNum);
                    break;

                case "pn_desc":
                    items = items.OrderByDescending(item => item.PartNum);
                    break;

                case "Type":
                    items = items.OrderBy(item => item.TypeName);
                    break;

                case "type_desc":
                    items = items.OrderByDescending(item => item.TypeName);
                    break;

                case "CAL Date":
                    items = items.OrderBy(item => item.CalDate);
                    break;

                case "cal_desc":
                    items = items.OrderByDescending(item => item.CalDate);
                    break;

                case "Qty":
                    items = items.OrderBy(item => item.Qty);
                    break;

                case "qty_desc":
                    items = items.OrderByDescending(item => item.Qty);
                    break;

                case "Unit":
                    items = items.OrderBy(item => item.UnitName);
                    break;

                case "unit_desc":
                    items = items.OrderByDescending(item => item.UnitName);
                    break;

                case "REM":
                    items = items.OrderBy(item => item.Remarks);
                    break;

                case "rem_desc":
                    items = items.OrderByDescending(item => item.Remarks);
                    break;

                case "Loc":
                    items = items.OrderBy(item => item.LocName);
                    break;

                case "loc_desc":
                    items = items.OrderByDescending(item => item.LocName);
                    break;

                default:
                    items = items.OrderBy(item => item.ItemName);
                    break;
            }

            #endregion sort function

            int pageSize = 99999;
            return View(await PaginatedList<ItemRegSHE>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: ItemRegSHE/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            UserDetails();

            #region Dropdown list function for Location, Manufacturer, Unit and type

            /**
             *  start of functions
             *  Functions for dropdown list
             */
            List<InvManufacturer> invManufacturers = new List<InvManufacturer>();
            invManufacturers = (from manuf in _contextManuf.invManufacturers select manuf).ToList();
            invManufacturers.Insert(0, new InvManufacturer { });
            ViewBag.ListOfManufacturer = invManufacturers;

            List<InvLocation> invLocations = new List<InvLocation>();
            invLocations = (from loc in _contextLoc.invLocations select loc).ToList();
            invLocations.Insert(0, new InvLocation { });
            ViewBag.ListOfLocation = invLocations;

            List<InvType> invTypes = new List<InvType>();
            invTypes = (from types in _contextType.invTypes select types).ToList();
            invTypes.Insert(0, new InvType { });
            ViewBag.ListOfType = invTypes;

            List<InvUnit> invUnits = new List<InvUnit>();
            invUnits = (from units in _contextUnit.invUnits select units).ToList();
            invUnits.Insert(0, new InvUnit { });
            ViewBag.ListOfUnit = invUnits;
            /**
             *  start of functions
             *  Functions for dropdown list
             */

            #endregion Dropdown list function for Location, Manufacturer, Unit and type

            if (id == 0)
                return View(new ItemRegSHE());
            else
                return View(_context.ItemRegSHEs.Find(id));
        }

        // POST: ItemRegSHE/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("ItemId,ItemName,ItemDesc,ManufName,AsstSerial,PartNum,TypeName,CalDate,Qty,CritLevel," +
            "UnitName,Remarks,LocName,UserName,UserIP,UserDate")] ItemRegSHE itemRegSHE)
        {
            if (ModelState.IsValid)
            {
                if (itemRegSHE.ItemId == 0)
                {
                    _context.Add(itemRegSHE);
                    addToast(); // triggers addToast
                }
                else
                {
                    _context.Update(itemRegSHE);
                    editToast(); // triggers editToast
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                warnToast(); // triggers warnToast
            }
            return View(itemRegSHE);
        }

        [HttpGet]
        public IActionResult Withdraw(int id)
        {
            return View(_context.ItemRegSHEs.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw([Bind("ItemId,Qty")] ItemReg itemReg, int quantity)
        {
            ViewBag.qty = quantity;
            //if (ModelState.IsValid) {
            var itemreg = _context.ItemRegSHEs.Find(itemReg.ItemId);
            itemreg.Qty -= ViewBag.qty;
            itemReg.UserName = ViewBag.DisplayName;
            itemreg.UserIP = HttpContext.Connection.RemoteIpAddress.ToString();
            itemreg.UserDate = DateTime.Now.ToString("MM/dd/yyyy");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //}
            //return View(itemReg);
        }

        // GET: ItemRegSHE/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetails();
            var itemreg = await _context.ItemRegSHEs.FindAsync(id);
            _context.ItemRegSHEs.Remove(itemreg);
            delToast();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemRegSHEExists(int id)
        {
            return _context.ItemRegSHEs.Any(e => e.ItemId == id);
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

        #region Toastr function

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
            _toastNotification.AddInfoToastMessage("The record already exists", new ToastrOptions()
            {
                Title = ""
            });
        }

        #endregion Toastr function
    }
}