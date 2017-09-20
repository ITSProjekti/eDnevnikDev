using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDnevnikDev.ViewModel
{
    public class UcenikRoleViewModel
    {
        public string UserUcenikId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public bool RolaUcenik { get; set; } = false;
        public bool RolaEditor { get; set; } = false;
    }
}