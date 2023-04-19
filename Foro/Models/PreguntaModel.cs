namespace Foro.Models
{
    public class PreguntaModel
    {
        public int IdPregunta { get; set; }
        public int IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }
}
