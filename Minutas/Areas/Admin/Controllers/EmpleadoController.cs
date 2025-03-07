using Microsoft.AspNetCore.Mvc;
using Minutas.Areas.Admin.Models;
using Minutas.Models;
using Minutas.Repositories;


namespace Minutas.Areas.Admin.Controllers
{
   
        [Area("Admin")]
        public class EmpleadoController : Controller
        {
            public EmpleadoRepository empRepository { get; }
            public DepartamentoRepository depaRepository { get; }

            public EmpleadoController(EmpleadoRepository empleadoRepository, DepartamentoRepository departamentoRepository)
            {
                empRepository = empleadoRepository;
                depaRepository = departamentoRepository;
            }

            public IActionResult Index()
            {
                var empleados = empRepository.GetEmpleadosActivos();
                return View(empleados);
            }

            [HttpGet]
            public IActionResult Agregar()
            {
                var vm = new AgregarEmpleadoViewModel
                {
                    Departamento = depaRepository.GetDepartamentosActivos()
                };

                return View(vm);
            }


            [HttpPost]
            public IActionResult Agregar(AgregarEmpleadoViewModel vm)
            {

                if (!empRepository.ValidarEmpleado(vm.Usuario, out string errores))
                {
                    vm.Departamento = depaRepository.GetDepartamentosActivos();
                    ModelState.AddModelError("", errores);
                    return View(vm);
                }


                vm.Usuario.ContraseñaHash = "REUNIONES";


                empRepository.Insert(vm.Usuario);

                return RedirectToAction("Index");
            }

            [HttpGet]
            public IActionResult Editar(int id)
            {
                var empleado = empRepository.Get(id);
                if (empleado == null)
                    return RedirectToAction("Index");

                return View(empleado);
            }

            [HttpPost]
            public IActionResult Editar(Usuarios empleado)
            {
                if (!empRepository.ValidarEmpleado(empleado, out string errores))
                {
                    ModelState.AddModelError("", errores);
                    return View(empleado);
                }

                empRepository.EditarEmpleado(empleado);
                return RedirectToAction("Index");
            }

            [HttpGet]
            public IActionResult Eliminar(int id)
            {
                var empleado = empRepository.Get(id);
                if (empleado == null)
                    return RedirectToAction("Index");

                return View(empleado);
            }

            [HttpPost]
            public IActionResult EliminarConfirmado(int id)
            {
                var empleado = empRepository.Get(id);
                if (empleado != null)
                {
                    empRepository.Eliminar(empleado);
                }
                return Json(new { success = true });
            }
        }

    
}
