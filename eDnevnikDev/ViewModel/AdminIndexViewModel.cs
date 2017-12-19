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

        public string PocetakPrvogPolugodista { get; set; }

        public string KrajPrvogPolugodista { get; set; }

        public string PocetakDrugogPolugodista { get; set; }

        public string KrajDrugogPolugodista { get; set; }

        public int UkupanBrojUcenika { get; set; }

        public string PocetakPrvogTromesecja { get; set; }

        public string KrajPrvogTromesecja { get; set; }

        public string PocetakDrugogTromesecja { get; set; }

        public string KrajDrugogTromesecja { get; set; }

        public string PocetakTrecegTromesecja { get; set; }

        public string KrajTrecegTromesecja { get; set; }

        public string PocetakCetvrtogTromesecja { get; set; }

        public string KrajCetvrtogTromesecja { get; set; }
    }
}