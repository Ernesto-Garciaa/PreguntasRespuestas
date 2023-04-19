namespace Foro.Models
{
    public class PreguntaModicarModel
    {
        public int IdUsuario { get; set; }
        public int idPregunta { get; set; }
        public string? NombreUsuario { get; set; }
        public string? TituloPregunta { get; set; }
        public string? ContenidoPregunta { get; set; }
        public DateTime FechaPregunta { get; set; }
        public string? estado { get; set; }
    }
}
