using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Models
{
    public class Napomena
    {
        public Napomena(string opis, int ucenikId, int profesorId, int casId)
        {
            this.Opis = opis;
            this.UcenikId = ucenikId;
            this.ProfesorId = profesorId;
            this.CasId = casId;
        }
        public Napomena()
        {

        }
        public int NapomenaId { get; set; }

        public string Opis { get; set; }

        public virtual Ucenik Ucenik { get; set; }

        [ForeignKey("Ucenik")]
        public int UcenikId { get; set; }

        public virtual Profesor Profesor { get; set; }
    
        [ForeignKey("Profesor")]
        public int ProfesorId { get; set; }

        public virtual Cas Cas { get; set; }

        [ForeignKey("Cas")]
        public int CasId { get; set; }
        

    }
}