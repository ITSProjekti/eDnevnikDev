using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class UpisCasaViewModel
    {
        [Required(ErrorMessage = "Unesite naziv")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Unesite opis")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Izaberite predmet")]
        public int PredmetId { get; set; }

        [Required(ErrorMessage = "Izaberite razred")]
        public int Razred { get; set; }

        [Required(ErrorMessage = "Izaberite odeljenje")]
        public int Odeljenje { get; set; }

        [Range(0, 8, ErrorMessage = "Redni broj časa nije validan")]
        [Required(ErrorMessage = "Unesite redni broj časa")]
        public int RedniBrojCasa { get; set; }

        [Range(1, 200, ErrorMessage ="Redni broj predmeta nije validan")]
        [Required(ErrorMessage = "Unesite redni broj predmeta")]
        public int RedniBrojPredmeta { get; set; }

        public DateTime Datum { get; set; }

        public bool Greska { get; set; }

        public string OpisGreske { get; set; }

    }
}