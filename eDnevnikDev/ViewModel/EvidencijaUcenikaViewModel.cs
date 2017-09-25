using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class EvidencijaUcenikaViewModel
    {
        public EvidencijaUcenikaViewModel()
        {

        }

        public EvidencijaUcenikaViewModel(int ucenikId, string ime, string prezime, string fotografija, int? broj, bool p)
        {
            this.UcenikId = ucenikId;
            this.Ime = ime;
            this.Prezime = prezime;
            this.Fotografija = fotografija;
            this.BrojUDnevniku = broj;
            this.Prisutan = p;
        }

        public int UcenikId { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public string Fotografija { get; set; }

        public int? BrojUDnevniku { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EvidencijaUcenikaViewModel"/> is prisutan.
        /// </summary>
        /// <value>
        ///   <c>true</c> if prisutan; otherwise, <c>false</c>.
        /// </value>
        public bool Prisutan { get; set; }
    }
}