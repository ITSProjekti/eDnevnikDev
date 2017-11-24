using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class AdminIndexViewModel
    {
        public int Decaci { get; set; }

        public int Devojcice { get; set; }

        public String PocetakPrvogPolugodista { get; set; }

        public String KrajPrvogPolugodista { get; set; }

        public String PocetakDrugogPolugodista { get; set; }

        public String KrajDrugogPolugodista { get; set; }

        public int UkupanBrojUcenika { get; set; }
    }
}