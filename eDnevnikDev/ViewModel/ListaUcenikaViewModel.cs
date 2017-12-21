using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class ListaUcenikaViewModel
    {
        public List<Ucenik> ListaUcenika { get; set; }
        public bool DodatUcenik { get; set; }
        public bool IzmenjenUcenik { get; set; }

    }
}