using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class OcenaViewModel
    {
        public int OcenaId { get; set; }
        public int? Ocena { get; set; }
        public bool? Plus { get; set; }
        public string TipOcene { get; set; }
        public string TipOcenePredmeta { get; set; }
        public string TipOpisneOcene { get; set; }
        public string Komentar { get; set; }
        public int Polugodiste { get; set; }
        public int Tromesecje { get; set; }
        public int UcenikId { get; set; }

    }
}