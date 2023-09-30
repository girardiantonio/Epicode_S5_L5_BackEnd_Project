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

        private Verbale GetVerbaleById(int IdVerbale)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Verbale WHERE IdVerbale = @IdVerbale";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@IdVerbale", IdVerbale);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Verbale verbale = new Verbale
                            {
                                IdVerbale = (int)reader["IdVerbale"],
                                DataViolazione = (DateTime)reader["DataViolazione"],
                                IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                                NominativoAgente = reader["NominativoAgente"].ToString(),
                                DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                                Importo = (decimal)reader["Importo"],
                                DecurtamentoPunti = (int)reader["DecurtamentoPunti"],
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                IdViolazione = (int)reader["IdViolazione"]
                            };
                            return verbale;
                        }
                        return null;
                    }
                }
            }
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
        public ActionResult ListaVerbali()
        {
            List<Verbale> verbali = new List<Verbale>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Verbale";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Verbale verbale = new Verbale
                            {
                                IdVerbale = (int)reader["IdVerbale"],
                                DataViolazione = (DateTime)reader["DataViolazione"],
                                IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                                NominativoAgente = reader["NominativoAgente"].ToString(),
                                DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                                Importo = (decimal)reader["Importo"],
                                DecurtamentoPunti = (int)reader["DecurtamentoPunti"],
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                IdViolazione = (int)reader["IdViolazione"],
                            };

                            verbali.Add(verbale);
                        }
                    }
                }

            }
            return View(verbali);
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

        [HttpPost]
        public ActionResult AggiungiVerbale(Verbale model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO Verbale (DataViolazione, IndirizzoViolazione, NominativoAgente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, IdAnagrafica, IdViolazione)" + "VALUES (@DataViolazione, @IndirizzoViolazione, @NominativoAgente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @IdAnagrafica, @IdViolazione)";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@DataViolazione", model.DataViolazione);
                        cmd.Parameters.AddWithValue("@IndirizzoViolazione", model.IndirizzoViolazione);
                        cmd.Parameters.AddWithValue("@NominativoAgente", model.NominativoAgente);
                        cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", model.DataTrascrizioneVerbale);
                        cmd.Parameters.AddWithValue("@Importo", model.Importo);
                        cmd.Parameters.AddWithValue("@DecurtamentoPunti", model.DecurtamentoPunti);
                        cmd.Parameters.AddWithValue("@IdAnagrafica", model.IdAnagrafica);
                        cmd.Parameters.AddWithValue("@IdViolazione", model.IdViolazione);

                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Verbale aggiunto con successo!";
                return RedirectToAction("ListaVerbali");
            }
            TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaVerbale(int IdVerbale)
        {
            Verbale verbaleDaEliminare = GetVerbaleById(IdVerbale);

            if (verbaleDaEliminare == null)
            {
                TempData["Errore"] = "Violazione non trovata!";
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Verbale WHERE IdVerbale = @IdVerbale";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdVerbale", IdVerbale);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Verbale eliminato con successo!";
            }
            return RedirectToAction("ListaVerbali");
        }

        [HttpGet]
        public ActionResult ModificaVerbale(int IdVerbale)
        {
            Verbale verbaleDaModificare = GetVerbaleById(IdVerbale);

            if (verbaleDaModificare == null)
            {
                TempData["Errore"] = "Verbale non trovato!";
            }

            return View(verbaleDaModificare);
        }

        [HttpPost]
        public ActionResult ModificaVerbale(Verbale verbaleModificato)
        {
            if (ModelState.IsValid)
            {
                string query =
                    "UPDATE Verbale SET " +
                    "DataViolazione = @DataViolazione, " +
                    "IndirizzoViolazione = @IndirizzoViolazione, " +
                    "NominativoAgente = @NominativoAgente, " +
                    "DataTrascrizioneVerbale = @DataTrascrizioneVerbale, " +
                    "Importo = @Importo, " +
                    "DecurtamentoPunti = @DecurtamentoPunti " +
                    "WHERE IdVerbale = @IdVerbale";

                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdVerbale", verbaleModificato.IdVerbale);
                        cmd.Parameters.AddWithValue("@DataViolazione", verbaleModificato.DataViolazione);
                        cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbaleModificato.IndirizzoViolazione);
                        cmd.Parameters.AddWithValue("@NominativoAgente", verbaleModificato.NominativoAgente);
                        cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbaleModificato.DataTrascrizioneVerbale);
                        cmd.Parameters.AddWithValue("@Importo", verbaleModificato.Importo);
                        cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbaleModificato.DecurtamentoPunti);

                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Verbale modificato con successo!";
            }
            return RedirectToAction("ListaVerbali");
        }


    }

}