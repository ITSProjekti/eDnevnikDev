using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class ListaPredmetaViewModel
    {
        public List<Predmet> ListaPredmeta { get; set; }
        public bool DodatPredmet { get; set; }
        public bool IzmenjenPredmet { get; set; }


    }
}