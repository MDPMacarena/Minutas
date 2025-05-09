using System;
using System.Collections.Generic;

namespace MinutasManage.Models;

public partial class MinutaUsuario
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public bool? Firmado { get; set; }

    public int IdMinuta { get; set; }

    public virtual Minutas IdMinutaNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
