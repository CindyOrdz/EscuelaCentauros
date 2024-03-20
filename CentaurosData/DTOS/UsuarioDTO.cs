using System.ComponentModel.DataAnnotations;

namespace CentaurosData.DTOS
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "El campo correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo Correo no tiene un formato válido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "El campo contraseña es obligatorio")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = null!;
    }
}
