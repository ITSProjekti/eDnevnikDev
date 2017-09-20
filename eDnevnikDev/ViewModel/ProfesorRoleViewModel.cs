using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class ProfesorRoleViewModel
    {
        public string UserProfesorId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public bool RolaProfesor { get; set; } = false;
        public bool RolaEditor { get; set; } = false;






    }
}