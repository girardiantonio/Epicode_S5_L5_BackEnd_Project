﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicode_S5_L5_BackEnd_Project.Models
{
    public class Trasgressore
    {
        public int IdAnagrafica { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Indirizzo { get; set; }
        public string Citta { get; set; }
        public string Cap { get; set; }
        public string Codice { get; set; }

        public string AnagraficaCompleta => $"{IdAnagrafica} - {Cognome} {Nome}";
    }
}