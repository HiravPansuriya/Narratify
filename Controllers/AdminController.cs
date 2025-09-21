using Microsoft.AspNetCore.Mvc;

namespace Narratify.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Posts()
        {
            return View();
        }
    }
}
