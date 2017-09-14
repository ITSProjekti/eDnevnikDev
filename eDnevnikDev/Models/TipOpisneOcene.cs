using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class TipOpisneOcene
    {
        public TipOpisneOcene()
        {
            Ocene = new HashSet<Ocena>();
        }
        /// <summary>
        /// Gets or sets the tip opisne ocene identifier.
        /// </summary>
        /// <value>
        /// The tip opisne ocene identifier.
        /// </value>
        public int TipOpisneOceneId { get; set; }

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