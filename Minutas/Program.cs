
using Microsoft.EntityFrameworkCore;
using MinutasManage.Models;
using MinutasManage.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
// Configura el contexto de la base de datos
builder.Services.AddDbContext<MinutasManage.Models.DbminutasContext>(options =>
    options.UseMySql("server=localhost;user=root;password=root;database=dbminutas;port=3307", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql")));



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
