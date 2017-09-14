using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class TipOcene
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TipOcene"/> class.
        /// </summary>
        public TipOcene()
        {
            Ocene = new HashSet<Ocena>();
        }

        /// <summary>
        /// Gets or sets the tip ocene identifier.
        /// </summary>
        /// <value>
        /// The tip ocene identifier.
        /// </value>
        public int TipOceneId { get; set; }

        /// <summary>
        /// Gets or sets the tip.
        /// </summary>
        /// <value>
        /// The tip.
        /// </value>
        public string Tip { get; set; }

        /// <summary>
        /// Gets or sets the ocene.
        /// </summary>
        /// <value>
        /// The ocene.
        /// </value>
        public virtual ICollection<Ocena> Ocene { get; set; }
    }
}