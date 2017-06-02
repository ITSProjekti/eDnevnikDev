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

        /// <summary>
        /// Gets or sets the Profesor identifier
        /// Primary key u bazi.
        /// </summary>
        /// <value>
        /// The profesor identifier.
        /// </value>
        public int ProfesorID { get; set; }

        /// <summary>
        /// Gets or sets the IME.
        /// Sluzi za cuvanje imena profesora.
        /// </summary>
        /// <value>
        /// string
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za ime je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za ime može da sadrži samo slova")]
        public string Ime { get; set; }


        /// <summary>
        /// Gets or sets the prezime.
        /// Sluzi za cuvanje prezime profesora.
        /// </summary>
        /// <value>
        /// string
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za prezime je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za prezime može da sadrži samo slova")]
        public string Prezime { get; set; }

        /// <summary>
        /// Sluzi za cuvanje broja telefona profeasora
        /// </summary>
        /// <value>
        /// string
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za broj telefona  je obavezno")]
        [RegularExpression(@"^\+(\d{1,3})-(\d{1,3})\/(\d{6,7})$", ErrorMessage = "Broj telefona roditelja nije ispravan (format: +381-11/1234567)")]
        [Display(Name = "Broj telefona ")]
        public string Telefon { get; set; }

        /// <summary>
        /// Gets or sets the adresa.
        /// Sluzi za cuvanje adrese profesora.
        /// </summary>
        /// <value>
        /// string
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za adresu je obavezno")]
        [RegularExpression(@"^[A-Za-z0-9'\.\-\s\,]+$", ErrorMessage = "Nisu dozoljeni specijalni karakteri")]
        public string Adresa { get; set; }

        /// <summary>
        ///  Služi za čuvanje da li je profesor redovan ili vandredan
        /// </summary>
        /// <value>
        /// bool
        /// </value>
        public bool Vanredan { get; set; }

        /// <summary>
        ///  Služi za čuvanje da li je profesor razredni starešina
        /// </summary>
        /// <value>
        /// bool
        /// </value>
        [Display(Name = "Razredni Starešina")]
        public bool RazredniStaresina { get; set; }

        /// <summary>
        ///  Služi za čuvanje predmeta na kojima profesor predaje
        /// </summary>
        /// <value>
        /// ICollection<Predmet>
        /// </value>
        public ICollection<Predmet> Predmeti { get; set; }


    }
}