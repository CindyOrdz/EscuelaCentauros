namespace CentaurosFront.Data
{
    public class Respuesta
    {
        public int Exito { get; set; }
        public string Mensaje { get; set; }
        public List<Estudiante> Data { get; set; }

        public Respuesta()
        {
            Exito = 0;
        }
    }
}
