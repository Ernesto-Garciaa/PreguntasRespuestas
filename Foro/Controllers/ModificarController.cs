using Foro.Data;
using Foro.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Foro.Controllers
{
    public class ModificarController : Controller
    {


        private readonly DbContext _context;

        public ModificarController(DbContext context)
        {
            _context = context;
        }

        public IActionResult Modificar(int id)
        {
            PreguntaModificarModel pregunta = GetPreguntaById(id);
            if (pregunta == null)
            {
                return NotFound();
            }
            return View(pregunta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modificar(int id, PreguntaModificarModel pregunta)
        {
            if (id != pregunta.IdPregunta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new(_context.Valor))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("ModificarPregunta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@idPregunta", pregunta.IdPregunta);
                        command.Parameters.AddWithValue("@idUsuario", pregunta.IdUsuario);
                        command.Parameters.AddWithValue("@titulo", pregunta.Titulo);
                        command.Parameters.AddWithValue("@pregunta", pregunta.Contenido);
                        command.Parameters.AddWithValue("@fecha", pregunta.Fecha);
                        command.Parameters.AddWithValue("@estado", pregunta.Estado);

                        command.ExecuteNonQuery();
                    }
                }

                // redirigir a la acción Index después de guardar la pregunta modificada
                return RedirectToAction(nameof(Index));
            }

            // si el modelo no es válido, volver a la vista Modificar y mostrar los errores de validación
            return View("ModificarPregunta", pregunta);
        }



        private PreguntaModificarModel GetPreguntaById(int id)
        {
            PreguntaModificarModel pregunta = null;

            using (SqlConnection connection = new(_context.Valor))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT idPregunta, idUsuario, titulo, pregunta, fecha, estado FROM Preguntas WHERE idPregunta = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pregunta = new PreguntaModificarModel
                            {
                                IdPregunta = reader.GetInt32(0),
                                IdUsuario = reader.GetInt32(1),
                                Titulo = reader.GetString(2),
                                Contenido = reader.GetString(3),
                                Fecha = reader.GetDateTime(4),
                                Estado = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return  pregunta;
        }
    


        public IActionResult Index()
        {
            return View();
        }
    }
}
