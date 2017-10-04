using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Predmet
    {
        public Predmet()
        {
            Profesori = new HashSet<Profesor>();
            Casovi = new HashSet<Cas>();
            Odeljenja = new HashSet<Odeljenje>();
        }

        /// <summary>
        /// Gets or sets the Predmet identifier
        /// Primary key u bazi.
        /// </summary>
        /// <value>
        /// The predmet identifier.
        /// </value>
        
        public int PredmetID { get; set; }

        /// <summary>
        /// Sluzi za cuvanje naziv predmeta.
        /// </summary>
        /// <value>
        /// string
        /// </value>
        [Display(Name = "Naziv predmeta")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za naziv je obavezno")]
        [RegularExpression(@"^[A-ZŠĐČĆŽa-zšđčćž0-9'\.\-\s\,]+$", ErrorMessage = "Nisu dozoljeni specijalni karakteri")]
        public string NazivPredmeta { get; set; }

        /// <summary>
        /// Gets or sets the tip ocene predmeta.
        /// </summary>
        /// <value>
        /// The tip ocene predmeta.
        /// </value>
        public virtual TipOcenePredmeta TipOcenePredmeta { get; set; }

        /// <summary>
        /// Gets or sets the tip ocene predmeta identifier.
        /// </summary>
        /// <value>
        /// The tip ocene predmeta identifier.
        /// </value>
        [ForeignKey("TipOcenePredmeta")]
        public int TipOcenePredmetaId { get; set; }

        /// <summary>
        ///  Služi za čuvanje profesora koji predaju ovaj predmet
        /// </summary>
        /// <value>
        /// ICollection<Predmet>
        /// </value>
        public virtual ICollection<Profesor> Profesori { get; set; }

        /// <summary>
        /// Gets or sets the casovi.
        /// </summary>
        /// <value>
        /// The casovi.
        /// </value>
        public virtual ICollection<Cas> Casovi { get; set; }

        public virtual ICollection<Odeljenje> Odeljenja { get; set; }

    }
}