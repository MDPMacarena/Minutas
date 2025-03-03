using System;
using System.Collections.Generic;

namespace Minutas.Models;

public partial class Minuta
{
    public int Id { get; set; }

    public int IdCreador { get; set; }

    public DateOnly FechaCreacion { get; set; }

    public string Contenidos { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public int IdDepartamento { get; set; }

    public virtual Usuario IdCreadorNavigation { get; set; } = null!;

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;
}
