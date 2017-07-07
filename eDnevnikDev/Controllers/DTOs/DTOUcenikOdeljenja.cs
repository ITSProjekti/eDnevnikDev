using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.DTOs
{
    /// <summary>
    /// Sluzi Za JSON
    /// </summary>
    public class DTOUcenikOdeljenja
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ID { get; set; }
        /// <summary>
        /// Gets or sets the IME.
        /// </summary>
        /// <value>
        /// The IME.
        /// </value>
        public string Ime { get; set; }
        /// <summary>
        /// Gets or sets the prezime.
        /// </summary>
        /// <value>
        /// The prezime.
        /// </value>
        public string Prezime { get; set; }
        /// <summary>
        /// Gets or sets the fotografija.
        /// </summary>
        /// <value>
        /// The fotografija.
        /// </value>
        public string Fotografija { get; set; }
    }
}