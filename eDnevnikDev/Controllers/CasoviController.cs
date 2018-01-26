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
        public static int[] predmetiID;

        public CasoviController()
        {

        }

        public CasoviController(ApplicationDbContext db)
        {
            _context = db;
        }

        // GET: Casovi/Create
        public ActionResult Create()
        {
            ViewBag.OdeljenjeId = new SelectList(_context.Odeljenja, "Id", "Id");
            ViewBag.PredmetId = new SelectList(_context.Predmeti, "PredmetID", "NazivPredmeta");
            ViewBag.ProfesorId = new SelectList(_context.Profesori, "ProfesorID", "Ime");
            return View(new UpisCasaViewModel());
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
            if (ModelState.IsValid)
            {
                var skolskaGodina = _context.SkolskaGodine.SingleOrDefault(x => x.Aktuelna == true);
                if(skolskaGodina == null)
                {
                    casViewModel.Greska = true;
                    casViewModel.OpisGreske = "Školska godina nije u toku!";
                    return View(casViewModel);
                }

                var odeljenje = _context.Odeljenja
                    .SingleOrDefault(x => x.Razred == casViewModel.Razred && x.OznakaID == casViewModel.Odeljenje && x.StatusID == 3);

                string prof = User.Identity.GetUserId();
                var profesor = _context.Profesori
                    .SingleOrDefault(p => p.UserProfesorId == prof);

                Cas cas = new Cas();

                if (odeljenje != null && profesor!=null)
                {
                    var listaUcenika = odeljenje.Ucenici
                    .OrderBy(x => x.BrojUDnevniku)
                    .ToList();

                    cas.Naziv = casViewModel.Naziv;
                    cas.Opis = casViewModel.Opis;
                    cas.PredmetId = casViewModel.PredmetId;
                    cas.OdeljenjeId = odeljenje.Id;
                    cas.Odeljenje = odeljenje;
                    cas.RedniBrojCasa = casViewModel.RedniBrojCasa;
                    cas.RedniBrojPredmeta = casViewModel.RedniBrojPredmeta;

                    cas.Polugodiste = _context
                                        .SkolskaGodine
                                        .SingleOrDefault(x => x.SkolskaGodinaId == odeljenje.SkolskaGodinaId)
                                        .Polugodista
                                        .SingleOrDefault(x => x.PocetakPolugodista <= DateTime.Today && x.KrajPolugodista >= DateTime.Today)
                                        .TipPolugodista;

                    cas.Tromesecje = _context
                                        .SkolskaGodine
                                        .SingleOrDefault(x => x.SkolskaGodinaId == odeljenje.SkolskaGodinaId)
                                        .Polugodista
                                        .SingleOrDefault(x => x.PocetakPolugodista <= DateTime.Today && x.KrajPolugodista >= DateTime.Today)
                                        .Tromesecja
                                        .SingleOrDefault(x => x.PocetakTromesecja <= DateTime.Today && x.KrajTromesecja >= DateTime.Today)
                                        .TipTromesecja;


                    cas.ProfesorId = profesor.ProfesorID;
                    cas.Datum = DateTime.Today;

                    var profesorPredmeti = _context.Profesori
                          .SingleOrDefault(p => p.ProfesorID == profesor.ProfesorID)
                          .Predmeti.Select(x => x);

                    var odeljenjePredmeti = _context.Odeljenja
                                          .SingleOrDefault(o => o.Id == odeljenje.Id)
                                          .Predmeti.Select(x => x);

                    //Lista predmeta koje profesor predaje konkretnom odeljenju
                    IEnumerable<Predmet> listaPredmeta = null;

                    if (profesorPredmeti != null && odeljenjePredmeti != null)
                    {
                        listaPredmeta = profesorPredmeti.Intersect(odeljenjePredmeti);
                    }
                    else
                    {
                        listaPredmeta = new List<Predmet>();
                    }

                    predmetiID = new int[listaPredmeta.Count()];

                    for (int i = 0; i < listaPredmeta.Count(); i++)
                    {
                        predmetiID[i] = listaPredmeta.ElementAt(i).PredmetID;
                    }

                    var ucenici = new List<UcenikSaPrisustvomViewModel>();

                    var casovi = _context.Casovi
                        .Where(c => c.Datum == DateTime.Today && c.OdeljenjeId == odeljenje.Id)
                        .ToList();

                    if (casovi.Count>0)
                    {
                        int maxCas = casovi
                            .Select(x => x.RedniBrojCasa)
                            .Max();

                        var prethodniCas = casovi
                            .SingleOrDefault(x => x.RedniBrojCasa == maxCas);

                        foreach (var item in odeljenje.Ucenici)
                        {
                            var odsutan = item.Odsustva
                                .SingleOrDefault(x => x.UcenikId == item.UcenikID && x.CasId == prethodniCas.CasId);

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
                    else
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

                    CasUceniciViewModel model = new CasUceniciViewModel()
                    {
                        Cas = cas,
                        Ucenici = ucenici,
                        Predmeti = listaPredmeta.ToList(),
                        // PredmetiID = predmetiID
                    };

                    _context.Casovi.Add(cas);
                    _context.SaveChanges();

                    return View("Cas", model);
                    // return View("Evidencija", model);

                }
                
                ViewBag.OdeljenjeId = new SelectList(_context.Odeljenja, "Id", "Id", cas.OdeljenjeId);
                ViewBag.PredmetId = new SelectList(_context.Predmeti, "PredmetID", "NazivPredmeta", cas.PredmetId);
                ViewBag.ProfesorId = new SelectList(_context.Profesori, "ProfesorID", "Ime", cas.ProfesorId);
                return View(casViewModel);
            }
            casViewModel.Greska = true;
            return View(casViewModel);
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
                    .SingleOrDefault(p => p.UserProfesorId == user)
                    .Predmeti
                    .ToList();

                Odeljenje odabranoOdeljenje = _context.Odeljenja
                    .SingleOrDefault(x => x.Razred == razred && x.OznakaID == odeljenje && x.StatusID == 3);

                var listaPredmetaOdeljenje = odabranoOdeljenje.Predmeti.ToList();

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
            Odeljenje odabranoOdeljenje = _context.Odeljenja
                .SingleOrDefault(x => x.Razred == razred && x.OznakaID == odeljenje && x.StatusID == 3);

            if (odabranoOdeljenje != null)
            {
                var predmet = odabranoOdeljenje.Predmeti
                .SingleOrDefault(x => x.PredmetID == predmetId);

                if (predmet != null)
                {
                    int redniBrojPredmeta;

                    redniBrojPredmeta = _context.Casovi
                        .Where(p => p.Odeljenje.Id == odabranoOdeljenje.Id && p.Predmet.PredmetID == predmetId)
                        .OrderBy(p => p.RedniBrojPredmeta)
                        .Select(p => p.RedniBrojPredmeta)
                        .Max();

                    return Json(redniBrojPredmeta + 1, JsonRequestBehavior.AllowGet);
                }
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
            DateTime datum = DateTime.Today;

            Odeljenje odabranoOdeljenje = _context.Odeljenja
                .SingleOrDefault(x => x.Razred == razred && x.OznakaID == odeljenje && x.StatusID == 3);

            if (odabranoOdeljenje != null)
            {
                Cas cas = _context.Casovi
                .SingleOrDefault(x => x.OdeljenjeId == odabranoOdeljenje.Id && x.Datum == datum && x.RedniBrojCasa == redniBrojCasa);

                if (cas == null)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }

                return Json(0, JsonRequestBehavior.AllowGet);
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
            Odeljenje odabranoOdeljenje = _context.Odeljenja
                .SingleOrDefault(x => x.Razred == razred && x.OznakaID == odeljenje && x.StatusID == 3);

            if (odabranoOdeljenje != null)
            {
                var predmet = odabranoOdeljenje.Predmeti
                    .SingleOrDefault(x => x.PredmetID == predmetId);

                var redniBrojPredmeta = _context.Casovi
                    .SingleOrDefault(x => x.RedniBrojPredmeta == rbPredmeta && x.Odeljenje.Id == odabranoOdeljenje.Id && x.Predmet.PredmetID == predmetId);

                if (redniBrojPredmeta == null)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }

                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(-1, JsonRequestBehavior.AllowGet);
        }

        //Aca Radi
        public JsonResult VratiOcene(int? odeljenjeId, int? profesorId, int? predmetId, int? ucenikId)
        {

            if (odeljenjeId != null && profesorId != null && predmetId != null)
            {

                var casoviId = _context.Casovi
                            .Where(c => c.ProfesorId == profesorId
                             && c.OdeljenjeId == odeljenjeId
                             && c.PredmetId == predmetId)
                             .Select(c => c.CasId);

                if (casoviId != null)
                {
                    var DTOocene = new List<DTOOcena>();

                    foreach (var c in casoviId)
                    {
                        var ocene = _context.Ocene
                                  .Where(o => o.CasId == c && o.UcenikId == ucenikId)
                                  .Select(o => o);

                        if (ocene != null)
                        {
                            foreach (var o in ocene)
                            {
                                var dtoOcena = new DTOOcena
                                {
                                    Ocena = o.Oznaka,
                                    Plus = o.Plus,
                                    TipOcene = o.TipOcene.Tip,
                                    TipOcenePredmeta = o.Cas.Predmet.TipOcenePredmeta.Tip,
                                    Komentar = o.Napomena,
                                    Polugodiste = o.Cas.Polugodiste,
                                    Tromesecje = o.Cas.Tromesecje
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
                    return Json(DTOocene, JsonRequestBehavior.AllowGet);
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
    }
}
