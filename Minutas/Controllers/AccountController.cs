using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinutasManage.Models;
using MinutasManage.Models.ViewModels;
using System.Security.Claims;

namespace MinutasManage.Controllers
{
    public class AccountController : Controller
    {
        private DbminutasContext Context { get; }

        private readonly PasswordHasher<Usuarios> _passwordHasher = new();

        public AccountController(DbminutasContext context)
        {
            Context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lg)
        {
            var usuario = await Context.Usuarios
                .Include(u => u.IdRolNavigation)
                .Include(u => u.IdDepartamentoNavigation) // <-- Asegúrate de incluir el departamento
                .FirstOrDefaultAsync(u => u.Correo == lg.Mail && u.Activo == true);

            if (usuario == null || _passwordHasher.VerifyHashedPassword(usuario, usuario.ContraseñaHash, lg.Password) != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim("Id", usuario.Id.ToString()),

                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.IdRolNavigation.Nombre),
                new Claim("Correo", usuario.Correo),
                new Claim("Departamento", usuario.IdDepartamentoNavigation.Nombre) // <-- Agregado

            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            // Redirige a /Admin/Home/Index
            return RedirectToAction("Index", "Home", new { area = "Admin" });

        }

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (await Context.Usuarios.AnyAsync(u => u.Correo == model.Correo))
            {
                ModelState.AddModelError("Correo", "Este correo ya está registrado.");
                return View(model);
            }

            Usuarios usuario = new Usuarios();
            usuario.IdDepartamento = model.IdDepartamento;
            usuario.NumEmpleado = model.NumEmpleado;
            usuario.Nombre = model.Nombre;
            usuario.Correo = model.Correo;
            usuario.ContraseñaHash = _passwordHasher.HashPassword(usuario, model.ContraseñaHash);
            usuario.Activo = true;
            usuario.IdRol = 2; // Asignar un rol por defecto, como "Usuario"
            usuario.FechaNacimiento = model.FechaNacimiento; // Asegúrate que venga correctamente desde el formulario

            Context.Usuarios.Add(usuario);
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
