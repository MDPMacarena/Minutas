using Microsoft.AspNetCore.Mvc;
using Minutas.Models;
using Minutas.Repositories;

namespace Minutas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Departamento")]
    public class DepartamentoController : Controller
    {
        private readonly DepartamentoRepository _depaRepo;

        public DepartamentoController(DepartamentoRepository depaRepo)
        {
            _depaRepo = depaRepo;
        }

     
        [HttpGet("")]
        public IActionResult Index()
        {
            var departamentos = _depaRepo.GetDepartamentosActivos();
            return View(departamentos); 
        }

      
        [HttpGet("Lista")]
        public IActionResult GetDepartamentos()
        {
            var departamentos = _depaRepo.GetDepartamentosActivos();
            return Json(departamentos);
        }

     
        [HttpPost("Agregar")]
        public IActionResult Agregar([FromForm] Departamento dep)
        {
            if (!_depaRepo.ValidarDepartamento(dep, out string errores))
            {
                return BadRequest(new { success = false, message = errores });
            }

            _depaRepo.Insert(dep);
            return Ok(new { success = true });
        }

        [HttpPost("Editar")]
        public IActionResult Editar([FromForm] Departamento dep)
        {
            if (!_depaRepo.ValidarDepartamento(dep, out string errores))
            {
                return BadRequest(new { success = false, message = errores });
            }

            _depaRepo.EditarDepartamento(dep);
            return Ok(new { success = true });
        }

        [HttpPost("EliminarConfirmado/{id}")]
        public IActionResult EliminarConfirmado(int id)
        {
            var departamento = _depaRepo.Get(id);
            if (departamento != null)
            {
                _depaRepo.Eliminar(departamento);
            }
            return Ok(new { success = true });
        }
    }



}
