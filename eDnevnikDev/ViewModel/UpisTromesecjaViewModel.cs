using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class UpisTromesecjaViewModel
    {
        public UpisTromesecjaViewModel()
        {
            
        }
        
        public DateTime PrvoPocetak { get; set; }

        public DateTime PrvoKraj { get; set; }

        public DateTime DrugoPocetak { get; set; }

        public DateTime DrugoKraj { get; set; }

        public DateTime TrecePocetak { get; set; }

        public DateTime TreceKraj { get; set; }

        public DateTime CetvrtoPocetak { get; set; }

        public DateTime CetvrtoKraj { get; set; }

        public string Poruka { get; set; }
    }
}