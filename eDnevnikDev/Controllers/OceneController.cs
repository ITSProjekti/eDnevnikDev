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
    public class OceneController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public OceneController()
        {
            _context = new ApplicationDbContext();
        }
        public OceneController(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        // GET: Ocene
        public ActionResult Index()
        {
            var ocene = _context.Ocene.Include(o => o.Cas).Include(o => o.TipOcene).Include(o => o.TipOpisneOcene).Include(o => o.Ucenik);
            return View(ocene.ToList());
        }

        // GET: Ocene/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocena ocena = _context.Ocene.Find(id);
            if (ocena == null)
            {
                return HttpNotFound();
            }
            return View(ocena);
        }

        // GET: Ocene/Create
        public ActionResult Create()
        {
            ViewBag.CasId = new SelectList(_context.Casovi, "CasId", "Opis");
            ViewBag.TipOceneId = new SelectList(_context.TipoviOcena, "TipOceneId", "Tip");
            ViewBag.TipOpisneOceneId = new SelectList(_context.TipoviOpisnihOcena, "TipOpisneOceneId", "Tip");
            ViewBag.UcenikId = new SelectList(_context.Ucenici, "UcenikID", "ImeOca");
            return View();
        }

        // POST: Ocene/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OcenaId,Oznaka,Plus,UcenikId,CasId,TipOceneId,TipOpisneOceneId,Napomena")] Ocena ocena)
        {
            if (ModelState.IsValid)
            {
                _context.Ocene.Add(ocena);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CasId = new SelectList(_context.Casovi, "CasId", "Opis", ocena.CasId);
            ViewBag.TipOceneId = new SelectList(_context.TipoviOcena, "TipOceneId", "Tip", ocena.TipOceneId);
            ViewBag.TipOpisneOceneId = new SelectList(_context.TipoviOpisnihOcena, "TipOpisneOceneId", "Tip", ocena.TipOpisneOceneId);
            ViewBag.UcenikId = new SelectList(_context.Ucenici, "UcenikID", "ImeOca", ocena.UcenikId);
            return View(ocena);
        }

        // GET: Ocene/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocena ocena = _context.Ocene.Find(id);
            if (ocena == null)
            {
                return HttpNotFound();
            }
            ViewBag.CasId = new SelectList(_context.Casovi, "CasId", "Opis", ocena.CasId);
            ViewBag.TipOceneId = new SelectList(_context.TipoviOcena, "TipOceneId", "Tip", ocena.TipOceneId);
            ViewBag.TipOpisneOceneId = new SelectList(_context.TipoviOpisnihOcena, "TipOpisneOceneId", "Tip", ocena.TipOpisneOceneId);
            ViewBag.UcenikId = new SelectList(_context.Ucenici, "UcenikID", "ImeOca", ocena.UcenikId);
            return View(ocena);
        }

        // POST: Ocene/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OcenaId,Oznaka,Plus,UcenikId,CasId,TipOceneId,TipOpisneOceneId,Napomena")] Ocena ocena)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(ocena).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CasId = new SelectList(_context.Casovi, "CasId", "Opis", ocena.CasId);
            ViewBag.TipOceneId = new SelectList(_context.TipoviOcena, "TipOceneId", "Tip", ocena.TipOceneId);
            ViewBag.TipOpisneOceneId = new SelectList(_context.TipoviOpisnihOcena, "TipOpisneOceneId", "Tip", ocena.TipOpisneOceneId);
            ViewBag.UcenikId = new SelectList(_context.Ucenici, "UcenikID", "ImeOca", ocena.UcenikId);
            return View(ocena);
        }

        // GET: Ocene/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocena ocena = _context.Ocene.Find(id);
            if (ocena == null)
            {
                return HttpNotFound();
            }
            return View(ocena);
        }

        // POST: Ocene/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ocena ocena = _context.Ocene.Find(id);
            _context.Ocene.Remove(ocena);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Vraca listu predmete koje profesor predaje, da bi profesor mogao da vidi ocene, da ih upise ili izmeni.
        /// TESTED. TEST_NAME= PredmetiTest_VracaPredmete()
        /// </summary>
        /// <returns>Listu predmeta</returns>
        public ActionResult Predmeti()
        {
            string user = User.Identity.GetUserId();
            var predmeti = _context.Profesori
                .Single(p => p.UserProfesorId == user)
                .Predmeti
                .Select(x=>x)
                .ToList();

            return View(predmeti);
        }
        
    }
}
