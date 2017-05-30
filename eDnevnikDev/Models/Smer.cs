using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Smer
    {
        public int SmerID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za naziv smera je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za naziv smera može da sadrži samo slova")]
        public string NazivSmera { get; set; }

        
    }
}