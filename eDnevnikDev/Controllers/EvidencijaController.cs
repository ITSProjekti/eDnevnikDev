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
        /// Vraca spisak svih ucenika po odeljenjima sortirane po azbucnom redu.Sve se renderuje na klijentskoj strani.
        /// NOT TESTED
        /// </summary>
        /// <returns>Spisak ucenika u odredjenom odeljenju</returns>
        public ActionResult Index()
        {            
            return View();
        }
    }
}