using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class ListaProfesoraViewModel
    {
        public List<Profesor> ListaProfesora { get; set; }
        public bool DodatProfesor { get; set; }
        public bool IzmenjenProfesor { get; set; }

    }
}