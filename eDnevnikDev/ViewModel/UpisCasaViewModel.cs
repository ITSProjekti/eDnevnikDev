using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class UpisCasaViewModel
    {
        public string Naziv { get; set; }

        public string Opis { get; set; }

        public int PredmetId { get; set; }

        public int Razred { get; set; }

        public int Odeljenje { get; set; }

        public int RedniBrojCasa { get; set; }

        public int RedniBrojPredmeta { get; set; }

    }
}