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
        public string JMBG { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za adresu je obavezno")]
        public string Adresa { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za roditelja/staratelja je obavezno")]
        public string RoditeljStaratelj { get; set; }

    }
}