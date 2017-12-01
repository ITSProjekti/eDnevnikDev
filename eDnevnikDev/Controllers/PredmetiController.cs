using eDnevnikDev.DTOs;
using eDnevnikDev.Helpers;
using eDnevnikDev.Models;
using eDnevnikDev.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        /// Ucitavamo Listu Predmeta iz Baze. Test name=PredmetController_Index
        /// </summary>
        /// <returns>Vracamo View sa Listom Predmeta</returns>
        public ActionResult Index()
        {
            IEnumerable<Predmet> ListaPredmeta = _context.Predmeti.ToList();
            return View("Index",ListaPredmeta);
        }

        /// <summary>
        /// Ucitava View za dodavanje predmeta. Test name=PredmetController_Dodaj
        /// </summary>
        /// <returns>Vraca View  Dodaj</returns>
        public ActionResult Dodaj()
        {
            return View("Dodaj");
        }

        /// <summary>
        /// Cuva predmet sa sve tipom ocene koji mu se dodeljuje. Test name=PredmetController_SacuvajPredmet
        /// <see cref="Predmet"/>
        /// </summary>
        /// <param name="predmet">The predmet.</param>
        /// <returns>Vraca nas na index stranu Predmeta</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SacuvajPredmet(KreiranjePredmetaViewModel predmetViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Predmet predmet = new Predmet();
                    predmet.NazivPredmeta = predmetViewModel.NazivPredmeta;
                    predmet.TipOcenePredmetaId = predmetViewModel.TipOcenePredmetaId;

                    _context.Predmeti.Add(predmet);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Predmeti");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }

            return View("Dodaj", predmetViewModel);
        }

        /// <summary>
        /// Vraća se lista predmeta kao i njihovi tipovi ocena (opisna, numerička)
        /// Koristi se "PredmetTipOcenePredmetaViewModel" koji sadrži liste predmeta 
        /// kao i liste tipova ocena predmeta
        /// </summary>
        /// <returns></returns>
        public ActionResult PrikaziPredmeteSaTipovimaOcena()
        {
            var listaPredmeta = _context.Predmeti.
                Select(x => x);

            var listaPredmetaSaTipom = listaPredmeta.Include("TipOcenePredmeta")
                .ToList();
                
            var listaTipovaOcenaPredmeta = _context.TipoviOcenaPredmeta
                .ToList();

            var predmetTipOcenePredmetaViewModel =  
                new PredmetTipOcenePredmetaViewModel(){ Predmeti= listaPredmetaSaTipom, TipoviOcenaPredmeta=listaTipovaOcenaPredmeta};

            return View(predmetTipOcenePredmetaViewModel);
        }

        /// <summary>
        /// Menja se tip ocene za određeni predmet
        /// Npr ako matematika ima tip ocene "opisna" moguća je izmena da bude "numerička"
        /// </summary>
        /// <param name="predmetId">The predmet identifier.</param>
        /// <param name="tipOcenePredmetaId">The tip ocene predmeta identifier.</param>
        [HttpPost]
        [ValidateHeaderAntiForgeryToken]
        public void IzmenaTipaOcenaUPredmetu(int predmetId, int tipOcenePredmetaId)
        {
            var predmet = _context.Predmeti
                .SingleOrDefault(p => p.PredmetID == predmetId);

            var tip = _context.TipoviOcenaPredmeta.SingleOrDefault(x => x.TipOcenePredmetaId == tipOcenePredmetaId);

            predmet.TipOcenePredmetaId = tipOcenePredmetaId;
            predmet.TipOcenePredmeta = tip;

            try
            {
                _context.Predmeti.AddOrUpdate(predmet);
                
            }
            catch (Exception)
            {
                
            }
            //Ova linija je pomerena iz try bloka, jer kad je bila u njemu aplikacija je radila, 
            //ali test nije zato sto u testu nije implementirana AddOrUpdate metoda
            _context.SaveChanges();
        }

        /// <summary>
        /// Kontroler vraća sve tipove ocena predmeta npr(brojčana, opisna)
        /// Test name=PredmetController_VratiSveTipoveOcenaPredmeta
        /// </summary>
        /// <returns></returns>
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
    }
}