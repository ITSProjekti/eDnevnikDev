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
        [RegularExpression("(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[012])[0-9]{9}",ErrorMessage ="JMBG nije ispravan")]
        public string JMBG { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za adresu je obavezno")]
        public string Adresa { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za roditelja/staratelja je obavezno")]
        public string RoditeljStaratelj { get; set; }

    }
}