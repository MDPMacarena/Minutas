using Microsoft.AspNetCore.Mvc;

namespace MinutasManage.Areas.Admin.Controllers
{
    public class ConfiguracionController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
