using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epicode_S5_L5_BackEnd_Project.Models;

namespace Epicode_S5_L5_BackEnd_Project.Controllers
{
    public class TrasgressoreController : Controller
    {
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        [HttpGet]
        public ActionResult ListaTrasgressori()
        {
            List<Trasgressore> trasgressori = new List<Trasgressore>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Anagrafica";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Trasgressore trasgressore = new Trasgressore
                            {
                                IdAnagrafica = Convert.ToInt32(reader["IdAnagrafica"]),
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                Citta = reader["Citta"].ToString(),
                                Cap = reader["Cap"].ToString(),
                                Codice = reader["Codice"].ToString()
                            };

                            trasgressori.Add(trasgressore);
                        }
                    }
                }

            }
             return View(trasgressori);
        }


        [HttpGet]
        public ActionResult AggiungiTrasgressore()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiTrasgressore(Trasgressore model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO Anagrafica (Cognome, Nome, Indirizzo, Citta, Cap, Codice)" + "VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @Cap, @Codice)";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@Cognome", model.Cognome);
                        cmd.Parameters.AddWithValue("@Nome", model.Nome);
                        cmd.Parameters.AddWithValue("@Indirizzo", model.Indirizzo);
                        cmd.Parameters.AddWithValue("@Citta", model.Citta);
                        cmd.Parameters.AddWithValue("@Cap", model.Cap);
                        cmd.Parameters.AddWithValue("@Codice", model.Codice);

                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Trasgressore aggiunto con successo!";
                return RedirectToAction("ListaTrasgressori");
            }
            TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            return View(model);
        }











    }
}