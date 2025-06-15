using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinutasManage.Areas.Admin.Models;
using MinutasManage.Models;
using MinutasManage.Repositories;
using MinutasManage.Services;

namespace MinutasManage.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class MinutasController : Controller
    {
        private readonly Repository<Minutas> _minutaRepo;
        private readonly Repository<Usuarios> _empleadoRepo;
        private readonly Repository<Departamento> _departamentoRepo;
        private readonly Repository<MinutaUsuario> _minutaUsuario;

        public MinutasController(Repository<MinutaUsuario> minusr, Repository<Departamento> dep, Repository<Usuarios> emp, Repository<Minutas> min)
        {
            _minutaUsuario = minusr;
            _minutaRepo = min;
            _empleadoRepo = emp;
            _departamentoRepo = dep;
        }
        public IActionResult Index()
        {
            return View();

        }


        // [HttpGet]
        // public IActionResult Minutas()
        // {
        //     var minutas = _minutaRepo.GetAll()
        //.Select(e => new {
        //    e.Id,
        //    e.Titulo,
        //    e.Contenido
        //})
        //.ToList();

        //     return Json(minutas);
        // }
        [HttpGet]
        [Route("Minutas/{State}")]
        public IActionResult Minutas(string State)
        {
            User.Claims.ToList();
            int id = int.Parse(User.FindFirst("Id")?.Value);
            var minutas = _minutaRepo.GetAll().AsQueryable<Minutas>().Include(x=>x.MinutaUsuario).Where(x=>x.Estado==State &&(x.IdCreador==id || x.MinutaUsuario.Any(y=>y.IdUsuario==id && y.IdMinuta==x.Id)) )
       .Select(e => new {
           e.Id,
           e.Titulo,
           e.Contenido
       })
       .ToList();

            return Json(minutas);
        }


        [HttpGet]
        [Route("Minuta/{id}")]
        public IActionResult Minuta(int id)
        {
            User.Claims.ToList();
            int uid = int.Parse(User.FindFirst("Id")?.Value);
            var m = _minutaRepo.GetAll().Select(e => new {
                e.Id,
                e.Titulo,
                e.Contenido
            }).FirstOrDefault(x => x.Id == id);

            return Json(m);
        }


        [HttpPost]
        public async Task<IActionResult> testia(MinutaIAViewModel minuta)
        {
            IaService _ia = new IaService();
            var texto = await _ia.GenerarRespuestaAsync("quiero que mejores la redaccion de esta minuta, no modifiques el formato solo la redaccion NI AGREGUES NADA MAS,solo quiero que agreges informacion en los capos, quiero que tu respuesta sea unicamente el json no quiero que agregues saltos de linea ni nada, solo el json en una sola linea de texto sin formato, los compromisos son una lista de strings y ya" + minuta.Contenido); // Fixed syntax error by replacing 'Json.SERIALIZER() minuta' with 'Json.Serialize(minuta)'
            texto=texto.Replace("```json", "");
            texto = texto.Replace("```", "");


            minuta.Contenido = texto;
            return Ok(minuta);
        }

        [HttpPost]
        public IActionResult Agregar(AgregarMinutaViewModel minuta)
        {
            User.Claims.ToList();
            int id = int.Parse(User.FindFirst("Id")?.Value);
            var empleado = _empleadoRepo.Get(id);
            var departamento = _departamentoRepo.Get(empleado.IdDepartamento);
            string[] nombre= departamento.Nombre.ToUpper().Split(" ");
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

            minuta.Asistentes = minuta.Asistentes[0].Split(",");
            //agregar asistentes
            foreach(var usr in minuta.Asistentes)
            {
                var usuario = _empleadoRepo.GetAll().FirstOrDefault(x => x.NumEmpleado == usr);
                if (usuario != null)
                {
                    MinutaUsuario mu = new MinutaUsuario()
                    {
                        IdMinuta = min.Id,
                        IdUsuario = usuario.Id
                    };
                    _minutaUsuario.Insert(mu);
                }
            }
            return Json(min);
        }


        [HttpPost]
        public IActionResult AgregarBorrador(AgregarMinutaViewModel minuta)
        {
            User.Claims.ToList();
            int id = int.Parse(User.FindFirst("Id")?.Value);
            var empleado = _empleadoRepo.Get(id);
            var departamento = _departamentoRepo.Get(empleado.IdDepartamento);
            string[] nombre = departamento.Nombre.ToUpper().Split(" ");
            string titulo = "";
            foreach (string n in nombre)
            {
                titulo += n[0];
            }
            titulo += "-" + DateTime.Now.ToString("yy/MM/dd").Replace("/", "");
            Random rnd = new Random();
            titulo += "-" + rnd.Next(1000, 9999).ToString("000");


            Minutas min = new Minutas()
            {
                IdCreador = empleado.Id,
                FechaCreacion = DateOnly.FromDateTime(DateTime.Today),
                Estado = "Borrador",
                IdDepartamento = empleado.IdDepartamento,
                Contenido = minuta.Contenido,
                Privada = minuta.Privada,
                Titulo = titulo
            };
            //agregar lista de externos a contenido da igual ponerlo allí porque solo se usa para visualizar

            _minutaRepo.Insert(min);

            //minuta.Asistentes = minuta.Asistentes[0].Split(",");
            ////agregar asistentes
            //foreach (var usr in minuta.Asistentes)
            //{
            //    var usuario = _empleadoRepo.GetAll().FirstOrDefault(x => x.NumEmpleado == usr);
            //    if (usuario != null)
            //    {
            //        MinutaUsuario mu = new MinutaUsuario()
            //        {
            //            IdMinuta = min.Id,
            //            IdUsuario = usuario.Id
            //        };
            //        _minutaUsuario.Insert(mu);
            //    }
            //}

            return Json(min);
        }
        [HttpDelete]
        [Route("admin/minutas/delete/{id}")]
        public IActionResult Eliminar(int id)
        {
            var min = _minutaRepo.Get(id);
            if(min != null)
            {
                _minutaRepo.Delete(min);

            }
            return Ok();
        }

    }
}

