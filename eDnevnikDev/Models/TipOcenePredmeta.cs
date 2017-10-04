using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class TipOcenePredmeta
    {
        public TipOcenePredmeta()
        {
            Predmeti = new HashSet<Predmet>();
        }

        /// <summary>
        /// Gets or sets the tip ocene predmeta identifier.
        /// </summary>
        /// <value>
        /// The tip ocene predmeta identifier.
        /// </value>
        public int TipOcenePredmetaId { get; set; }

        /// <summary>
        /// Gets or sets the tip.
        /// </summary>
        /// <value>
        /// Tip ocene može da bude opisna, brojčana..
        /// </value>
        public string Tip { get; set; }

        public virtual ICollection<Predmet> Predmeti { get; set; }
    }
}