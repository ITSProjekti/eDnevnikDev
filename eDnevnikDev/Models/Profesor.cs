using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eDnevnikDev.Models
{
    public class Profesor
    {
        public Profesor()
        {
            Predmeti = new HashSet<Predmet>();
        }

        public int ProfesorID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za ime je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za ime može da sadrži samo slova")]
        public string Ime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za prezime je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za prezime može da sadrži samo slova")]
        public string Prezime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za broj telefona  je obavezno")]
        [RegularExpression(@"^\+(\d{1,3})-(\d{1,3})\/(\d{6,7})$", ErrorMessage = "Broj telefona roditelja nije ispravan (format: +381-11/1234567)")]
        [Display(Name = "Broj telefona ")]
        public string Telefon { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za adresu je obavezno")]
        [RegularExpression(@"^[A-Za-z0-9'\.\-\s\,]+$", ErrorMessage = "Nisu dozoljeni specijalni karakteri")]
        public string Adresa { get; set; }
        
        public bool Vanredan { get; set; }
        [Display(Name = "Razredni Starešina")]
        public bool RazredniStaresina { get; set; }


        public ICollection<Predmet> Predmeti { get; set; }


    }
}