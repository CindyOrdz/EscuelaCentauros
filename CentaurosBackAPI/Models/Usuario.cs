using System;
using System.Collections.Generic;

namespace CentaurosBackAPI.Models;

public partial class Usuario
{
    public uint IdUsuario { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    public virtual ICollection<Role> IdRols { get; set; } = new List<Role>();
}
