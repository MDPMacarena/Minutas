using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinutasManage.Models;
using MinutasManage.Repositories;
using System.Security.Claims;

namespace MinutasManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PerfilController : Controller
    {

        public Repository<Usuarios> Repository { get; }

        public PerfilController(Repository<Usuarios> repository)
        {
            Repository = repository;
        }


        public IActionResult Index()
        {
            // Obtener el Id del usuario desde Claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);

            // Traer el usuario desde el repositorio
            var usuario = Repository.GetAll().AsQueryable<Usuarios>().Include(u => u.IdRolNavigation)
                    .Include(u => u.IdDepartamentoNavigation)
                .Where(u => u.Id == userId)
                    
                .Select(u => new Usuarios
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    NumEmpleado = u.NumEmpleado,
                    FechaNacimiento = u.FechaNacimiento,
                    IdRolNavigation = u.IdRolNavigation,
                    IdDepartamentoNavigation = u.IdDepartamentoNavigation,
                    IdRol = u.IdRol,
                   
                })
                .FirstOrDefault();

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarPerfil(Usuarios model)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);

            var usuarioDb = Repository.GetAll().FirstOrDefault(u => u.Id == userId);
            if (usuarioDb == null)
                return NotFound();

            usuarioDb.Nombre = model.Nombre;
            usuarioDb.Correo = model.Correo;

 

            Repository.Update(usuarioDb);
           

            return RedirectToAction("Index");
        }


    }
}
    
