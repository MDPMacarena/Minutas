using Microsoft.AspNetCore.Mvc;
using Minutas.Models;
using Minutas.Repositories;
using Minutas.Areas.Admin.Models;

namespace Minutas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartamentoController : Controller
    {
        private readonly DepartamentoRepository _depaRepo;
        private readonly EmpleadoRepository _empleadoRepo;

        public DepartamentoController(DepartamentoRepository depaRepo, EmpleadoRepository empleadoRepo)
        {
            _depaRepo = depaRepo;
            _empleadoRepo = empleadoRepo;
        }

        // **Vista principal para listar departamentos**
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var departamentos = _depaRepo.GetDepartamentosActivos();
                var empleados = _empleadoRepo.GetEmpleadosActivos();
                ViewBag.Empleados = empleados;
                ViewBag.Departamentos = departamentos;
                return View(departamentos);
            }
            catch (Exception ex)
            {
                return Content("Error al cargar los departamentos: " + ex.Message);
            }
        }

        // **Obtenemos lista de departamentos para ser mostrados en el frontend**
        [HttpGet("Lista")]
        public IActionResult GetDepartamentos()
        {
            var departamentos = _depaRepo.GetDepartamentosActivos();
            return Json(departamentos);
        }

        // **Formulario para agregar un nuevo departamento**
        [HttpGet]
        public IActionResult Agregar()
        {
            var vm = new AgregarDepartamentoViewModel
            {
                Empleados = _empleadoRepo.GetEmpleadosActivos(),  // Para elegir un jefe
                Departamentos = _depaRepo.GetDepartamentosActivos() // Para elegir un departamento superior
            };

            return View(vm);
        }

        // **Lógica para agregar un nuevo departamento**
        [HttpPost]
        public IActionResult Agregar(AgregarDepartamentoViewModel vm)
        {
            if (!_depaRepo.ValidarDepartamento(vm.Departamento, out string errores, out string avisos))
            {
                TempData["ErrorAgregar"] = errores;
                return RedirectToAction("Index");
            }

            _depaRepo.Insert(vm.Departamento); // Insertar nuevo departamento
            TempData["SuccessAgregar"] = "Departamento agregado correctamente.";

            if (!string.IsNullOrEmpty(avisos))
                TempData["SuccessAgregar"] += " | " + avisos;

            return RedirectToAction("Index");
        }

        // **Formulario para editar departamento**
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var departamento = _depaRepo.Get(id);
            if (departamento == null)
                return NotFound();

            var dto = new
            {
                departamento.Id,
                departamento.Nombre,
                departamento.IdJefe,
                departamento.IdDeptSuperior
            };

            return Json(dto);
        }

        // **Lógica para editar departamento**
        [HttpPost]
        public IActionResult Editar(Departamento departamento)
        {
            if (!_depaRepo.ValidarDepartamento(departamento, out string errores, out string avisos))
            {
                TempData["ErrorEditar"] = errores;
                return RedirectToAction("Index");
            }

            _depaRepo.EditarDepartamento(departamento);

            TempData["SuccessEditar"] = "Departamento editado correctamente." +
                (!string.IsNullOrEmpty(avisos) ? " | " + avisos : "");

            return RedirectToAction("Index");
        }

        // **Confirmación de eliminación del departamento**
        [HttpPost]
        public IActionResult EliminarConfirmado(int id)
        {
            var departamento = _depaRepo.Get(id);
            if (departamento != null)
            {
                _depaRepo.Eliminar(departamento);  // Eliminación o desactivación lógica
                TempData["SuccessEliminar"] = $"Departamento {departamento.Nombre} eliminado correctamente.";
            }

            return Json(new { success = true });
        }
    }





}
