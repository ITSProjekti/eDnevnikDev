using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Oznaka
    {
        public Oznaka()
        {
            Smerovi = new HashSet<Smer>();
        }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OznakaId { get; set; }
        public ICollection<Smer> Smerovi { get; set; }
    }
}