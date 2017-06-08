using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    /// <summary>
    /// Klasa Odeljenje
    /// </summary>
    public class Odeljenje
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Odeljenje"/> class.
        /// </summary>
        public Odeljenje()
                {
                    Ucenici = new HashSet<Ucenik>();
                }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the oznaka.
        /// </summary>
        /// <value>
        /// The oznaka.
        /// </value>
        [Required(ErrorMessage = "Polje za odeljenje je obavezno")]
        [Display(Name = "Odeljenje")]
        public int Oznaka { get; set; }

        /// <summary>
        /// Gets or sets the ucenici. Kolekcija Ucenika
        /// </summary>
        /// <value>
        /// The ucenici.
        /// </value>
        public ICollection<Ucenik> Ucenici { get; set; }

        /// <summary>
        /// Gets or sets the smer.
        /// </summary>
        /// <value>
        /// The smer.
        /// </value>
        public Smer Smer { get; set; }

        /// <summary>
        /// Gets or sets the smer identifier. ID smera
        /// </summary>
        /// <value>
        /// The smer identifier.
        /// </value>
        [ForeignKey("Smer")]
        public int SmerID { get; set; }


    }
}