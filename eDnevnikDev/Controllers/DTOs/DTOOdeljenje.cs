using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.DTOs
{
    /// <summary>
    /// Sluzi za JSON 
    /// </summary>
    public class DTOOdeljenje
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the oznaka.
        /// </summary>
        /// <value>
        /// The oznaka.
        /// </value>
        public int Oznaka { get; set; }

    }
}