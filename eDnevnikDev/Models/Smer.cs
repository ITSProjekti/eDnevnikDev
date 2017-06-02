using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Smer
    {
        /// <summary>
        /// Gets or sets the Smer identifier
        /// Primary key u bazi.
        /// </summary>
        /// <value>
        /// The smer identifier.
        /// </value>
        public int SmerID { get; set; }

        /// <summary>
        /// Sluzi za cuvanje naziv smera.
        /// </summary>
        /// <value>
        /// string
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za naziv smera je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za naziv smera može da sadrži samo slova")]
        [Display(Name = "Naziv smera")]
        public string NazivSmera { get; set; }

        
    }
}