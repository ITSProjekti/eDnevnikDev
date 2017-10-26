using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class CasUceniciViewModel
    {
        public CasUceniciViewModel()
        {

        }
        public CasUceniciViewModel(Cas c, List<Ucenik> u, List<Predmet> p)
        {
            Cas = c;
            Ucenici = u;
            Predmeti = p;
        }
        public Cas Cas { get; set; }

        public List<Ucenik> Ucenici { get; set; }
        public List<Predmet> Predmeti { get; set; }


    }
}