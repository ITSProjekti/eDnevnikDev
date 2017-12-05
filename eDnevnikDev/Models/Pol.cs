using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Pol
    {
        public Pol()
        {
            Ucenici = new HashSet<Ucenik>();
            Profesori = new HashSet<Profesor>();
        }

        public int PolId { get; set; }

        [Display(Name = "Smer")]
        public string Naziv { get; set; }

        public virtual ICollection<Ucenik> Ucenici { get; set; }

        public virtual ICollection<Profesor> Profesori { get; set; }
    }
}