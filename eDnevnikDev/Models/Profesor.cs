using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eDnevnikDev.Models
{
    public class Profesor
    {
        public int ProfesorID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za ime je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za ime može da sadrži samo slova")]
        public string Ime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za prezime je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za prezime može da sadrži samo slova")]
        public string Prezime { get; set; }
    }
}