
using Microsoft.EntityFrameworkCore;
using Minutas.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
// Configura el contexto de la base de datos
builder.Services.AddDbContext<Minutas.Models.DbminutasContext>(options =>
    options.UseMySql("server=localhost;user=root;password=root;database=dbminutas", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql")));

// Agrega esta línea para registrar DepartamentoRepository
builder.Services.AddScoped<DepartamentoRepository>();

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
