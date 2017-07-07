using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Smer
    {
        public Smer()
        {
            Odeljenja = new HashSet<Odeljenje>();
        }

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

        /// <summary>
        /// Cuvanje Trajanje Smera
        /// </summary>
        /// <value>
        /// The trajanje.
        /// </value>
        [Required( ErrorMessage = "Polje za trajanje smera je obavezno")]
        [Display(Name = "Trajanje smera")]
        public byte Trajanje { get; set; }

        /// <summary>
        /// Kolekcija Odeljenja
        /// </summary>
        /// <value>
        /// The odeljenja.
        /// </value>
        public virtual ICollection<Odeljenje> Odeljenja { get; set; }

    }
}