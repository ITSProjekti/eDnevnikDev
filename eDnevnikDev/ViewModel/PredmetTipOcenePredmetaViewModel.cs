using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class PredmetTipOcenePredmetaViewModel
    {
        /// <summary>
        /// Gets or sets the predmeti.
        /// </summary>
        /// <value>
        /// The predmeti.
        /// </value>
        public List<Predmet> Predmeti { get; set; }

        /// <summary>
        /// Gets or sets the tipovi ocena predmeta.
        /// </summary>
        /// <value>
        /// The tipovi ocena predmeta.
        /// </value>
        public List<TipOcenePredmetaViewModel> TipoviOcenaPredmeta { get; set; }
    }
}