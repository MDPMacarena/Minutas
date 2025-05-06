using System;
using System.Collections.Generic;

namespace Minutas.Models;

public partial class Usuarios
{
    public int Id { get; set; }

    public int IdDepartamento { get; set; }

    public bool? Activo { get; set; }

    public string NumEmpleado { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string ContraseñaHash { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public int IdRol { get; set; }

    public virtual ICollection<Departamento> Departamento { get; set; } = new List<Departamento>();

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    public virtual Roles IdRolNavigation { get; set; } = null!;

    public virtual ICollection<MinutaUsuario> MinutaUsuario { get; set; } = new List<MinutaUsuario>();

    public virtual ICollection<Minutas> Minutas { get; set; } = new List<Minutas>();
}
