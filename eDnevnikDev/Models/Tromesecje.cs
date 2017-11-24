using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Tromesecje
    {
        public int TromesecjeId { get; set; }

        public int PolugodisteId { get; set; }

        [Display(Name = "Početak tromesečja")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public DateTime PocetakTromesecja { get; set; }

        [Display(Name = "Kraj tromesečja")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public DateTime KrajTromesecja { get; set; }

        [Display(Name = "Tip tromesečja")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [Range(1, 4, ErrorMessage = "Tip tromesecja mora da bude od 1 do 4")]
        public int TipTromesecja { get; set; }

        [ForeignKey("PolugodisteId")]
        public Polugodiste Polugoduste { get; set; }
    }

    //public enum TipTromesecja
    //{
    //    PrvoTromesecje = 1,
    //    DrugoTromesecje = 2,
    //    TreceTromese = 3,
    //    CetvrtoTromesecje = 4
    //};
}