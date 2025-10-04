using Microsoft.AspNetCore.Mvc;

namespace Narratify.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            switch (statusCode)
            {
                case 403:
                    return View("AccessDenied");
                case 404:
                    return View("NotFound");
                default:
                    return View("Error");
            }
        }
    }
}
