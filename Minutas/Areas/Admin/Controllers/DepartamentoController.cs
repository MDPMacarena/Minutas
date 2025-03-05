using Microsoft.AspNetCore.Mvc;
using Minutas.Models;

namespace Minutas.Areas.Admin.Controllers
{
    public class DepartamentoController : Controller
    {

     
        [HttpGet]
        public IActionResult Agregar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Agregar(Departamento dep)
        {
            return View();
        }
    }
}
