using Foro.Data;
using Microsoft.AspNetCore.Mvc;
using Foro.Models;
using System.Data.SqlClient;
using System.Data;

namespace Foro.Controllers
{
    public class RespuestasController : Controller
    {
        private readonly DbContext _context;

        public RespuestasController(DbContext context)
        {
            _context = context;
        }

     

        public IActionResult AgregarRespuesta(int idPregunta, int idUsuario)
        {
            ViewBag.idPregunta = idPregunta;
            ViewBag.idUsuario = idUsuario;
            return View();
        }

        [HttpPost]
        public IActionResult AgregarRespuesta(int idPregunta, int idUsuario, string respuesta)
        {
            using (SqlConnection connection = new(_context.Valor))
            {
                var command = new SqlCommand("AgregarRespuesta", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idPregunta", idPregunta);
                command.Parameters.AddWithValue("@idUsuarioRespuesta", idUsuario);
                command.Parameters.AddWithValue("@respuesta", respuesta);
                command.Parameters.AddWithValue("@fecha", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToAction("mostrarPreguntas", "Preguntas");
        }



        public IActionResult ConsultarRespuestas()
        {
            List<Respuesta> respuestas = new List<Respuesta>();

            using (SqlConnection connection = new(_context.Valor))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("ConsultarRespuestas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Respuesta respuesta = new Respuesta
                            {
                                IdRespuesta = Convert.ToInt32(reader["idRespuesta"]),
                                IdPregunta = Convert.ToInt32(reader["idPregunta"]),
                                IdUsuarioRespuesta = Convert.ToInt32(reader["idUsuarioRespuesta"]),
                                respuesta = reader["respuesta"].ToString(),
                                Fecha = Convert.ToDateTime(reader["fecha"]),
                                titulo = reader["titulo"].ToString(),
                                nombre = reader["nombre_usuario"].ToString()
                            };

                            respuestas.Add(respuesta);
                        }
                    }
                }
            }

            return View(respuestas);
        }











    }


    

    
}
