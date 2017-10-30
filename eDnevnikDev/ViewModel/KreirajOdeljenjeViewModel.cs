using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class KreirajOdeljenjeViewModel
    {
        public List<Ucenik> ListaNerasporedjenihUcenika { get; set; }
        public List<Ucenik> ListaRasporedjenihUcenika { get; set; }
        public List<Odeljenje> PrvaGodina { get; set; }
        public List<Odeljenje> DrugaGodina { get; set; }
        public List<Odeljenje> TrecaGodina { get; set; }
        public List<Odeljenje> CetvrtaGodina { get; set; }

    }
}