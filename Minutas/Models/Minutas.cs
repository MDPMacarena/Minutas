using System;
using System.Collections.Generic;

namespace MinutasManage.Models;

public partial class Minutas
{
    public int Id { get; set; }

    public int IdCreador { get; set; }

    public DateOnly FechaCreacion { get; set; }

    public string Estado { get; set; } = null!;

    public int IdDepartamento { get; set; }

    public string Contenido { get; set; } = null!;

    public sbyte Privada { get; set; }

    public string Titulo { get; set; } = null!;

    public virtual Usuarios IdCreadorNavigation { get; set; } = null!;

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    public virtual ICollection<MinutaUsuario> MinutaUsuario { get; set; } = new List<MinutaUsuario>();
}
