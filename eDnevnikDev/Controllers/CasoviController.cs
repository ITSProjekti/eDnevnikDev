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
using eDnevnikDev.DTOs;
using eDnevnikDev.ViewModel;
using eDnevnikDev.Helpers;

namespace eDnevnikDev.Controllers
{
    public class CasoviController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        private static int[] predmetiID;
        private static int? profesorId;
        private static int? odeljenjeId;


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
        /// <summary>
        /// Kontoler služi za kreiranje casa
        /// </summary>
        /// <param name="cas">The cas.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UpisCasaViewModel casViewModel)
        {
            var odeljenje = _context.Odeljenja
                .SingleOrDefault(x => x.Razred == casViewModel.Razred && x.OznakaID == casViewModel.Odeljenje && x.StatusID == 3);

            string prof = User.Identity.GetUserId();

            var profesor = _context.Profesori
                .SingleOrDefault(p => p.UserProfesorId == prof);

            var listaUcenika = odeljenje.Ucenici
                .OrderBy(x => x.BrojUDnevniku)
                .ToList();

            // Kreiranje casa casa sa hardcode-ovanim polugodistem i tromesecjem
            Cas cas = new Cas();
            cas.Naziv = casViewModel.Naziv;
            cas.Opis = casViewModel.Opis;
            cas.PredmetId = casViewModel.PredmetId;
            cas.OdeljenjeId = odeljenje.Id;
            cas.RedniBrojCasa = casViewModel.RedniBrojCasa;
            cas.RedniBrojPredmeta = casViewModel.RedniBrojPredmeta;
            cas.Polugodiste = 1;
            cas.Tromesecje = 1;
            cas.ProfesorId = profesor.ProfesorID;
            cas.Datum = DateTime.Today;

            //Setovanje static promenljivih
            profesorId = cas.ProfesorId;
            odeljenjeId = cas.OdeljenjeId;

            //U slucaju da je potrebno vratiti sve predmete koje profesor predaje odeljenju
            //var profesorPredmeti = _context.Profesori
            //          .SingleOrDefault(p => p.ProfesorID == profesor.ProfesorID)
            //          .Predmeti.Select(x => x);

            //var odeljenjePredmeti = _context.Odeljenja
            //                      .SingleOrDefault(o => o.Id == odeljenje.Id)
            //                      .Predmeti.Select(x => x);


            //Lista predmeta koje profesor predaje konkretnom odeljenju
            //IEnumerable<Predmet> listaPredmeta = null;

            //if (profesorPredmeti != null && odeljenjePredmeti != null)
            //{

            //    listaPredmeta = profesorPredmeti.Intersect(odeljenjePredmeti);
            //}
            //else
            //{
            //    listaPredmeta = new List<Predmet>();
            //}

            //Vracanje predmeta koji je upisan na casu
            var predmet = _context.Predmeti.SingleOrDefault(p => p.PredmetID == cas.PredmetId);

            List<Predmet> listaPredmeta = new List<Predmet>();

            if(predmet!=null)
            {
                listaPredmeta.Add(predmet);
            }
        
            predmetiID = new int[listaPredmeta.Count()];

            for (int i = 0; i < listaPredmeta.Count(); i++)
            {
                predmetiID[i] = listaPredmeta.ElementAt(i).PredmetID;
            }

            if (ModelState.IsValid)
            {
                _context.Casovi.Add(cas);
                _context.SaveChanges();

                CasUceniciViewModel model = new CasUceniciViewModel()
                {
                    Cas = cas,
                    Ucenici = listaUcenika,
                    Predmeti = listaPredmeta

                };

                return View("Cas", model);
                // return View("Evidencija", model);

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

        /// <summary>
        /// Kontroler vraca listu predmeta
        /// Prvo kupi sve predmete koje ulogovan profesor predaje u školi
        /// Zatim kupi sve predmete koje odredeno odeljenje ima
        /// Vrši se presek te dve liste i vracaju se predmeti koje profesor može da predaje odeljenju
        /// samo ako to odeljenje ima taj predmet u rasporedu i ako taj profesor predaje taj predmet u školi
        /// </summary>
        /// <param name="razred">The razred.</param>
        /// <param name="odeljenje">The odeljenje.</param>
        /// <returns></returns>
        public JsonResult VratiPredmete(int razred, int odeljenje)
        {
            string user = User.Identity.GetUserId();
            try
            {
                var listaPredmetaProf = _context.Profesori
                .Single(p => p.UserProfesorId == user)
                .Predmeti
                .Select(p => p);

                Odeljenje odabranoOdeljenje = _context.Odeljenja.SingleOrDefault(x => x.Razred == razred && x.OznakaID == odeljenje && x.StatusID == 3);

                var listaPredmetaOdeljenje = odabranoOdeljenje.Predmeti;
                if (odabranoOdeljenje != null)
                {
                    var listaPredmeta = listaPredmetaProf.Intersect(listaPredmetaOdeljenje)
                        .Select(p => new DTOPredmetCas() { PredmetID = p.PredmetID, NazivPredmeta = p.NazivPredmeta });
                    return Json(listaPredmeta.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Json(new List<DTOPredmetCas>(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Vratis the redni broj predmeta.
        /// </summary>
        /// <param name="razred">The razred.</param>
        /// <param name="odeljenje">The odeljenje.</param>
        /// <param name="predmetId">The predmet identifier.</param>
        /// <returns></returns>
        public JsonResult VratiRedniBrojPredmeta(int razred, int odeljenje, int predmetId)
        {
            try
            {
                Odeljenje odabranoOdeljenje = _context.Odeljenja.SingleOrDefault(x => x.Razred == razred && x.OznakaID == odeljenje && x.StatusID == 3);

                var predmet = odabranoOdeljenje.Predmeti
                    .SingleOrDefault(x => x.PredmetID == predmetId);
                int redniBrojPredmeta;
                try
                {
                    redniBrojPredmeta = _context.Casovi
                    .Where(d => d.Odeljenje.Id == odabranoOdeljenje.Id && d.Predmet.PredmetID == predmetId)
                    .OrderBy(x => x.RedniBrojPredmeta)
                    .Select(r => r.RedniBrojPredmeta)
                    .Max();
                }
                catch (Exception)
                {
                    redniBrojPredmeta = 0;
                }

                return Json(redniBrojPredmeta + 1, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Provera postojanja rednog broja casa, da ne bi
        /// kreirao cas sa rednim brojem nekog vec kreiranog casa
        /// </summary>
        /// <param name="razred">The razred.</param>
        /// <param name="odeljenje">The odeljenje.</param>
        /// <param name="redniBrojCasa">The redni broj casa.</param>
        /// <returns></returns>
        public JsonResult ProveraPostojanjaRednogBrojaCasa(int razred, int odeljenje, int redniBrojCasa)
        {
            try
            {
                DateTime datum = DateTime.Today;

                Odeljenje odabranoOdeljenje = _context.Odeljenja.SingleOrDefault(x => x.Razred == razred && x.OznakaID == odeljenje && x.StatusID == 3);

                Cas cas = new Cas();

                cas = _context.Casovi.Where(x => x.OdeljenjeId == odabranoOdeljenje.Id && x.Datum == datum)
                    .SingleOrDefault(x => x.RedniBrojCasa == redniBrojCasa);
                if (cas == null)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }


                return Json(0, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Proverava postojanje rednog broja predmeta, da ne bi 
        /// kreirao predmet sa rednim brojem nekog vec kreiranog predmeta
        /// </summary>
        /// <param name="razred">The razred.</param>
        /// <param name="odeljenje">The odeljenje.</param>
        /// <param name="predmetId">The predmet identifier.</param>
        /// <param name="rbPredmeta">The rb predmeta.</param>
        /// <returns></returns>
        public JsonResult ProveraPostojanjaRednogBrojaPredmeta(int razred, int odeljenje, int predmetId, int rbPredmeta)
        {
            try
            {
                Odeljenje odabranoOdeljenje = _context.Odeljenja.SingleOrDefault(x => x.Razred == razred && x.OznakaID == odeljenje && x.StatusID == 3);

                var predmet = odabranoOdeljenje.Predmeti
                    .SingleOrDefault(x => x.PredmetID == predmetId);

                var redniBrojPredmeta = _context.Casovi
                .Where(d => d.Odeljenje.Id == odabranoOdeljenje.Id && d.Predmet.PredmetID == predmetId)
                .OrderBy(x => x.RedniBrojPredmeta)
                .SingleOrDefault(x => x.RedniBrojPredmeta == rbPredmeta);

                if (redniBrojPredmeta == null)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }

                return Json(0, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }



        //Aca Radi
        public JsonResult VratiOcene()
        {

            if (odeljenjeId != null && profesorId != null && predmetiID != null)
            {
                List<int> nizCasoviID=new List<int>();

                foreach (var predmetID in predmetiID)
                {
                    var casoviID = _context.Casovi
                            .Where(c => c.ProfesorId == profesorId
                             && c.OdeljenjeId == odeljenjeId
                             && c.PredmetId == predmetID)
                             .Select(c => c.CasId);

                    foreach (var casID in casoviID)
                    {
                        nizCasoviID.Add(casID);
                    }
                }

                

                if (nizCasoviID != null)
                {
                    var DTOocene = new List<DTOOcena>();

                    foreach (var c in nizCasoviID)
                    {
                        var ocene = _context.Ocene
                                  .Where(o => o.CasId == c)
                                  .Select(o => o);

                        if (ocene != null)
                        {
                            foreach (var o in ocene)
                            {
                                var dtoOcena = new DTOOcena
                                {
                                    OcenaId=o.OcenaId,
                                    Ocena = o.Oznaka,
                                    Plus = o.Plus,
                                    TipOcene = o.TipOcene.Tip,
                                    TipOcenePredmeta = o.Cas.Predmet.TipOcenePredmeta.Tip,
                                    Komentar = o.Napomena,
                                    Polugodiste = o.Cas.Polugodiste,
                                    Tromesecje = o.Cas.Tromesecje,
                                    UcenikId=o.UcenikId,
                                    PredmetId=o.Cas.PredmetId
                                };

                                if (o.TipOpisneOceneId != null)
                                {
                                    var tipOpisneOcene = _context.TipoviOpisnihOcena
                                        .Where(x => x.TipOpisneOceneId == o.TipOpisneOceneId)
                                        .Select(x => x).ToList();

                                    dtoOcena.TipOpisneOcene = tipOpisneOcene.ElementAt(0).Tip;
                                }

                                DTOocene.Add(dtoOcena);
                            }

                        }


                    }

                    return Json(DTOocene.OrderBy(o=>o.OcenaId), JsonRequestBehavior.AllowGet);

                }

                return Json(new DTOOcena(), JsonRequestBehavior.AllowGet);

            }


            return Json(new DTOOcena(), JsonRequestBehavior.AllowGet);

        }


        public JsonResult VratiPredmeteID()
        {
            if (predmetiID != null && predmetiID.Count() > 0)
            {
                var dtoPredmetID = new DTOPredmetID
                {
                    PredmetiID = predmetiID
                };

                return Json(dtoPredmetID, JsonRequestBehavior.AllowGet);
            }

            return Json(new DTOPredmetID(), JsonRequestBehavior.AllowGet);

        }

        public JsonResult VratiTipOcene()
        {
            var tipoviOcena = _context.TipoviOcena;

            if(tipoviOcena!=null)
            {
                List<DTOTipOcene> dtoTipoviOcena = new List<DTOTipOcene>();

                foreach (var tipOcene in tipoviOcena)
                {
                    dtoTipoviOcena.Add(new DTOTipOcene
                    {
                        TipOceneId = tipOcene.TipOceneId,
                        Tip = tipOcene.Tip
                    });
                }

                return Json(dtoTipoviOcena, JsonRequestBehavior.AllowGet);
            }

                return Json(new DTOTipOcene(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult VratiTipOpisneOcene()
        {
            var tipoviOpisnihOcena = _context.TipoviOpisnihOcena;

            if (tipoviOpisnihOcena != null)
            {
                List<DTOTipOpisneOcene> dtoTipoviOpisnihOcena = new List<DTOTipOpisneOcene>();

                foreach (var tipOpisneOcene in tipoviOpisnihOcena)
                {
                    dtoTipoviOpisnihOcena.Add(new DTOTipOpisneOcene
                    {
                        TipOpisneOceneId = tipOpisneOcene.TipOpisneOceneId,
                        Tip = tipOpisneOcene.Tip
                    });
                }

                return Json(dtoTipoviOpisnihOcena, JsonRequestBehavior.AllowGet);
            }

            return Json(new DTOTipOpisneOcene(), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateHeaderAntiForgeryToken]
        public void DodajOcenu(DTOOcenaUnos dtoOcenaUnos)
        {
            if (dtoOcenaUnos != null)
            {
                var ocena = new Ocena
                {
                  Oznaka=dtoOcenaUnos.Oznaka,
                  Plus=dtoOcenaUnos.Plus,
                  UcenikId=dtoOcenaUnos.UcenikId,
                  CasId=dtoOcenaUnos.CasId,
                  TipOceneId=dtoOcenaUnos.TipOceneId,
                  TipOpisneOceneId=dtoOcenaUnos.TipOpisneOceneId,
                  Napomena=dtoOcenaUnos.Napomena
                };

                _context.Ocene.Add(ocena);
                _context.SaveChanges();
            }
        }



    }
}
