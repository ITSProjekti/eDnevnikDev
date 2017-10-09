using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.DTOs
{
    public class DTOPredmet
    {
        public int PredmetId { get; set; }

        public string  NazivPredmeta { get; set; }

        public int TipOcenePredmetaId { get; set; }

    }
}