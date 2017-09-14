using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Ocena
    {
        /// <summary>
        /// Gets or sets the ocena identifier.
        /// </summary>
        /// <value>
        /// The ocena identifier.
        /// </value>
        public int OcenaId { get; set; }

        /// <summary>
        /// Gets or sets the oznaka.
        /// </summary>
        /// <value>
        /// The oznaka.
        /// </value>
        [Range(1,5, ErrorMessage ="Ocena mora da bude od 1 do 5")]
        public int Oznaka { get; set; }

        

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Ocena"/> is plus.
        /// </summary>
        /// <value>
        ///   <c>true</c> if plus; otherwise, <c>false</c>.
        /// </value>
        public bool Plus { get; set; }

        /// <summary>
        /// Gets or sets the ucenik identifier.
        /// </summary>
        /// <value>
        /// The ucenik identifier.
        /// </value>
        public int UcenikId { get; set; }

        /// <summary>
        /// Gets or sets the cas identifier.
        /// </summary>
        /// <value>
        /// The cas identifier.
        /// </value>
        public int CasId { get; set; }

        /// <summary>
        /// Gets or sets the tip ocene identifier.
        /// </summary>
        /// <value>
        /// The tip ocene identifier.
        /// </value>
        public int TipOceneId { get; set; }

        /// <summary>
        /// Gets or sets the tip opisne ocene identifier.
        /// </summary>
        /// <value>
        /// The tip opisne ocene identifier.
        /// </value>
        public int TipOpisneOceneId { get; set; }


        /// <summary>
        /// Gets or sets the napomena.
        /// </summary>
        /// <value>
        /// The napomena.
        /// </value>
        public string Napomena { get; set; }

        /// <summary>
        /// Gets or sets the cas.
        /// </summary>
        /// <value>
        /// The cas.
        /// </value>
        [ForeignKey("CasId")]
        public virtual Cas Cas { get; set; }

        /// <summary>
        /// Gets or sets the tip ocene.
        /// </summary>
        /// <value>
        /// The tip ocene.
        /// </value>
        [ForeignKey("TipOceneId")]
        public virtual TipOcene TipOcene { get; set; }

        /// <summary>
        /// Gets or sets the tip opisne ocene.
        /// </summary>
        /// <value>
        /// The tip opisne ocene.
        /// </value>
        [ForeignKey("TipOpisneOceneId")]
        public virtual TipOpisneOcene TipOpisneOcene { get; set; }

        /// <summary>
        /// Gets or sets the ucenik.
        /// </summary>
        /// <value>
        /// The ucenik.
        /// </value>
        [ForeignKey("UcenikId")]
        public virtual Ucenik Ucenik { get; set; }
    }
}