using Microsoft.AspNetCore.Mvc;
using Minutas.Models;
using Minutas.Repositories;

namespace Minutas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartamentoController : Controller
    {
        public DepartamentoRepository DepaRepository { get; }


        public DepartamentoController(DepartamentoRepository departamentoRepository)
        {
            DepaRepository = departamentoRepository;
        }


        public IActionResult Index()
        {
            var departamentos = DepaRepository.GetDepartamentosActivos();
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
            if (!DepaRepository.ValidarDepartamento(dep, out string errores))
            {
                ModelState.AddModelError("", errores);
                return View(dep);
            }

            DepaRepository.Insert(dep);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var departamento = DepaRepository.Get(id);
            if (departamento == null)
                return RedirectToAction("Index");

            return View(departamento);
        }

        [HttpPost]
        public IActionResult Editar(Departamento dep)
        {
            if (!DepaRepository.ValidarDepartamento(dep, out string errores))
            {
                ModelState.AddModelError("", errores);
                return View(dep);
            }

            DepaRepository.EditarDepartamento(dep);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var departamento = DepaRepository.Get(id);
            if (departamento == null)
                return RedirectToAction("Index");

            return View(departamento);
        }

        [HttpPost]
        public IActionResult Eliminar(Departamento dep)
        {
            var departamento = DepaRepository.Get(dep.Id);
            if (departamento != null)
            {
                DepaRepository.Eliminar(departamento);
            }
            return RedirectToAction("Index");
        }

    }
}
