using Microsoft.AspNetCore.Mvc;
using MinutasManage.Areas.Admin.Models;
using MinutasManage.Models;
using MinutasManage.Repositories;


namespace MinutasManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmpleadoController : Controller
    {
        public Repository<Usuarios> empRepository { get; }
        public Repository<Departamento> depaRepository { get; }

        public Repository<Roles> rolRepository { get; }

        public EmpleadoController(Repository<Usuarios> empleadoRepository, Repository<Departamento> departamentoRepository, Repository<Roles> repositoryr)
        {
            rolRepository = repositoryr;
            empRepository = empleadoRepository;
            depaRepository = departamentoRepository;
        }

        public IActionResult Index()
        {
            var empleados = empRepository.GetAll().Where(x => x.Activo == true).OrderBy(x => x.Nombre);
            ViewBag.Departamentos = depaRepository.GetAll().Where(x => x.Activo == true).OrderBy(x => x.Nombre); // Para el modal
            ViewBag.Roles = rolRepository.GetAll().Select(r => new { r.Id, r.Nombre }).ToList();

            return View(empleados);
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            var vm = new AgregarEmpleadoViewModel
            {
                Departamento = depaRepository.GetAll().Where(x => x.Activo == true).OrderBy(x => x.Nombre)
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult Agregar(AgregarEmpleadoViewModel vm)
        {
            //// Validación del empleado
            //if (!empRepository.ValidarEmpleado(vm.Usuario, out string errores, out string aviso))
            //{
            //    TempData["ErrorAgregar"] = errores;
            //    TempData["AbrirModalCrearUsuario"] = true;

            //    // Persistencia de datos en TempData para rellenar el formulario
            //    TempData["Nombre"] = vm.Usuario.Nombre;
            //    TempData["Correo"] = vm.Usuario.Correo;
            //    TempData["Departamento"] = vm.Usuario.IdDepartamento;
            //    TempData["Rol"] = vm.Usuario.IdRol;
            //    TempData["NumEmpleado"] = vm.Usuario.NumEmpleado;
            //    TempData["FechaNacimiento"] = vm.Usuario.FechaNacimiento.ToString("yyyy-MM-dd");

            //    return RedirectToAction("Index");
            //}

            // Setear campos que no vienen del formulario
            vm.Usuario.ContraseñaHash = "REUNIONES"; // ¡Psst! Esto debería ser un hash real, no una palabra secreta visible 😅
            vm.Usuario.IdRol = 3; // Rol por defecto (puede ser redundante si ya lo mandas desde el form)

            empRepository.Insert(vm.Usuario);

            // Toast de éxito
            TempData["SuccessAgregar"] = "Empleado agregado correctamente";
            //if (!string.IsNullOrWhiteSpace(aviso))
            //    TempData["SuccessAgregar"] += $" | {aviso}";

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
            //if (!empRepository.ValidarEmpleado(empleado, out string errores, out string aviso))
            //{
            //    TempData["ErrorEditar"] = errores;
            //    return RedirectToAction("Index");
            //}

            empRepository.Update(empleado);

            //TempData["SuccessEditar"] = "Empleado editado correctamente" +
            //    (!string.IsNullOrEmpty(aviso) ? " | " + aviso : "");

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

                bool eraJefe = depaRepository.GetAll().Any(d => d.IdJefe == id);
                empRepository.Delete(empleado);

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
