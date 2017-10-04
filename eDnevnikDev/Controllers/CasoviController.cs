using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using Microsoft.AspNet.Identity;

namespace eDnevnikDev.Controllers
{
    public class CasoviController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Casovi/Create
        public ActionResult Create()
        {
            ViewBag.OdeljenjeId = new SelectList(_context.Odeljenja, "Id", "Id");
            ViewBag.PredmetId = new SelectList(_context.Predmeti, "PredmetID", "NazivPredmeta");
            ViewBag.ProfesorId = new SelectList(_context.Profesori, "ProfesorID", "Ime");
            return View();
        }

        // POST: Casovi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CasId,Datum,Naziv,Opis,ProfesorId,PredmetId,OdeljenjeId,Polugodiste,Tromesecje,RedniBrojCasa,RedniBrojPredmeta")] Cas cas)
        {
            if (ModelState.IsValid)
            {
                _context.Casovi.Add(cas);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OdeljenjeId = new SelectList(_context.Odeljenja, "Id", "Id", cas.OdeljenjeId);
            ViewBag.PredmetId = new SelectList(_context.Predmeti, "PredmetID", "NazivPredmeta", cas.PredmetId);
            ViewBag.ProfesorId = new SelectList(_context.Profesori, "ProfesorID", "Ime", cas.ProfesorId);
            return View(cas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult VratiPredmete(int razred, int odeljenje)
        {
            string user = User.Identity.GetUserId();
            var listaPredmetaProf = _context.Profesori
                .Single(p => p.UserProfesorId == user)
                .Predmeti;
            Odeljenje odabranoOdeljenje = _context.Odeljenja.SingleOrDefault(x => x.Razred == razred && x.OznakaID == odeljenje && x.StatusID == 3);

            if(odabranoOdeljenje != null)
            {
                var listaPredmeta = listaPredmetaProf.Where(x => x.Odeljenja == odabranoOdeljenje).ToList();
                return Json(listaPredmeta, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new List<Predmet>(), JsonRequestBehavior.AllowGet);
            }

            
        }
    }
}
