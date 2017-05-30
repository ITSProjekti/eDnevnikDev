using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Predmet
    {
        public int PredmetID { get; set; }


        [Display(Name = "Naziv predmeta")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za naziv je obavezno")]
        [RegularExpression(@"^[A-ZŠĐČĆŽa-zšđčćž0-9'\.\-\s\,]+$", ErrorMessage = "Nisu dozoljeni specijalni karakteri")]
        public string NazivPredmeta { get; set; }




    }
}