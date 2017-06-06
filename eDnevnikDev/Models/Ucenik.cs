using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
namespace eDnevnikDev.Models
{
    /// <summary>
    /// This is the Ucenik class
    /// </summary>
    public class Ucenik
    {
        /// <summary>
        /// Gets or sets the ucenik identifier
        /// Primary key u bazi.
        /// </summary>
        /// <value>
        /// The ucenik identifier.
        /// </value>
        public int UcenikID { get; set; }
        [Display(Name ="Ime oca / staratelja")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za ime oca/staratelja je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za ime oca/staratelja može da sadrži samo slova")]
        public string ImeOca { get; set; }

        [Display(Name = "Prezime oca / staratelja")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za prezime oca/staratelja je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za prezime oca/staratelja može da sadrži samo slova")]
        public string PrezimeOca { get; set; }

        [Display(Name = "Ime majke / staratelja")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za ime majke/staratelja je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za ime majke/staratelja može da sadrži samo slova")]
        public string ImeMajke { get; set; }

        [Display(Name = "Prezime majke / staratelja")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za prezime majke/staratelja je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za prezime majke/staratelja može da sadrži samo slova")]
        public string PrezimeMajke { get; set; }
        /// <summary>
        /// Gets or sets the IME.
        /// Sluzi za cuvanje imena ucenika.
        /// </summary>
        /// <value>
        /// The IME.
        /// </value>
        /// 
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za ime je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za ime može da sadrži samo slova")]
        public string Ime { get; set; }

        /// <summary>
        /// Gets or sets the prezime.
        /// Sluzi za cuvanje prezime ucenika.
        /// </summary>
        /// <value>
        /// The prezime.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za prezime je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Polje za prezime može da sadrži samo slova")]
        public string Prezime { get; set; }
        /// <summary>
        /// Gets or sets the JMBG.
        /// Sluzi za cuvanje JMBG ucenika.
        /// </summary>
        /// <value>
        /// The JMBG.
        /// </value>

        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za JMBG je obavezno")]
        [RegularExpression("(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[012])[0|9][0-9]{2}[0-9]{6}", ErrorMessage = "JMBG nije ispravan")]
        public string JMBG { get; set; }

        /// <summary>
        /// Gets or sets the adresa.
        /// Sluzi za cuvanje adrese ucenika.
        /// </summary>
        /// <value>
        /// The adresa.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za adresu je obavezno")]
        [RegularExpression(@"^[A-ZŠĐČĆŽa-zšđčćž0-9'\.\-\s\,]+$", ErrorMessage = "Nisu dozoljeni specijalni karakteri")]
        public string Adresa { get; set; }

        /// <summary>
        /// Služi za čuvanje mesta prebivališta
        /// </summary>
        /// <value>
        /// Mesto prebivališta.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za mesto prebivališta je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž]+ ?)+$", ErrorMessage = "Mesto prebivališta nije ispravno")]
        [Display(Name = "Mesto prebivališta")]
        public string MestoPrebivalista { get; set; }


        /// <summary>
        /// Služi za čuvanje broja telefona roditelja staratelja
        /// </summary>
        /// <value>
        /// Broj telefona
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za broj telefona roditelja je obavezno")]
        [RegularExpression(@"^\+(\d{1,3})-(\d{1,3})\/(\d{6,7})$", ErrorMessage = "Broj telefona roditelja nije ispravan (format: +381-11/1234567)")]
        [Display(Name = "Broj telefona roditelja")]
        public string BrojTelefonaRoditelja { get; set; }


        /// <summary>
        /// Navigacioni property, referencira grad u tabeli Grad
        /// </summary>
        /// <value>
        /// Grad
        /// </value>
        [ForeignKey("MestoRodjenjaId")]
        [Display(Name = "Mesto Rođenja")]
        [Required(ErrorMessage = "Polje za mesto rođenja je obavezno")]
        public Grad MestoRodjenja { get; set; }

        /// <summary>
        /// Properti koji cuva id Mesta rodjenja
        /// </summary>
        /// <value>
        /// int
        /// </value>
        public int MestoRodjenjaId { get; set; }

        /// <summary>
        ///  Služi za čuvanje da li je ucenik redovan ili vandredan
        /// </summary>
        /// <value>
        /// bool
        /// </value>
        public bool Vanredan { get; set; }


        /// <summary>
        /// Navigacioni property, referencira smer u tabeli Smer
        /// </summary>
        /// <value>
        /// Smer
        /// </value>
        [ForeignKey("SmerID")]
        [Required(ErrorMessage = "Polje za smer je obavezno")]
        public Smer Smer { get; set; }

        /// <summary>
        /// Properti koji cuva id Smera
        /// </summary>
        /// <value>
        /// int
        /// </value>
        public int SmerID { get; set; }

        public Odeljenje Odeljenje { get; set; }

        [ForeignKey("Odeljenje")]
        public int OdeljenjeId { get; set; }

        [Required(ErrorMessage = "Polje za razred je obavezno")]
        public byte Razred { get; set; }



    }
}