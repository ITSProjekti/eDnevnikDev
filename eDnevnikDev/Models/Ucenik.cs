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
        [RegularExpression(@"^([A-ZŠĐČĆŽ]{1}[a-zšđčćž]+ ?)+$", ErrorMessage ="Polje za ime može da sadrži samo slova i mora da počinje velikim slovom")]
        public string Ime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za prezime je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽ]{1}[a-zšđčćž]+ ?)+$", ErrorMessage = "Polje za prezime može da sadrži samo slova i mora da počinje velikim slovom")]
        public string Prezime { get; set; }
        public string JMBG { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za adresu je obavezno")]
        public string Adresa { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za roditelja/staratelja je obavezno")]
        public string RoditeljStaratelj { get; set; }

    }
}