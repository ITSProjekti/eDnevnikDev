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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ocene
        public ActionResult Index()
        {
            var ocene = db.Ocene.Include(o => o.Cas).Include(o => o.TipOcene).Include(o => o.TipOpisneOcene).Include(o => o.Ucenik);
            return View(ocene.ToList());
        }

        // GET: Ocene/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocena ocena = db.Ocene.Find(id);
            if (ocena == null)
            {
                return HttpNotFound();
            }
            return View(ocena);
        }

        // GET: Ocene/Create
        public ActionResult Create()
        {
            ViewBag.CasId = new SelectList(db.Casovi, "CasId", "Opis");
            ViewBag.TipOceneId = new SelectList(db.TipoviOcena, "TipOceneId", "Tip");
            ViewBag.TipOpisneOceneId = new SelectList(db.TipoviOpisnihOcena, "TipOpisneOceneId", "Tip");
            ViewBag.UcenikId = new SelectList(db.Ucenici, "UcenikID", "ImeOca");
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
                db.Ocene.Add(ocena);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CasId = new SelectList(db.Casovi, "CasId", "Opis", ocena.CasId);
            ViewBag.TipOceneId = new SelectList(db.TipoviOcena, "TipOceneId", "Tip", ocena.TipOceneId);
            ViewBag.TipOpisneOceneId = new SelectList(db.TipoviOpisnihOcena, "TipOpisneOceneId", "Tip", ocena.TipOpisneOceneId);
            ViewBag.UcenikId = new SelectList(db.Ucenici, "UcenikID", "ImeOca", ocena.UcenikId);
            return View(ocena);
        }

        // GET: Ocene/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocena ocena = db.Ocene.Find(id);
            if (ocena == null)
            {
                return HttpNotFound();
            }
            ViewBag.CasId = new SelectList(db.Casovi, "CasId", "Opis", ocena.CasId);
            ViewBag.TipOceneId = new SelectList(db.TipoviOcena, "TipOceneId", "Tip", ocena.TipOceneId);
            ViewBag.TipOpisneOceneId = new SelectList(db.TipoviOpisnihOcena, "TipOpisneOceneId", "Tip", ocena.TipOpisneOceneId);
            ViewBag.UcenikId = new SelectList(db.Ucenici, "UcenikID", "ImeOca", ocena.UcenikId);
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
                db.Entry(ocena).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CasId = new SelectList(db.Casovi, "CasId", "Opis", ocena.CasId);
            ViewBag.TipOceneId = new SelectList(db.TipoviOcena, "TipOceneId", "Tip", ocena.TipOceneId);
            ViewBag.TipOpisneOceneId = new SelectList(db.TipoviOpisnihOcena, "TipOpisneOceneId", "Tip", ocena.TipOpisneOceneId);
            ViewBag.UcenikId = new SelectList(db.Ucenici, "UcenikID", "ImeOca", ocena.UcenikId);
            return View(ocena);
        }

        // GET: Ocene/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocena ocena = db.Ocene.Find(id);
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
            Ocena ocena = db.Ocene.Find(id);
            db.Ocene.Remove(ocena);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Predmeti()
        {
            var prof = db.Profesori.Single(p => p.UserProfesorId == User.Identity.GetUserId());
            IQueryable<Predmet> predmeti = db.Predmeti.Where(p => p.Profesori.Contains(prof));

            return View(predmeti.ToList());
        }
    }
}
