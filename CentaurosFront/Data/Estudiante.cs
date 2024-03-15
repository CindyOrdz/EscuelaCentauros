using System.ComponentModel.DataAnnotations;

namespace CentaurosFront.Data
{
    public class Estudiante
    {
        [Required(ErrorMessage = "El campo cedula es obligatoria")]
        public int Cedula { get; set; }

        [Required(ErrorMessage = "El campo nombres es obligatorio")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "El campo apellidos es obligatorio")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El campo Correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo Correo no tiene un formato válido")]
        public string Correo { get; set; } = null!;
    }
}
