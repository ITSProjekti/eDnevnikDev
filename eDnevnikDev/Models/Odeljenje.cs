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






        [Required(ErrorMessage = "Polje za oznaku je obavezno")]
        public int OznakaID { get; set; }

        /// <summary>
        /// Gets or sets the oznaka.
        /// </summary>
        /// <value>
        /// The oznaka.
        /// </value>
        [Display(Name = "Oznaka")]
        public Oznaka Oznaka { get; set; }



        [ForeignKey("Status")]
        public int StatusID { get; set; }

        public Status Status { get; set; }

        public int PocetakSkolskeGodine { get; set; }
        public int KrajSkolskeGodine { get; set; }

        public int Razred { get; set; }


        /// <summary>
        /// Gets or sets the ucenici. Kolekcija Ucenika
        /// </summary>\
        /// <value>
        /// The ucenici.
        /// </value>
        public virtual ICollection<Ucenik> Ucenici { get; set; }

        public static int SledecaSkolskaGodina(int razred, int odeljenje, ApplicationDbContext _context)
        {
            var odeljenja = _context.Odeljenja.Where(o => o.OznakaID == odeljenje && o.Razred == razred).ToList();

            if (odeljenja.Any())
                return odeljenja.Max(o => o.KrajSkolskeGodine);
            else
                return DateTime.Now.Year;
        }


    }
}