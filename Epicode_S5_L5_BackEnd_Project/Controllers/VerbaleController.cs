using Epicode_S5_L5_BackEnd_Project.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epicode_S5_L5_BackEnd_Project.Controllers
{
    public class VerbaleController : Controller
    {

        private List<Violazione> listaViolazioni;
        private List<Trasgressore> listaTrasgressori;

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        [HttpGet]
        public List<Violazione> ListaViolazioni()
        {
            if (listaViolazioni == null)
            {
                listaViolazioni = new List<Violazione>();

                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT * FROM Violazione";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Violazione violazione = new Violazione
                                {
                                    IdViolazione = (int)reader["IdViolazione"],
                                    Descrizione = reader["Descrizione"].ToString(),
                                };

                                listaViolazioni.Add(violazione);
                            }
                        }
                    }
                }
            }
            return listaViolazioni;
        }


        [HttpGet]
        public List<Trasgressore> ListaTrasgressori()
        {
            if (listaTrasgressori == null)
            {
                listaTrasgressori = new List<Trasgressore>();

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
                                    IdAnagrafica = (int)reader["IdAnagrafica"],
                                    Cognome = reader["Cognome"].ToString(),
                                    Nome = reader["Nome"].ToString(),
                                    Indirizzo = reader["Indirizzo"].ToString(),
                                    Citta = reader["Citta"].ToString(),
                                    Cap = reader["Cap"].ToString(),
                                    Codice = reader["Codice"].ToString()
                                };

                                listaTrasgressori.Add(trasgressore);
                            }
                        }
                    }
                }
            }
            return listaTrasgressori;
        }


        [HttpGet]
        public ActionResult AggiungiVerbale()
        {
            if (listaViolazioni == null)
            {
                listaViolazioni = ListaViolazioni();
            }

            if (listaTrasgressori == null)
            {
                listaTrasgressori = ListaTrasgressori();
            }

            var trasgressoriSelectList = new SelectList(listaTrasgressori, "IdAnagrafica", "AnagraficaCompleta");

            var violazioniSelectList = new SelectList(listaViolazioni, "IdViolazione", "ViolazioneCompleta");

            ViewBag.ListaAnagrafica = trasgressoriSelectList;
            ViewBag.ListaViolazioni = violazioniSelectList;

            return View();
        }

    }

}