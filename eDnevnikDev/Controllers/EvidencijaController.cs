using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;

namespace eDnevnikDev.Controllers
{
    public class EvidencijaController : Controller
    {
        ApplicationDbContext _context;
        public EvidencijaController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Evidencija
        /// <summary>
        /// Menjace se controla/Nije zavrseno
        /// </summary>
        /// <returns>Vraca sve profesore</returns>
        public ActionResult Index()
        {            
            return View();
        }
    }
}