using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDnevnikDev.Controllers
{
    public class PredmetiController : Controller
    {
        ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PredmetiController"/> class.
        /// </summary>
        public PredmetiController()
        {
            _context = new ApplicationDbContext();
        }
        public PredmetiController(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Profesori
        /// <summary>
        /// Ucitavamo Listu Predmeta iz Baze
        /// </summary>
        /// <returns>Vracamo View sa Listom Predmeta</returns>
        public ActionResult Index()
        {
            IEnumerable<Predmet> ListaPredmeta = _context.Predmeti.ToList();
            return View("Index",ListaPredmeta);
        }

        /// <summary>
        /// Ucitava View za dodavanje predmeta
        /// </summary>
        /// <returns>Vraca View  Dodaj</returns>
        public ActionResult Dodaj()
        {
            return View("Dodaj");
        }

        public ActionResult SacuvajPredmet(Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                _context.Predmeti.Add(predmet);
                _context.SaveChanges();
                return RedirectToAction("Index", "Predmeti");
                
            }
            else
            {
                return View("Dodaj", predmet);
            }
        }

    }
}