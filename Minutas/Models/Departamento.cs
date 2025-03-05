using System;
using System.Collections.Generic;

namespace Minutas.Models;

public partial class Departamento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdJefe { get; set; }

    public int? IdDeptSuperior { get; set; }

    public bool? Activo { get; set; }

    public virtual Departamento? IdDeptSuperiorNavigation { get; set; }

    public virtual ICollection<Departamento> InverseIdDeptSuperiorNavigation { get; set; } = new List<Departamento>();

    public virtual ICollection<Minutas> Minutas { get; set; } = new List<Minutas>();

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
