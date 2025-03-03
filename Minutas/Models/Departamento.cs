using System;
using System.Collections.Generic;

namespace Minutas.Models;

public partial class Departamento
{
    public int Id { get; set; }

    public int IdJefe { get; set; }

    public int IdDepSuperior { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Minuta> Minuta { get; set; } = new List<Minuta>();

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
