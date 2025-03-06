using Microsoft.AspNetCore.Mvc;
using Minutas.Models;
using Minutas.Repositories;

namespace Minutas.Areas.Admin.Controllers
{
    public class DepartamentoController : Controller
    {


        private readonly DepartamentoRepository depaRepository;

        public DepartamentoController(DepartamentoRepository departamentoRepository)
        {
            depaRepository = departamentoRepository;
        }

        public IActionResult Index()
        {
            var departamentos = depaRepository.GetDepartamentosActivos();
            return View(departamentos);
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(Departamento dep)
        {
            if (!depaRepository.ValidarDepartamento(dep, out string errores))
            {
                ModelState.AddModelError("", errores);
                return View(dep);
            }

            depaRepository.Insert(dep);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var departamento = depaRepository.Get(id);
            if (departamento == null)
                return RedirectToAction("Index");

            return View(departamento);
        }

        [HttpPost]
        public IActionResult Editar(Departamento dep)
        {
            if (!depaRepository.ValidarDepartamento(dep, out string errores))
            {
                ModelState.AddModelError("", errores);
                return View(dep);
            }

            depaRepository.EditarDepartamento(dep);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var departamento = depaRepository.Get(id);
            if (departamento == null)
                return RedirectToAction("Index");

            return View(departamento);
        }

        [HttpPost]
        public IActionResult Eliminar(Departamento dep)
        {
            var departamento = depaRepository.Get(dep.Id);
            if (departamento != null)
            {
                depaRepository.Eliminar(departamento);
            }
            return RedirectToAction("Index");
        }
    }
}
