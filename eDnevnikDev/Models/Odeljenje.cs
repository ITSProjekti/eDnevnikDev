using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Odeljenje
    {
        public int Id { get; set; }
        public int Oznaka { get; set; }

        public ICollection<Ucenik> Ucenici { get; set; }

        public Odeljenje()
        {
            Ucenici = new HashSet<Ucenik>();
        }

    }
}