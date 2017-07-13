using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eDnevnikDev.Models;
using System.ComponentModel.DataAnnotations;

namespace eDnevnikDev.ViewModel
{
    /// <summary>
    /// ViewModel Ucenika
    /// </summary>
    public class UcenikViewModel
    {
        /// <summary>
        /// Gets or sets the ucenik.
        /// </summary>
        /// <value>
        /// The ucenik.
        /// </value>
        public Ucenik  Ucenik { get; set; }
        /// <summary>
        /// Gets or sets the gradovi.
        /// </summary>
        /// <value>
        /// The gradovi. IEnumerable<Grad> Kolekcija
        /// </value>
        public IEnumerable<Grad> Gradovi { get; set; }

        /// <summary>
        /// Gets or sets the smerovi.
        /// </summary>
        /// <value>
        /// The smerovi.IEnumerable<Smer> Kolekcija
        /// </value>
        public IEnumerable<Smer> Smerovi { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// HttpPostedFileBase
        /// </value>
        public HttpPostedFileBase File { get; set; }

        public int Oznaka { get; set; }

        public int SmestiUNovoOdeljenje { get; set; }
    }
}