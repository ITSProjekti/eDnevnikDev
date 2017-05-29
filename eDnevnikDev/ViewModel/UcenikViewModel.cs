using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eDnevnikDev.Models;

namespace eDnevnikDev.ViewModel
{
    public class UcenikViewModel
    {
        public Ucenik  Ucenik { get; set; }
        public IEnumerable<Grad> Gradovi { get; set; }
    }
}