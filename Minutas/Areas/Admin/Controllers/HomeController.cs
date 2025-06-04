using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MinutasManage.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class HomeController : Controller
    {
        [Route("/admin/home/index")]
        [Route("/admin/home")]
        [Route("/admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
