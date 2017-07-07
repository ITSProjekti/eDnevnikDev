using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class StatusOdeljenje
    {
        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        public int StatusId { get; set; }
        /// <summary>
        /// Cuvanje Opisa Odeljenja
        /// </summary>
        /// <value>
        /// The opis.
        /// </value>
        public string Opis { get; set; }

    }
}