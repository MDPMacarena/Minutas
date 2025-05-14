using Microsoft.AspNetCore.Mvc;
using MinutasManage.Areas.Admin.Models;
using MinutasManage.Models;
using MinutasManage.Repositories;


namespace MinutasManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmpleadoController : Controller
    {
        public EmpleadoRepository empRepository { get; }
        public DepartamentoRepository depaRepository { get; }
        public DbminutasContext Context { get; }

        public EmpleadoController(EmpleadoRepository empleadoRepository, DepartamentoRepository departamentoRepository, DbminutasContext context)
        {
            empRepository = empleadoRepository;
            depaRepository = departamentoRepository;
            Context = context;
        }

        public IActionResult Index()
        {
            var empleados = empRepository.GetEmpleadosActivos();
            ViewBag.Departamentos = depaRepository.GetDepartamentosActivos(); // Para el modal
            ViewBag.Roles = Context.Roles.Select(r => new { r.Id, r.Nombre }).ToList();

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
            if (!empRepository.ValidarEmpleado(vm.Usuario, out string errores, out string aviso))
            {
                TempData["ErrorAgregar"] = errores;
                return RedirectToAction("Index");
            }

            vm.Usuario.ContraseñaHash = "REUNIONES";
            vm.Usuario.IdRol = 3;
            empRepository.Insert(vm.Usuario);

            TempData["SuccessAgregar"] = "Empleado agregado correctamente";
            if (!string.IsNullOrEmpty(aviso))
                TempData["SuccessAgregar"] += " | " + aviso;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var empleado = empRepository.Get(id);
            if (empleado == null)
                return NotFound();

            var dto = new
            {
                empleado.Id,
                empleado.NumEmpleado,
                empleado.Nombre,
                empleado.Correo,
                empleado.IdDepartamento,
                empleado.IdRol,
                empleado.FechaNacimiento
            };

            return Json(dto);
        }

        [HttpPost]
        public IActionResult Editar(Usuarios empleado)
        {
            if (!empRepository.ValidarEmpleado(empleado, out string errores, out string aviso))
            {
                TempData["ErrorEditar"] = errores;
                return RedirectToAction("Index");
            }

            empRepository.EditarEmpleado(empleado);

            TempData["SuccessEditar"] = "Empleado editado correctamente" +
                (!string.IsNullOrEmpty(aviso) ? " | " + aviso : "");

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult EliminarConfirmado(int id)
        {
            try
            {
                var empleado = empRepository.Get(id);
                if (empleado == null)
                    return Json(new { success = false, message = "Usuario no encontrado" });

                bool eraJefe = Context.Departamento.Any(d => d.IdJefe == id);
                empRepository.Eliminar(empleado);

                string mensaje = eraJefe
                    ? $"Usuario {empleado.NumEmpleado}-{empleado.Nombre} (jefe de departamento) ha sido desactivado"
                    : $"Usuario {empleado.NumEmpleado}-{empleado.Nombre} eliminado correctamente";

                TempData["SuccessEliminar"] = mensaje;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ErrorEliminar"] = "Error al procesar la solicitud: " + ex.Message;
                return Json(new { success = false });
            }
        }

    }


}
