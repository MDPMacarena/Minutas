using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Minutas.Models;
using System.Security.Claims;

namespace Minutas.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(DbminutasContext context)
        {
            Context = context;
        }

        public DbminutasContext Context { get; }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string rol, string password)
        {

            var usuario = Context.Usuarios
                .SingleOrDefault(x => x.NumEmpleado == rol && x.ContraseñaHash == password && x.Activo == true);

            if (usuario != null)
            {
                var rolUsuario = usuario.IdRolNavigation.Nombre;


                List<Claim> claims = new List<Claim>
                {
                new Claim("Id", usuario.Id.ToString()),
                 new Claim(ClaimTypes.Role, rolUsuario)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));


                if (rolUsuario == "Admin")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }


            ModelState.AddModelError("", "El usuario o contraseña son incorrectos o el usuario está inactivo");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
