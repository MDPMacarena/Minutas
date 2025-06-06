using Microsoft.AspNetCore.Mvc;

namespace MinutasManage.Controllers
{
    public class PerfilController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
