using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDnevnikDev.ViewModel
{
    public class KreirajOdeljenjeViewModel
    {
        public List<NerasporedjenUcenikViewModel> ListaNerasporedjenihUcenika { get; set; }
        public List<RasporedjenUcenikViewModel> ListaRasporedjenihUcenika { get; set; }
        public int Razred{ get; set; }
        public int Odeljenje { get; set; }
        public bool DodatiUceniciUOdeljenje { get; set; }
        public bool NisuDodatiUceniciUOdeljenje { get; set; }

    }
}