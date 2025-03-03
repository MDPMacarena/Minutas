using System;
using System.Collections.Generic;

namespace Minutas.Models;

public partial class Usuario
{
    public bool? Activo { get; set; }

    public int Id { get; set; }

    public string NumEmpleado { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public int IdDepartamento { get; set; }

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    public virtual ICollection<Minuta> Minuta { get; set; } = new List<Minuta>();

    public virtual ICollection<MinutaUsuario> MinutaUsuario { get; set; } = new List<MinutaUsuario>();
}
