using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.DTOs
{
    public class DTOUcenik
    {
        public string Id { get; set; }

        public string JMBG { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public List<string> Role { get; set; }
    }
}