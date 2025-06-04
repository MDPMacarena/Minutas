using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MinutasManage.Models;
using MinutasManage.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Habilita MVC
builder.Services.AddControllersWithViews();

// Configura el contexto de base de datos con MySQL
builder.Services.AddDbContext<DbminutasContext>(options =>
    options.UseMySql(
        "server=localhost;user=root;password=root;database=dbminutas;port=3307",
        ServerVersion.Parse("8.1.0-mysql")
    )
);

// Inyección de dependencias para los repositorios
builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));


// Configura la autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Middleware para servir archivos estáticos (css, js, etc.)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();

app.Run();


app.Run();
