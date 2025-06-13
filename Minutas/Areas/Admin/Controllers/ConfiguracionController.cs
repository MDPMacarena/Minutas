using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinutasManage.Areas.Admin.Models;
using System.Text.Json;

namespace MinutasManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "administrador")]
    public class ConfiguracionController : Controller
    {
        private readonly string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Modifi", "correo.json");

        public IActionResult Index()
        {
            ConfiguracionCorreo config;

            if (System.IO.File.Exists(rutaArchivo))
            {
                var json = System.IO.File.ReadAllText(rutaArchivo);
                config = JsonSerializer.Deserialize<ConfiguracionCorreo>(json);
            }
            else
            {
                config = new ConfiguracionCorreo
                {
                    Host = "smtp.ejemplo.com",
                    Puerto = 465,
                    Encriptacion = "SSL",
                    Remitente = "sistema@minutas.com",
                    Token = "********"
                };
            }

            return View(config);
        }

        [HttpPost]
 
        public IActionResult GuardarCampoCorreo([FromBody] CampoConfigModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Campo))
                return BadRequest(new { mensaje = "Datos inválidos" });

            bool exito = GuardarCampoEnArchivo(model.Campo, model.Valor);

            if (!exito)
                return StatusCode(500, new { mensaje = "Error al guardar la configuración" });

            return Ok(new { mensaje = "Campo guardado exitosamente" });
        }

        private bool GuardarCampoEnArchivo(string campo, string valor)
        {
            try
            {
                var json = System.IO.File.ReadAllText(rutaArchivo);
                var config = JsonSerializer.Deserialize<ConfiguracionCorreo>(json);

                switch (campo.ToLower())
                {
                    case "host":
                        config.Host = valor;
                        break;
                    case "puerto":
                        if (int.TryParse(valor, out int puerto))
                            config.Puerto = puerto;
                        else
                            return false;
                        break;
                    case "encriptacion":
                        config.Encriptacion = valor;
                        break;
                    case "remitente":
                        config.Remitente = valor;
                        break;
                    case "token":
                        config.Token = valor;
                        break;
                    default:
                        return false;
                }

                var nuevoJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(rutaArchivo, nuevoJson);

                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
