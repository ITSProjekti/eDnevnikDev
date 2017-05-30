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

        public PredmetiController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Profesori
        public ActionResult Index()
        {
            IEnumerable<Predmet> ListaPredmeta = _context.Predmeti.ToList();
            return View(ListaPredmeta);
        }

        public ActionResult Dodaj()
        {
            return View();
        }

    }
}