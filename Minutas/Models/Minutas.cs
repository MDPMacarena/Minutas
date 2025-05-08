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

    public string Objetivo { get; set; } = null!;

    public string OrdenDia { get; set; } = null!;

    public string Desarrollo { get; set; } = null!;

    public string CompromisosYtareas { get; set; } = null!;

    public sbyte? Privadas { get; set; }

    public virtual Usuarios IdCreadorNavigation { get; set; } = null!;

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    public virtual ICollection<MinutaUsuario> MinutaUsuario { get; set; } = new List<MinutaUsuario>();
}
