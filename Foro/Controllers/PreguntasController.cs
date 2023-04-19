﻿using Foro.Data;
using Microsoft.AspNetCore.Mvc;
using Foro.Models;
using System.Data.SqlClient;
using System.Data;


namespace Foro.Controllers
{

      
    public class PreguntasController : Controller
    {


         private readonly DbContext _context;
    
                public PreguntasController(DbContext context)
                {
                    _context = context;
                }


        public IActionResult InsertarPregunta(PreguntaModel pregunta)
        {
            try
            {

                using (SqlConnection connection = new(_context.Valor))
                {
                    connection.Open();
                    using (var command = new SqlCommand("InsertarPregunta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idUsuario", pregunta.IdUsuario);
                        command.Parameters.AddWithValue("@titulo", pregunta.Titulo);
                        command.Parameters.AddWithValue("@pregunta", pregunta.Contenido);
                        command.Parameters.AddWithValue("@fecha", pregunta.Fecha);
                        command.Parameters.AddWithValue("@estado", pregunta.Estado);

                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("mostrarPreguntas");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(pregunta);
            }
        }






    [HttpGet]
        public ActionResult IngresarPregunta()
        {
          
            return View("Preguntas");
        }

        [HttpPost]
        public ActionResult IngresarPregunta(PreguntasModel p)
        {
            try
            {
                if(ModelState.IsValid)
                {


                    using (SqlConnection con = new(_context.Valor))
                    {
                        using (SqlCommand cmd = new("sp_insertarPregunta", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = p.IdUsuario;
                            cmd.Parameters.Add("@titulo", SqlDbType.VarChar).Value = p.Titulo;
                            cmd.Parameters.Add("@pregunta", SqlDbType.VarChar).Value = p.Pregunta;
                            cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = DateTime.Now.Date;
                            cmd.Parameters.Add("@estado", SqlDbType.Int).Value = p.estado;
                            con.Open();
                            cmd.ExecuteNonQuery();
                           
                            con.Close();
                        }
                    }
                    
                    return RedirectToAction("mostrarPreguntas", "Preguntas");
                }

            }catch(Exception)
            {
                return View("Preguntas");
            }
            ViewData["Error"] = "La pregunta no pudo Enviarse";
            return View("mostrarPreguntas");
        }


        public IActionResult Preguntas()
        {
            // Obtener el idUsuario de la sesión
            int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("idUsuario"));

            // Asignar el idUsuario al modelo
            var model = new PreguntasModel
            {
                IdUsuario = idUsuario
            };

            return View(model);
        }




        [HttpGet]
        public ActionResult mostrarPreguntas()
        {
            try
            {
                using (SqlConnection con = new(_context.Valor))
                {
                    using (SqlCommand cmd = new("PreguntasUsuarios", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        List<PreguntaModicarModel> preguntas = new List<PreguntaModicarModel>();

                        while (reader.Read())
                        {
                            PreguntaModicarModel pregunta = new PreguntaModicarModel();
                            pregunta.IdUsuario = Convert.ToInt32(reader["idUsuario"]);
                            pregunta.idPregunta = Convert.ToInt32(reader["idPregunta"]);
                            pregunta.NombreUsuario = reader["nombreUsuario"].ToString();
                            pregunta.TituloPregunta = reader["tituloPregunta"].ToString();
                            pregunta.ContenidoPregunta = reader["pregunta"].ToString();
                            pregunta.FechaPregunta = DateTime.Parse(reader["fecha"].ToString());
                            //pregunta.estado = reader["estado"].ToString();

                            Response.Cookies.Append("user", "Bienvenido " + pregunta.IdUsuario);
                            pregunta.estado = reader["estado"].ToString();
                            preguntas.Add(pregunta);

                        }

                        con.Close();

                        return View(preguntas);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return View("Preguntas");
            }
        }


        public IActionResult Index()
        {
            return View();
        }




    }
}