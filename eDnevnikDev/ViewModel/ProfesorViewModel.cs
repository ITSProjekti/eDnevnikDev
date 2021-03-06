﻿using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    /// <summary>
    /// ViewModel Profesora
    /// </summary>
    public class ProfesorViewModel
    {
        /// <summary>
        /// Gets or sets the profesor.
        /// </summary>
        /// <value>
        /// The profesor.
        /// </value>
        public Profesor Profesor { get; set; }

        /// <summary>
        /// Gets or sets the predmeti i ds.
        /// </summary>
        /// <value>
        /// The predmeti i ds.List<int> PredmetiIDs Kolekcija
        /// </value>
        [Required(ErrorMessage ="Izaberite bar jedan predmet")]
        [Display(Name ="Predmeti")]
        public List<int> PredmetiIDs { get; set; }

        public HttpPostedFileBase File { get; set; }

        public bool Greska { get; set; }


        /// <summary>
        /// Gets or sets the predmeti.
        /// </summary>
        /// <value>
        /// The predmeti.IEnumerable<Predmet> Predmeti Kolekcijaa
        /// </value>
        public IEnumerable<Predmet> Predmeti { get; set; }

        public IEnumerable<Pol> Polovi { get; set; }


    }
}