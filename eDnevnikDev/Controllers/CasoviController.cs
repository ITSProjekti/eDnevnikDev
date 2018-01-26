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
        private static CasUceniciViewModel casUceniciViewModel = null;
        private static int? odeljenjeId = null;
        private static int? profesorId = null;
        private static int? predmetId = null;

        public CasoviController()
        {

        }

        public CasoviController(ApplicationDbContext db)
        {
            _context = db;
        }

        public ActionResult Cas()
        {

            if (casUceniciViewModel != null)
            {
                if(odeljenjeId != null && profesorId != null && predmetId != null)
                {
                    casUceniciViewModel.listaOcena = VratiOcene((int)odeljenjeId, (int)profesorId, (int)predmetId);
                }

                return View(casUceniciViewModel);
            }

            return View(new CasUceniciViewModel());
        }

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
        /// Kontoler služi za kreiranje časa
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

            Cas cas = new Cas();
            cas.Naziv = casViewModel.Naziv;
            cas.Opis = casViewModel.Opis;
            cas.PredmetId = casViewModel.PredmetId;
            cas.OdeljenjeId = odeljenje.Id;
            cas.Odeljenje = _context.Odeljenja.Single(x => x.Id == odeljenje.Id);
            cas.RedniBrojCasa = casViewModel.RedniBrojCasa;
            cas.RedniBrojPredmeta = casViewModel.RedniBrojPredmeta;

            cas.Polugodiste = _context
                                .SkolskaGodine
                                .Single(x => x.SkolskaGodinaId == odeljenje.SkolskaGodinaId)
                                .Polugodista
                                .Single(x => x.PocetakPolugodista <= DateTime.Today && x.KrajPolugodista > DateTime.Today)
                                .TipPolugodista;

            cas.Tromesecje = _context
                                .SkolskaGodine
                                .Single(x => x.SkolskaGodinaId == odeljenje.SkolskaGodinaId)
                                .Polugodista
                                .Single(x => x.PocetakPolugodista <= DateTime.Today && x.KrajPolugodista > DateTime.Today)
                                .Tromesecja
                                .Single(x => x.PocetakTromesecja <= DateTime.Today && x.KrajTromesecja > DateTime.Today)
                                .TipTromesecja;


            cas.ProfesorId = profesor.ProfesorID;
            cas.Datum = DateTime.Today;



            var profesorPredmeti = _context.Profesori
                      .SingleOrDefault(p => p.ProfesorID == profesor.ProfesorID)
                      .Predmeti.Select(x => x);

            var odeljenjePredmeti = _context.Odeljenja
                                  .SingleOrDefault(o => o.Id == odeljenje.Id)
                                  .Predmeti.Select(x => x);

            if (ModelState.IsValid)
            {
                var ucenici = new List<UcenikSaPrisustvomViewModel>();

                try
                {
                    var casovi = _context.Casovi
                    .Where(c => c.Datum == DateTime.Today && c.OdeljenjeId == odeljenje.Id);

                    //casovi = casovi.Where()

                    int maxCas = casovi.Max(x => x.RedniBrojCasa);

                    var prethodniCas = casovi.SingleOrDefault(x => x.RedniBrojCasa == maxCas);

                    foreach (var item in odeljenje.Ucenici)
                    {
                        var odsutan = item.Odsustva.Where(x => x.CasId == prethodniCas.CasId)
                            .SingleOrDefault(x => x.UcenikId == item.UcenikID);

                        if (odsutan == null)
                        {
                            ucenici.Add(new UcenikSaPrisustvomViewModel
                            {
                                BrojUDnevniku = item.BrojUDnevniku,
                                Fotografija = item.Fotografija,
                                UcenikID = item.UcenikID,
                                Ime = item.Ime,
                                Prezime = item.Prezime,
                                Prisutan = true
                            });
                        }
                        else
                        {
                            ucenici.Add(new UcenikSaPrisustvomViewModel
                            {
                                BrojUDnevniku = item.BrojUDnevniku,
                                Fotografija = item.Fotografija,
                                UcenikID = item.UcenikID,
                                Ime = item.Ime,
                                Prezime = item.Prezime,
                                Prisutan = false
                            });
                        }
                    }
                    ucenici.OrderBy(x => x.BrojUDnevniku).ToList();
                }
                catch (Exception)
                {
                    foreach (var item in odeljenje.Ucenici)
                    {
                        ucenici.Add(new UcenikSaPrisustvomViewModel
                        {
                            BrojUDnevniku = item.BrojUDnevniku,
                            Fotografija = item.Fotografija,
                            UcenikID = item.UcenikID,
                            Ime = item.Ime,
                            Prezime = item.Prezime,
                            Prisutan = true
                        });
                    }

                }

                var predmet = _context.Predmeti.Where(p => p.PredmetID == casViewModel.PredmetId)
                    .Select(p => new PredmetCasViewModel
                    {
                        PredmetID = p.PredmetID,
                        NazivPredmeta = p.NazivPredmeta,
                        TipOcenePredmeta = p.TipOcenePredmeta.Tip
                    }).SingleOrDefault();

                casUceniciViewModel = new CasUceniciViewModel()
                {
                    Cas = cas,
                    Ucenici = ucenici,
                    Predmet = predmet,
                };

                odeljenjeId = cas.OdeljenjeId;
                profesorId = cas.ProfesorId;
                predmetId = cas.PredmetId;

                _context.Casovi.Add(cas);
                _context.SaveChanges();

                return RedirectToAction("Cas", "Casovi");

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
        /// Kontroler vraća listu predmeta
        /// Prvo kupi sve predmete koje ulogovan profesor predaje u školi
        /// Zatim kupi sve predmete koje određeno odeljenje ima
        /// Vrši se presek te dve liste i vraćaju se predmeti koje profesor može da predaje odeljenju
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



        /// <summary>
        /// Metoda vraca listu ocena na osnovu proslejdenih parametara
        /// <see cref="OcenaViewModel"/> 
        /// </summary>
        /// <param name="odeljenjeId"></param>
        /// <param name="profesorId"></param>
        /// <param name="predmetId"></param>
        /// <returns>Vraca listu ocena</returns>
        public List<OcenaViewModel> VratiOcene(int odeljenjeId, int profesorId, int predmetId)
        {
            var ocene = _context.Ocene.Where(o => o.Cas.ProfesorId == profesorId)
                                      .Where(o => o.Cas.PredmetId == predmetId)
                                      .Where(o => o.Cas.OdeljenjeId == odeljenjeId)
                                      .Select(o => o);

            if (ocene != null)
            {
                var listaTipovaOpisnihOcena = _context.TipoviOpisnihOcena.ToList();
                var listaOcena = new List<OcenaViewModel>();

                foreach (var ocena in ocene)
                {
                    var ocenaVM = new OcenaViewModel
                    {
                        OcenaId=ocena.OcenaId,
                        Ocena = ocena.Oznaka,
                        Plus = ocena.Plus,
                        TipOcene = ocena.TipOcene.Tip,
                        Komentar = ocena.Napomena,
                        Polugodiste = ocena.Cas.Polugodiste,
                        Tromesecje = ocena.Cas.Tromesecje,
                        UcenikId = ocena.UcenikId,
                        TipOcenePredmeta = ocena.Cas.Predmet.TipOcenePredmeta.Tip
                    };

                    if (ocena.TipOpisneOceneId != null)
                    {
                        var tipOpisneOcene = listaTipovaOpisnihOcena.SingleOrDefault(x => x.TipOpisneOceneId == ocena.TipOpisneOceneId);

                        if (tipOpisneOcene != null)
                        {
                            ocenaVM.TipOpisneOcene = tipOpisneOcene.Tip;
                        }

                    }

                    listaOcena.Add(ocenaVM);
                }

                return listaOcena;

            }

            return new List<OcenaViewModel>();
        }

        /// <summary>
        /// Metoda vraca tipove ocena
        /// <see cref="DTOTipOcene"/> 
        /// </summary>
        /// <returns></returns>
        public JsonResult VratiTipOcene()
        {
            var listaTipovaOcena = _context.TipoviOcena.Select(x => new DTOTipOcene
            {
                TipOceneId = x.TipOceneId,
                Tip = x.Tip
            })
            .ToList();

            return Json(listaTipovaOcena, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Metoda vraca tipove  opisne ocene
        /// <see cref="DTOTipOpisneOcene"/> 
        /// </summary>
        /// <returns></returns>
        public JsonResult VratiTipOpisneOcene()
        {
            var listaTipovaOpisnihOcena = _context.TipoviOpisnihOcena.Select(x => new DTOTipOpisneOcene
            {
                TipOpisneOceneId = x.TipOpisneOceneId,
                Tip = x.Tip
            });

            return Json(listaTipovaOpisnihOcena);
        }

        /// <summary>
        /// Metoda vrsi upis prosledjene ocene i vraca upisanu ocenu na view
        /// <see cref="DTOOcenaUnos"/> 
        /// <see cref="DTOOcena"/> 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateHeaderAntiForgeryToken]
        public JsonResult UpisiOcenu(DTOOcenaUnos dtoOcenaUnos)
        {
            if(ModelState.IsValid)
            {
                var ocena = new Ocena
                {
                    Oznaka = dtoOcenaUnos.Oznaka,
                    Plus = dtoOcenaUnos.Plus,
                    UcenikId = dtoOcenaUnos.UcenikId,
                    CasId = dtoOcenaUnos.CasId,
                    TipOceneId = dtoOcenaUnos.TipOceneId,
                    TipOpisneOceneId = dtoOcenaUnos.TipOpisneOceneId,
                    Napomena = dtoOcenaUnos.Napomena
                };

                _context.Ocene.Add(ocena);
                _context.SaveChanges();

                var dtoOcena = _context.Ocene.Where(o => o.OcenaId == ocena.OcenaId)
                                             .Select(o=> new DTOOcena
                                             {
                                                 OcenaId=o.OcenaId,
                                                 Ocena=o.Oznaka,
                                                 Plus=o.Plus,
                                                 TipOcene=o.TipOcene.Tip,
                                                 TipOcenePredmeta=o.Cas.Predmet.TipOcenePredmeta.Tip,
                                                 TipOpisneOcene=o.TipOpisneOcene.Tip,
                                                 Komentar=o.Napomena,
                                                 Polugodiste=o.Cas.Polugodiste,
                                                 Tromesecje=o.Cas.Tromesecje,
                                                 UcenikId=o.UcenikId
                                             })
                                             .SingleOrDefault();

                return Json(dtoOcena, JsonRequestBehavior.AllowGet);
            }

            return Json(new DTOOcena(), JsonRequestBehavior.AllowGet);
        }

    }
}
