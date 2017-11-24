using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class SkolskaGodina
    {
        public SkolskaGodina()
        {
            Polugodista = new HashSet<Polugodiste>();
        }

        public int SkolskaGodinaId { get; set; }

        [Display(Name ="Početak školske godine")]
        [Required(ErrorMessage ="Polje je obavezno")]
        public DateTime PocetakSkolskeGodine { get; set; }

        [Display(Name = "Kraj školske godine")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public DateTime KrajSkolskeGodine { get; set; }

        public bool? Aktuelna { get; set; }

        public virtual ICollection<Polugodiste> Polugodista { get; set; }
    }
}