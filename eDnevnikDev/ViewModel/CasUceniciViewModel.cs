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
        public CasUceniciViewModel(Cas c, List<UcenikSaPrisustvomViewModel> u, List<OcenaViewModel> o, PredmetCasViewModel p)
        {
            Cas = c;
            Ucenici = u;
            listaOcena=o;
            Predmet = p;
        }
        public Cas Cas { get; set; }

        public List<UcenikSaPrisustvomViewModel> Ucenici { get; set; }
        public PredmetCasViewModel Predmet { get; set; }
        public List<OcenaViewModel> listaOcena { get; set; }


    }
}