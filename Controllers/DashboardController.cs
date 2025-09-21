using Microsoft.AspNetCore.Mvc;

namespace Narratify.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
