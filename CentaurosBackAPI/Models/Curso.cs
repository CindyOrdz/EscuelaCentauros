using System;
using System.Collections.Generic;

namespace CentaurosBackAPI.Models;

public partial class Curso
{
    public uint IdCurso { get; set; }

    public string Curso1 { get; set; } = null!;

    public uint HorasDuracion { get; set; }

    public virtual ICollection<Diploma> Diplomas { get; set; } = new List<Diploma>();
}
