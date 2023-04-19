namespace Foro.Models
{
    public class Respuesta
    {
        public int IdRespuesta { get; set; }
        public int IdPregunta { get; set; }
        public int IdUsuarioRespuesta { get; set; }
        public string respuesta { get; set; }
        public DateTime Fecha { get; set; }
        public string titulo { get; set; }
        public string nombre { get; set; }
    }
}
