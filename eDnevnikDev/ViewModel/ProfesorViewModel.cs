using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class ProfesorViewModel
    {
        public Profesor Profesor { get; set; }

        public List<int> PredmetiIDs { get; set; }

        public IEnumerable<Predmet> Predmeti { get; set; }


    }
}