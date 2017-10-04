using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using eDnevnikDev.DTOs;
using System.Data.Entity.Migrations;

namespace eDnevnikDev.Controllers
{
    public class NapomeneController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Napomene
        public ActionResult Index()
        {
            var napomene = _context.Napomene.Include(n => n.Cas).Include(n => n.Profesor).Include(n => n.Ucenik);
            return View(napomene.ToList());
        }

        // GET: Napomene/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Napomena napomena = _context.Napomene.Find(id);
            if (napomena == null)
            {
                return HttpNotFound();
            }
            return View(napomena);
        }

        // GET: Napomene/Create
        public ActionResult Create()
        {
            ViewBag.CasId = new SelectList(_context.Casovi, "CasId", "Opis");
            ViewBag.ProfesorId = new SelectList(_context.Profesori, "ProfesorID", "Ime");
            ViewBag.UcenikId = new SelectList(_context.Ucenici, "UcenikID", "ImeOca");
            return View();
        }

        // POST: Napomene/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(DTONapomena dtoNapomena)
        {
            Napomena napomena = new Napomena();
            

            if (ModelState.IsValid)
            {
                napomena.Opis = dtoNapomena.Opis;
                napomena.UcenikId = dtoNapomena.UcenikId;
                napomena.ProfesorId = dtoNapomena.ProfesorId;
                napomena.CasId = dtoNapomena.CasId;

                _context.Napomene.Add(napomena);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CasId = new SelectList(_context.Casovi, "CasId", "Opis", napomena.CasId);
            ViewBag.ProfesorId = new SelectList(_context.Profesori, "ProfesorID", "Ime", napomena.ProfesorId);
            ViewBag.UcenikId = new SelectList(_context.Ucenici, "UcenikID", "ImeOca", napomena.UcenikId);
            return View(napomena);
        }

        // GET: Napomene/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Napomena napomena = _context.Napomene.Find(id);
            if (napomena == null)
            {
                return HttpNotFound();
            }
            ViewBag.CasId = new SelectList(_context.Casovi, "CasId", "Opis", napomena.CasId);
            ViewBag.ProfesorId = new SelectList(_context.Profesori, "ProfesorID", "Ime", napomena.ProfesorId);
            ViewBag.UcenikId = new SelectList(_context.Ucenici, "UcenikID", "ImeOca", napomena.UcenikId);
            return View(napomena);
        }

        // POST: Napomene/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public void Edit(DTONapomena dtoNapomena)
        {
            if(dtoNapomena!=null)
            {
                var napomena = new Napomena
                {
                    NapomenaId = dtoNapomena.NapomenaId,
                    Opis = dtoNapomena.Opis,
                    UcenikId = dtoNapomena.UcenikId,
                    ProfesorId = dtoNapomena.ProfesorId,
                    CasId = dtoNapomena.CasId
                };

                _context.Napomene.AddOrUpdate(napomena);
                _context.SaveChanges();
            }

        }

        // GET: Napomene/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Napomena napomena = _context.Napomene.Find(id);
            if (napomena == null)
            {
                return HttpNotFound();
            }
            return View(napomena);
        }

        // POST: Napomene/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Napomena napomena = _context.Napomene.Find(id);
            _context.Napomene.Remove(napomena);
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

        public JsonResult VratiNapomenu(int ucenikId, int casId)
        {
            var napomena = _context.Napomene
                .Where(n => n.UcenikId == ucenikId)
                .SingleOrDefault(n => n.CasId == casId);

            DTONapomena dtoNapomena = new DTONapomena {
                NapomenaId = napomena.NapomenaId,
                Opis = napomena.Opis,
                ProfesorId = napomena.ProfesorId,
                UcenikId = napomena.UcenikId,
                CasId=napomena.CasId
            };

            return Json(dtoNapomena, JsonRequestBehavior.AllowGet);
        }
    }
}
