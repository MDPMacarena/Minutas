using Microsoft.AspNetCore.Mvc;
using Minutas.Repositories;
using Minutas.Models;

namespace Minutas.Areas.Admin.Controllers
{
    public class DepartamentoController : Controller
    {
        Repository<Departamento> Departamentorepository;

        public IActionResult Index()
        {
            var departamentos = Departamentorepository.GetAll().OrderBy(d => d.Nombre);

            return View(departamentos);
        }
    }
}
