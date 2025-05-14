using Microsoft.AspNetCore.Mvc;

namespace MinutasManage.Areas.Admin.Controllers
{
    public class PerfilController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
