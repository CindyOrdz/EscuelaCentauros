using System;
using System.Collections.Generic;

namespace CentaurosBackAPI.Models;

public partial class Estudiante
{
    public uint Cedula { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public uint? IdUsuario { get; set; }

    public virtual ICollection<Diploma> Diplomas { get; set; } = new List<Diploma>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
