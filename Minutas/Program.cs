
using Microsoft.EntityFrameworkCore;
using Minutas.Models;
using Minutas.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
// Configura el contexto de la base de datos
//builder.Services.AddDbContext<Minutas.Models.DbminutasContext>(options =>
//    options.UseMySql("server=localhost;user=root;password=root;database=dbminutas;port=3307", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql")));

builder.Services.AddDbContext<DbminutasContext>(options =>
    options.UseMySql("server=localhost;user=root;password=root;database=dbminutas;port=3307",
        new MySqlServerVersion(new Version(8, 1, 0)))
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

// Agrega esta línea para registrar los repositorios
builder.Services.AddScoped<DepartamentoRepository>();

builder.Services.AddScoped<EmpleadoRepository>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseStaticFiles();
app.MapAreaControllerRoute(
    name: "areas",
    areaName: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
app.MapDefaultControllerRoute();

app.Run();
