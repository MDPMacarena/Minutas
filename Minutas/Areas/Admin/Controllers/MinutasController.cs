using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinutasManage.Areas.Admin.Models;
using MinutasManage.Models;
using MinutasManage.Repositories;

namespace MinutasManage.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class MinutasController : Controller
    {
        private readonly Repository<Minutas> _minutaRepo;
        private readonly Repository<Usuarios> _empleadoRepo;
        private readonly Repository<Departamento> _departamentoRepo;
        public MinutasController(Repository<Departamento> dep, Repository<Usuarios> emp, Repository<Minutas> min)
        {
            _minutaRepo = min;
            _empleadoRepo = emp;
            _departamentoRepo = dep;
        }
        public IActionResult Index()
        {
            return View();

        }


        [HttpGet]
        public IActionResult Minutas()
        {
            var minutas = _minutaRepo.GetAll()
       .Select(e => new {
           e.Id,
           e.Titulo,
           e.Contenido
       })
       .ToList();
            
            return Json(minutas);
        }


        [HttpPost]
        public IActionResult Agregar(AgregarMinutaViewModel minuta)
        {
            User.Claims.ToList();
            int id = int.Parse(User.FindFirst("Id")?.Value);
            var empleado = _empleadoRepo.Get(id);
            var departamento = _departamentoRepo.Get(empleado.IdDepartamento);
            string[] nombre= departamento.Nombre.Split(" ");
            string titulo = "";
            foreach(string n in nombre)
            {
                titulo += n[0];
            }
            titulo += "-" + DateTime.Now.ToString("yy/MM/dd").Replace("/","");
            Random rnd = new Random();
            titulo += "-" + rnd.Next(1000, 9999).ToString("000");


            Minutas min = new Minutas()
            {
                IdCreador = empleado.Id,
                FechaCreacion = DateOnly.FromDateTime(DateTime.Today),
                Estado = "PorFirmar",
                IdDepartamento = empleado.IdDepartamento,
                Contenido = minuta.Contenido,
                Privada = minuta.Privada,
                Titulo = titulo
            };
            //agregar lista de externos a contenido da igual ponerlo allí porque solo se usa para visualizar

            _minutaRepo.Insert(min);




            return Json(min);
        }

    }
}
