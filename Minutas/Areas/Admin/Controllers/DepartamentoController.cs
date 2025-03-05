using Microsoft.AspNetCore.Mvc;
using Minutas.Models;
using Minutas.Repositories;

namespace Minutas.Areas.Admin.Controllers
{
    public class DepartamentoController : Controller
    {


        Repository<Departamento> Departamentorepository;
        public IActionResult Index()

        {
            var departamentos = Departamentorepository.GetAll().OrderBy(x => x.Nombre);
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
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(dep.Nombre))
            {
                ModelState.AddModelError("", "El Nombre esta vacio");
            }
            if (dep.IdJefe <= 0)
            {
                ModelState.AddModelError("", "El Id del Jefe no es válido");
            }
            if (dep.IdDeptSuperior <= 0)
            {
                ModelState.AddModelError("", "El Id del Departamento Superior no es válido");
            }

            if (ModelState.IsValid)
            {
                Departamentorepository.Insert(dep);
                return RedirectToAction("Index");
            }
            return View(dep);


        }

        [HttpGet]
        public IActionResult Editar(int id)
        {

            var departamento = Departamentorepository.Get(id);
            if (departamento == null)
            {
                return RedirectToAction("Index");
            }

            return View(departamento);
        }

        [HttpPost]
        public IActionResult Editar(Departamento dep)
        {
            var departamento = Departamentorepository.Get(dep.Id);
            if (departamento == null)
            {
                return RedirectToAction("Index");
            }
            departamento.Nombre = dep.Nombre;
            departamento.IdJefe = dep.IdJefe;
            departamento.IdDeptSuperior = dep.IdDeptSuperior;

            Departamentorepository.Update(departamento);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var departamento = Departamentorepository.Get(id);
            if (departamento == null)
            {
                return RedirectToAction("Index");
            }

            return View(departamento);
        }
        [HttpPost]
        public IActionResult Eliminar(Departamento dep)
        {
            var departamento = Departamentorepository.Get(dep.Id);
            if (departamento != null)
            {

                Departamentorepository.Delete(departamento);

            }
            return RedirectToAction("Index");
        }
    }
}
