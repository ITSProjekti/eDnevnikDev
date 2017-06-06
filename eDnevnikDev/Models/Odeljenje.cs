using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Odeljenje
    {

        public Odeljenje()
                {
                    Ucenici = new HashSet<Ucenik>();
                }
        public int Id { get; set; }

        [Required(ErrorMessage = "Polje za odeljenje je obavezno")]
        [Display(Name = "Odeljenje")]
        public int Oznaka { get; set; }

        public ICollection<Ucenik> Ucenici { get; set; }

        public Smer Smer { get; set; }

        [ForeignKey("Smer")]
        public int SmerID { get; set; }


    }
}