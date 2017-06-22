using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDnevnikDev.Controllers
{
    public class OdeljenjeController : Controller
    {
        // GET: Odeljenje
        /// <summary>
        /// Index kontrola
        /// </summary>
        /// <returns>Vraca View za Index</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}