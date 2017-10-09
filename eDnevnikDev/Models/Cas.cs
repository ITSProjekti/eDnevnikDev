using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Cas
    {
        public Cas()
        {
            Napomene = new HashSet<Napomena>();
            Odsustva = new HashSet<Odsustvo>();
            Ocene = new HashSet<Ocena>();
        }
        public int CasId { get; set; }

        [Required(ErrorMessage ="Unesite datum")]
        public DateTime Datum { get; set; }

        [Required(ErrorMessage = "Unesite naziv časa")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Unesite opis časa")]
        public string Opis { get; set; }

        public virtual Profesor Profesor { get; set; }

        [ForeignKey("Profesor")]
        public int ProfesorId { get; set; }

        public virtual Predmet Predmet { get; set; }

        [ForeignKey("Predmet")]
        public int PredmetId { get; set; }

        public virtual Odeljenje Odeljenje { get; set; }

        [ForeignKey("Odeljenje")]
        public int OdeljenjeId { get; set; }

        [Required]
        [Range(1,2)]
        public int Polugodiste { get; set; }

        [Required]
        [Range(1, 4)]
        public int Tromesecje { get; set; }


        [Required]
        [Range(0,8)]
        public int RedniBrojCasa { get; set; }


        [Required]
        public int RedniBrojPredmeta { get; set; }

        public virtual ICollection<Napomena> Napomene { get; set; }
        public virtual ICollection<Odsustvo> Odsustva { get; set; }
        public virtual ICollection<Ocena> Ocene { get; set; }



    }
}