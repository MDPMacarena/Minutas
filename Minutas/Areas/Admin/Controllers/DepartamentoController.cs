using Microsoft.AspNetCore.Mvc;
using Minutas.Models;
using Minutas.Repositories;

namespace Minutas.Areas.Admin.Controllers
{
    public class DepartamentoController : Controller
    {


        private readonly DepartamentoRepository _departamentoRepository;

        public DepartamentoController(DepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
        }

        public IActionResult Index()
        {
            var departamentos = _departamentoRepository.GetAll().OrderBy(x => x.Nombre);
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
            if (!_departamentoRepository.ValidarDepartamento(dep, out string errores))
            {
                ModelState.AddModelError("", errores);
                return View(dep);
            }

            _departamentoRepository.Insert(dep);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var departamento = _departamentoRepository.Get(id);
            if (departamento == null)
                return RedirectToAction("Index");

            return View(departamento);
        }

        [HttpPost]
        public IActionResult Editar(Departamento dep)
        {
            if (!_departamentoRepository.ValidarDepartamento(dep, out string errores))
            {
                ModelState.AddModelError("", errores);
                return View(dep);
            }

            var departamento = _departamentoRepository.Get(dep.Id);
            if (departamento == null)
                return RedirectToAction("Index");

            departamento.Nombre = dep.Nombre;
            departamento.IdJefe = dep.IdJefe;
            departamento.IdDeptSuperior = dep.IdDeptSuperior;

            _departamentoRepository.Update(departamento);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var departamento = _departamentoRepository.Get(id);
            if (departamento == null)
                return RedirectToAction("Index");

            return View(departamento);
        }

        [HttpPost]
        public IActionResult Eliminar(Departamento dep)
        {
            var departamento = _departamentoRepository.Get(dep.Id);
            if (departamento != null)
            {
                _departamentoRepository.Delete(departamento);
            }
            return RedirectToAction("Index");
        }
    }
}
