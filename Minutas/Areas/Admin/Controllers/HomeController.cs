using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinutasManage.Models;
using MinutasManage.Repositories;
using System.Security.Claims;

namespace MinutasManage.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class HomeController : Controller
    {
        public Repository<Usuarios> Repository { get; }

        public HomeController(Repository<Usuarios> repository)
        {
            Repository = repository;
        }



        [Route("/admin/home/index")]
        [Route("/admin/home")]
        [Route("/admin")]
        
        public IActionResult Index()
        {
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var departamento = User.FindFirst("Departamento")?.Value;

            User.Claims.ToList();
            int id = int.Parse(User.FindFirst("Id")?.Value);
            var user = Repository.Get(id);



            ViewBag.Name = user.Nombre;
            ViewBag.Rol = rol;
            ViewBag.Departamento = departamento;


            return View();
        }


    }
}
