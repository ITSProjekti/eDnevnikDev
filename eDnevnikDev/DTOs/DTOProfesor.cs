using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.DTOs
{
    public class DTOProfesor
    {
        public string Id { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public List<string> Role { get; set; }
    }
}