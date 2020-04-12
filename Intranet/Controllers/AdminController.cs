using Microsoft.AspNetCore.Mvc;

namespace Intranet.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}