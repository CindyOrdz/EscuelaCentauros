using System;
using System.Collections.Generic;

namespace CentaurosBackAPI.Models;

public partial class Diploma
{
    public uint IdDiploma { get; set; }

    public uint IdCurso { get; set; }

    public uint IdEstudiante { get; set; }

    public DateOnly Fecha { get; set; }

    public ulong Nro { get; set; }

    public ulong Nci { get; set; }

    public uint Ciudad { get; set; }

    public virtual Ciudade CiudadNavigation { get; set; } = null!;

    public virtual Curso IdCursoNavigation { get; set; } = null!;

    public virtual Estudiante IdEstudianteNavigation { get; set; } = null!;
}
