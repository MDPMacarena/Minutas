using Microsoft.AspNetCore.Mvc;

namespace Minutas.Controllers
{
    public class PerfilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
