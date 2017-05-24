using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Ucenik
    {
        public int UcenikID { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Polje za ime je obavezno")]
        public string Ime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za prezime je obavezno")]
        public string Prezime { get; set; }

        [Required(AllowEmptyStrings =false,ErrorMessage ="Polje za JMBG je obavezno")]
        [RegularExpression("(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[012])[0|9][0-9]{2}[0-9]{6}", ErrorMessage ="JMBG nije ispravan")]
        public string JMBG { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za adresu je obavezno")]
        [RegularExpression(@"^[A-Za-z0-9'\.\-\s\,]+$",ErrorMessage ="Nisu dozoljeni specijalni karakteri")]
        public string Adresa { get; set; }

        [Display(Name = "Roditelj")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za roditelja/staratelja je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽ]{1}[a-zšđčćž]+ ?)+$", ErrorMessage = "Ime roditelja nije ispravno (Prvo slovo mora biti veliko)")]
        public string RoditeljStaratelj { get; set; }

    }
}