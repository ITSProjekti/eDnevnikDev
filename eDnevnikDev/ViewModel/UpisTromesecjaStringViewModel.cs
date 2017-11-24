using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class UpisTromesecjaStringViewModel
    {
        public UpisTromesecjaStringViewModel()
        {

        }

        public string PrvoPocetak { get; set; }

        public string PrvoKraj { get; set; }

        public string DrugoPocetak { get; set; }

        public string DrugoKraj { get; set; }

        public string TrecePocetak { get; set; }

        public string TreceKraj { get; set; }

        public string CetvrtoPocetak { get; set; }

        public string CetvrtoKraj { get; set; }

        public string Poruka { get; set; }

        public bool RokZaKreiranjeSkolskeGodine { get; set; } = false;
    }
}