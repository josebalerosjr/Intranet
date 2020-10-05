using Intranet.Classes;
using Intranet.DataAccess.Repository.IRepository;
using Intranet.Models.CorpComm;
using Intranet.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;

namespace Intranet.Areas.CorpComm.Controllers
{
    [Area("CorpComm")]
    [Route("CorpComm/[controller]/[Action]/{id?}")]
    public class EmailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailOptions _emailOptions;

        public EmailController(IUnitOfWork unitOfWork, IOptions<EmailOptions> emailOptions)
        {
            _unitOfWork = unitOfWork;
            _emailOptions = emailOptions.Value;
        }

        public IActionResult Index()
        {
            UserDetails();
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            UserDetails();
            Email email = new Email();

            if (id == null)
            {
                // for create
                return View(email);
            }

            email = _unitOfWork.Email.Get(id.GetValueOrDefault());
            if (email == null)
            {
                return NotFound();
            }
            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Email email)
        {
            UserDetails();
            if (ModelState.IsValid)
            {
                if (email.Id == 0)
                {
                    _unitOfWork.Email.Add(email);
                }
                else
                {
                    _unitOfWork.Email.Update(email);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(email);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Email.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Email.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Email.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion API CALLS

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
    }
}