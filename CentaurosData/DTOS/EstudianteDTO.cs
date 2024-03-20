using System.ComponentModel.DataAnnotations;

namespace CentaurosData.DTOS
{
    public class EstudianteDTO
    {
        [Required(ErrorMessage = "El campo cedula es obligatoria")]
        [StringLength(10, ErrorMessage = "El número de cédula debe tener como máximo 10 dígitos.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de cédula debe contener solo números.")]
        public string Cedula { get; set; } = null!;

        [Required(ErrorMessage = "El campo nombres es obligatorio")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "El campo apellidos es obligatorio")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El campo Correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo Correo no tiene un formato válido")]
        public string Correo { get; set; } = null!;
    }
}
