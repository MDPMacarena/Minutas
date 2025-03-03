using System;
using System.Collections.Generic;

namespace Minutas.Models;

public partial class MinutaUsuario
{
    public int IdUsuario { get; set; }

    public int IdMinuta { get; set; }

    public bool? Firmado { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
