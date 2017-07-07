using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    /// <summary>
    /// Arhiva Kreiranja Odeljenja
    /// </summary>
    public class ArhivaKreiranjaOdeljenja
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the razred.
        /// </summary>
        /// <value>
        /// The razred.
        /// </value>
        public byte Razred { get; set; }
        /// <summary>
        /// Gets or sets my property.
        /// </summary>
        /// <value>
        /// My property.
        /// </value>
        public int MyProperty { get; set; }

        /// <summary>
        /// Gets or sets the odeljenje identifier.
        /// </summary>
        /// <value>
        /// The odeljenje identifier.
        /// </value>
        public int OdeljenjeId{ get; set; }
        /// <summary>
        /// Gets or sets the odeljenje.
        /// </summary>
        /// <value>
        /// The odeljenje.
        /// </value>
        public virtual Odeljenje Odeljenje { get; set; }

        /// <summary>
        /// Gets or sets the vreme promene statusa.
        /// </summary>
        /// <value>
        /// The vreme promene statusa.
        /// </value>
        public DateTime VremePromeneStatusa{ get; set; }

        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        public int StatusId { get; set; }
        /// <summary>
        /// Status Odeljenja.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public virtual StatusOdeljenje Status { get; set; }

        //

    }
}