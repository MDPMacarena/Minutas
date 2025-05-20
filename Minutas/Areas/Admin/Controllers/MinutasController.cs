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
        private readonly MinutasRepository _minutaRepo;
        private readonly EmpleadoRepository _empleadoRepo;
        private readonly DepartamentoRepository _departamentoRepo;
        public MinutasController(DepartamentoRepository dep, EmpleadoRepository emp, MinutasRepository min)
        {
            _minutaRepo = min;
            _empleadoRepo = emp;
            _departamentoRepo = dep;
        }
        public IActionResult Index()
        {
            return View();

        }



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
            //int id = int.Parse(User.FindFirst("Id")?.Value);
            int id = 10;
            Minutas min = new Minutas()
            {
                IdCreador = id,
                FechaCreacion = DateOnly.FromDateTime(DateTime.Today),
                Estado = "PorFirmar",

                IdDepartamento = 10,
                Contenido = minuta.Contenido,

                Privada = 0,
                Titulo = "Pruebaaa1"



            };
            //agregar lista de externos a contenido da igual ponerlo allí porque solo se usa para visualizar

            _minutaRepo.Insert(min);




            return View();
        }

    }
}
