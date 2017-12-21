using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class IzmenaPredmetaViewModel
    {
        public int PredmetId { get; set; }

        [Display(Name = "Naziv predmeta")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Polje za naziv je obavezno")]
        [RegularExpression(@"^([A-ZŠĐČĆŽa-zšđčćž0-9\s]+ ?)+$", ErrorMessage = "Nisu dozvoljeni specijalni karakteri")]
        public string NazivPredmeta { get; set; }

        [Display(Name = "Tip predmeta")]
        public int TipOcenePredmetaId { get; set; }
        public List<TipOcenePredmetaViewModel> TipoviOcenaPredmeta { get; set; }

        public bool Greska { get; set; }
    }
}