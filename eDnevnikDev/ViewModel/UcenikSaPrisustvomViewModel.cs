using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class UcenikSaPrisustvomViewModel
    {
        public int UcenikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Fotografija { get; set; }
        public int? BrojUDnevniku { get; set; }
        public bool Prisutan { get; set; }
    }
}