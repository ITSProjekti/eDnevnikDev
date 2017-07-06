using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Evidencija
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public int PredmetID { get; set; }
        public int UcenikID { get; set; }
        public int ProfesorID { get; set; }


        public virtual Predmet Predmet { get; set; }

        public virtual Ucenik Ucenik { get; set; }
        public virtual Profesor Profesor { get; set; }

    }
}