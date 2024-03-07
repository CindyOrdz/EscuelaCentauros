namespace CentaurosBackAPI.Models.DTOS
{
    public class EstudianteDTO
    {
        public uint Cedula { get; set; }

        public string Nombres { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public string Correo { get; set; } = null!;

    }
}
