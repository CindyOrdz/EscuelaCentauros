using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CentaurosFront.Data
{
    public class Usuario
    {
        [Required(ErrorMessage = "El campo correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo Correo no tiene un formato válido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "El campo contraseña es obligatorio")]
        [PasswordPropertyText]
        public string Clave { get; set; } = null!;
    }

}
