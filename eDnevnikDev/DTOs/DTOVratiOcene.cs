using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.DTOs
{
    public class DTOVratiOcene
    {
        public int ocenaId;
        public int? oznaka;
        public bool? plus;
        public string tipOpisneOcene;
        public int? tipOpisneOceneId;
        public string tipOcene;
        public int? tipOceneId;
        public string napomena;
        public int polugodiste;
        public int tromesecje;
    }
}