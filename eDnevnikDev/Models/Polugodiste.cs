using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Polugodiste
    {
        public Polugodiste()
        {
            Tromesecja = new HashSet<Tromesecje>();
        }

        public int PolugodisteId { get; set; }

        public int SkolskaGodinaId { get; set; }

        [Display(Name = "Početak polugodišta")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public DateTime PocetakPolugodista { get; set; }

        [Display(Name = "Kraj polugodišta")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public DateTime KrajPolugodista { get; set; }

        [Display(Name = "Tip polugodišta")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [Range(1, 2, ErrorMessage = "Tip polugodista moze da bude 1 ili 2")]
        public int TipPolugodista { get; set; }

        [ForeignKey("SkolskaGodinaId")]
        public SkolskaGodina SkolskaGodina { get; set; }

        public virtual ICollection<Tromesecje> Tromesecja { get; set; }
    }

    //public enum TipPolugodista
    //{
    //    PrvoPolugodiste = 1,
    //    DrugoPolugodiste = 2
    //};
}