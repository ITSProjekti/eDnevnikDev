using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class ArhivaOdeljenja
    {
        public int ArhivaOdeljenjaID { get; set; }
        public int OdeljenjeID { get; set; }

        public Odeljenje Odeljenje { get; set; }
        public int UcenikID { get; set; }

        public Ucenik Ucenik { get; set; }
    }
}