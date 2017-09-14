using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Odsustvo
    {
        public int OdsustvoId { get; set; }

        public int UcenikId { get; set; }

        [ForeignKey("UcenikId")]
        public virtual Ucenik Ucenik { get; set; }

        public int CasId { get; set; }

        [ForeignKey("CasId")]
        public virtual Cas Cas { get; set; }

    }
}