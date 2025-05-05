using Microsoft.AspNetCore.Mvc;
using Minutas.Models;
using Minutas.Repositories;

namespace Minutas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Departamento")]
    public class DepartamentoController : Controller
    {
        public DepartamentoRepository DepaRepository { get; }

        public DepartamentoController(DepartamentoRepository departamentoRepository)
        {
            DepaRepository = departamentoRepository;
        }

        // Obtener departamentos activos (ej: para llenar la tabla con JS)
        [HttpGet("Lista")]
        public IActionResult GetDepartamentos()
        {
            var departamentos = DepaRepository.GetDepartamentosActivos();
            return Json(departamentos);
        }

        // Agregar nuevo departamento (desde formulario JS)
        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Departamento dep)
        {
            if (!DepaRepository.ValidarDepartamento(dep, out string errores))
            {
                return BadRequest(new { success = false, message = errores });
            }

            DepaRepository.Insert(dep);
            return Ok(new { success = true });
        }

        // Editar departamento
        [HttpPost("Editar")]
        public IActionResult Editar([FromBody] Departamento dep)
        {
            if (!DepaRepository.ValidarDepartamento(dep, out string errores))
            {
                return BadRequest(new { success = false, message = errores });
            }

            DepaRepository.EditarDepartamento(dep);
            return Ok(new { success = true });
        }

        // Eliminar departamento (baja lógica)
        [HttpPost("EliminarConfirmado/{id}")]
        public IActionResult EliminarConfirmado(int id)
        {
            var departamento = DepaRepository.Get(id);
            if (departamento != null)
            {
                DepaRepository.Eliminar(departamento);
            }
            return Ok(new { success = true });
        }


    }
}
