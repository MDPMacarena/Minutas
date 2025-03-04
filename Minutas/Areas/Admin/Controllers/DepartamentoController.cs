using Microsoft.AspNetCore.Mvc;

namespace Minutas.Areas.Admin.Controllers
{
    public class DepartamentoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
