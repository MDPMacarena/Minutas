using Microsoft.AspNetCore.Mvc;
using MinutasManage.Models;
using MinutasManage.Repositories;
using MinutasManage.Areas.Admin.Models;

namespace MinutasManage.Areas.Admin.Controllers
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
            var departamentosExistentes = _depaRepo.GetDepartamentosActivos();

            if (!_depaRepo.ValidarDepartamento(vm.Departamento, out string errores, out string avisos, departamentosExistentes.ToList()))
            {
                TempData["ErrorAgregar"] = errores;
                TempData["AbrirModalCrear"] = true;

                // GUARDAR datos del formulario
                TempData["DepNombre"] = vm.Departamento.Nombre;
                TempData["DepJefe"] = vm.Departamento.IdJefe;
                TempData["DepSup"] = vm.Departamento.IdDeptSuperior;

                return RedirectToAction("Index");
            }

            _depaRepo.Insert(vm.Departamento);
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
        public IActionResult Editar(AgregarDepartamentoViewModel vm)
        {
            var departamentosExistentes = _depaRepo.GetDepartamentosActivos();

            if (!_depaRepo.ValidarDepartamento(vm.Departamento, out string errores, out string avisos, departamentosExistentes.ToList()))
            {
                TempData["ErrorEditar"] = errores;
                return RedirectToAction("Index");
            }

            _depaRepo.Update(vm.Departamento); // Actualizar el departamento
            TempData["SuccessEditar"] = "Departamento actualizado correctamente.";

            if (!string.IsNullOrEmpty(avisos))
                TempData["SuccessEditar"] += " | " + avisos;

            return RedirectToAction("Index");
        }



        // **Confirmación de eliminación del departamento**
        [HttpPost]
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                var departamento = _depaRepo.Get(id);
                if (departamento == null)
                    return Json(new { success = false, message = "Departamento no encontrado" });

                var departamentosExistentes = _depaRepo.GetDepartamentosActivos();

                if (!_depaRepo.ValidarDepartamento(departamento, out string errores, out _, departamentosExistentes.ToList(), esEliminacion: true))
                {
                    return Json(new
                    {
                        success = false,
                        message = errores,
                        showToast = true // Indicador para mostrar toast en el cliente
                    });
                }

                _depaRepo.Eliminar(departamento);
                return Json(new
                {
                    success = true,
                    message = $"Departamento '{departamento.Nombre}' eliminado correctamente.",
                    showToast = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error inesperado: " + ex.Message,
                    showToast = true
                });
            }
        }


    }

}
