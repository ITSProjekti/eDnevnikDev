using eDnevnikDev.DTOs;
using eDnevnikDev.Helpers;
using eDnevnikDev.Models;
using eDnevnikDev.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
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
        /// Ucitavamo Listu Predmeta iz Baze. Test name=PredmetController_Index
        /// </summary>
        /// <returns>Vracamo View sa Listom Predmeta</returns>
        [Authorize(Roles = "Administrator, Editor")]
        public ActionResult Index(bool? dodatPredmet, bool? izmenjenPredmet)
        {
            if (dodatPredmet != null)
            {
                var model = new ListaPredmetaViewModel
                {
                    ListaPredmeta = _context.Predmeti.ToList(),
                    DodatPredmet = (bool)dodatPredmet
                };

                return View(model);
            }

            if (izmenjenPredmet != null)
            {
                var model = new ListaPredmetaViewModel
                {
                    ListaPredmeta = _context.Predmeti.ToList(),
                    IzmenjenPredmet = (bool)izmenjenPredmet
                };

                return View(model);
            }

            return View(new ListaPredmetaViewModel
            {
                ListaPredmeta = _context.Predmeti.ToList()
            });
        }

        /// <summary>
        /// Ucitava View za dodavanje predmeta. Test name=PredmetController_Dodaj
        /// </summary>
        /// <returns>Vraca View  Dodaj</returns>
        [Authorize(Roles = "Administrator, Editor")]
        public ActionResult Dodaj()
        {
            return View(new PredmetViewModel());
        }

        /// <summary>
        /// Cuva predmet sa sve tipom ocene koji mu se dodeljuje. Test name=PredmetController_SacuvajPredmet
        /// <see cref="Predmet"/>
        /// </summary>
        /// <param name="predmet">The predmet.</param>
        /// <returns>Vraca nas na index stranu Predmeta</returns>
        [Authorize(Roles = "Administrator, Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SacuvajPredmet(PredmetViewModel predmetViewModel)
        {
            //proverava se da li predmet vec postoji u bazi
            if (_context.Predmeti.Where(p => p.NazivPredmeta == predmetViewModel.Predmet.NazivPredmeta).Any())
                ModelState.AddModelError("Predmet.NazivPredmeta", "Predmet već postoji!");

            if (ModelState.IsValid)
            {
                try
                {
                    Predmet predmet = new Predmet();
                    predmet.NazivPredmeta = predmetViewModel.Predmet.NazivPredmeta;
                    predmet.TipOcenePredmetaId = predmetViewModel.Predmet.TipOcenePredmetaId;

                    _context.Predmeti.Add(predmet);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Predmeti", new { dodatPredmet = true });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                return View("Dodaj", new PredmetViewModel { Predmet = predmetViewModel.Predmet, Greska = true });
            }
            else
            {
                return View("Dodaj", new PredmetViewModel { Predmet = predmetViewModel.Predmet, Greska = true });
            }

        }


        /// <summary>
        /// Menja se tip ocene za određeni predmet
        /// Npr ako matematika ima tip ocene "opisna" moguća je izmena da bude "numerička"
        /// </summary>
        /// <param name="predmetId">The predmet identifier.</param>
        /// <param name="tipOcenePredmetaId">The tip ocene predmeta identifier.</param>
        [HttpPost]
        [ValidateHeaderAntiForgeryToken]
        private void IzmenaTipaOcenaUPredmetu(int predmetId, int tipOcenePredmetaId)
        {
            var predmet = _context.Predmeti
                .SingleOrDefault(p => p.PredmetID == predmetId);

            var tip = _context.TipoviOcenaPredmeta.SingleOrDefault(x => x.TipOcenePredmetaId == tipOcenePredmetaId);

            if (predmet != null && tip != null)
            {
                predmet.TipOcenePredmetaId = tipOcenePredmetaId;
                predmet.TipOcenePredmeta = tip;
                _context.Predmeti.AddOrUpdate(predmet);
                _context.SaveChanges();

            }

        }

        /// <summary>
        /// Kontroler vraća sve tipove ocena predmeta npr(brojčana, opisna)
        /// Test name=PredmetController_VratiSveTipoveOcenaPredmeta
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Editor")]
        public JsonResult VratiSveTipoveOcenaPredmeta()
        {
            try
            {
                var tipoviOcenaPredmeta = _context.TipoviOcenaPredmeta
                .Select(t => new DTOTipOcenePredmeta() { TipOcenePredmetaId = t.TipOcenePredmetaId, Tip = t.Tip });

                if (tipoviOcenaPredmeta != null)
                {
                    return Json(tipoviOcenaPredmeta, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return Json(new List<DTOTipOcenePredmeta>(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrator, Editor")]
        public ActionResult IzmeniPredmet(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Predmet predmet = _context.Predmeti.SingleOrDefault(p => p.PredmetID == id);

            if (predmet == null)
            {
                return HttpNotFound();
            }

            var model = new IzmenaPredmetaViewModel
            {
                PredmetId = predmet.PredmetID,
                NazivPredmeta = predmet.NazivPredmeta,
                TipOcenePredmetaId = predmet.TipOcenePredmetaId,
                TipoviOcenaPredmeta = _context.TipoviOcenaPredmeta.Select(x => new TipOcenePredmetaViewModel
                {
                    TipOcenePredmetaId = x.TipOcenePredmetaId,
                    Tip = x.Tip
                }).ToList()
            };

            return View(model);
        }

        [Authorize(Roles = "Administrator, Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IzmeniPredmet(IzmenaPredmetaViewModel model)
         {
            //proverava se da li predmet vec postoji u bazi
            if (_context.Predmeti.Where(p => p.NazivPredmeta == model.NazivPredmeta).Any())
            {
                var predmetId = _context.Predmeti
                    .Where(p => p.NazivPredmeta == model.NazivPredmeta)
                    .Select(p => p.PredmetID)
                    .SingleOrDefault();

                if(predmetId!=model.PredmetId)
                {
                    ModelState.AddModelError("NazivPredmeta", "Predmet već postoji!");
                }
            }


            if (ModelState.IsValid)
            {
                var predmet = new Predmet
                {
                    PredmetID = model.PredmetId,
                    NazivPredmeta = model.NazivPredmeta,
                    TipOcenePredmetaId = model.TipOcenePredmetaId
                };

                _context.Predmeti.AddOrUpdate(predmet);
                _context.SaveChanges();

                return RedirectToAction("Index", new { izmenjenPredmet = true });
            }

            model.TipoviOcenaPredmeta = _context.TipoviOcenaPredmeta.Select(x => new TipOcenePredmetaViewModel
            {
                TipOcenePredmetaId = x.TipOcenePredmetaId,
                Tip = x.Tip

            }).ToList();

            model.Greska = true;

            return View(model);

        }
    }
}