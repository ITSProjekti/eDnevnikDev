using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.DTOs
{
    public class DTOOdeljenjeSaUcenicima
    {
        

        public bool Kreirano { get; set; }

        public List<DTOUcenikOdeljenja> Ucenici { get; set; }



    }
}