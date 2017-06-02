using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Grad
    {
        /// <summary>
        /// Gets or sets the Grad identifier
        /// Primary key u bazi.
        /// </summary>
        /// <value>
        /// The grad identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Sluzi za cuvanje naziv grada.
        /// </summary>
        /// <value>
        /// string
        /// </value>
        public string Naziv { get; set; }
    }
}