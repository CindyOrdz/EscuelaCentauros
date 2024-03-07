using System;
using System.Collections.Generic;

namespace CentaurosBackAPI.Models;

public partial class Role
{
    public uint IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> IdUsuarios { get; set; } = new List<Usuario>();
}
