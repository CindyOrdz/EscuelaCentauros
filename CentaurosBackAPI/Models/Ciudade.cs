using System;
using System.Collections.Generic;

namespace CentaurosBackAPI.Models;

public partial class Ciudade
{
    public uint IdCiudad { get; set; }

    public string Ciudad { get; set; } = null!;

    public virtual ICollection<Diploma> Diplomas { get; set; } = new List<Diploma>();
}
