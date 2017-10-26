using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.DTOs
{
    public class DTOOcenaUnos
    {
        public int? Oznaka { get; set; }
        public bool? Plus { get; set; }
        public int UcenikId { get; set; }
        public int CasId { get; set; }
        public int TipOceneId { get; set; }
        public int? TipOpisneOceneId { get; set; }
        public string Napomena { get; set; }

    }
}