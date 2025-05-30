using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinutasManage.Models;
using System.Security.Claims;

namespace MinutasManage.Controllers
{
    public class AccountController : Controller
    {
        private DbminutasContext Context { get; }

        private readonly PasswordHasher<Usuarios> _passwordHasher;

        public AccountController(DbminutasContext context)
        {
            Context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contraseña)
        {
            var usuario = await Context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Activo == true);

            if (usuario == null || _passwordHasher.VerifyHashedPassword(usuario, usuario.ContraseñaHash, contraseña) != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.IdRolNavigation.Nombre),
                new Claim("Correo", usuario.Correo)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(Usuarios model, string contraseña)
        {
            if (await Context.Usuarios.AnyAsync(u => u.Correo == model.Correo))
            {
                ModelState.AddModelError("Correo", "Este correo ya está registrado.");
                return View(model);
            }

            model.ContraseñaHash = _passwordHasher.HashPassword(model, contraseña);
            model.Activo = true;
            model.IdRol = 2; // Asignar un rol por defecto, como "Usuario"
            model.FechaNacimiento = model.FechaNacimiento; // Asegúrate que venga correctamente desde el formulario

            Context.Usuarios.Add(model);
            await Context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
