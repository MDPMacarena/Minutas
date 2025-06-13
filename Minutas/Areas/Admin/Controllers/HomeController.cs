using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var departamento = User.FindFirst("Departamento")?.Value;

            ViewBag.Rol = rol;
            ViewBag.Departamento = departamento;

            return View();
        }
    }
}
