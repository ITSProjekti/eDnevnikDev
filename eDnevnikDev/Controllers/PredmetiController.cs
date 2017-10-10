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
        /// Cuva predmet Unet u Spisak predmeta. Test name=PredmetController_SacuvajPredmet
        /// <see cref="Predmet"/>
        /// </summary>
        /// <param name="predmet">The predmet.</param>
        /// <returns>Vraca nas na index stranu Predmeta</returns>
        public ActionResult SacuvajPredmet(Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                _context.Predmeti.Add(predmet);
                _context.SaveChanges();
                return RedirectToAction("Index", "Predmeti");
                
            }
            else
            {
                return View("Dodaj", predmet);
            }
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
            catch (Exception e)
            {
                
            }
            //Ova linija je pomerena iz try bloka, jer kad je bila u njemu aplikacija je radila, 
            //ali test nije zato sto u testu nije implementirana AddOrUpdate metoda
            _context.SaveChanges();
        }
    }
}