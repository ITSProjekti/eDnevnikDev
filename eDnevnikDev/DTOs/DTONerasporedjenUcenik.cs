using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.DTOs
{
    public class DTONerasporedjenUcenik
    {
        public int UcenikId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public string Smer { get; set; }
    }
}