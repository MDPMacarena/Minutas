using Microsoft.AspNetCore.Mvc;

namespace Minutas.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
