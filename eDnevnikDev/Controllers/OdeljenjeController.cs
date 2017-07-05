using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.DTOs;

namespace eDnevnikDev.Controllers
{

    public class OdeljenjeController : Controller
    {
        ApplicationDbContext _context;

        public OdeljenjeController()
        {
            _context = new ApplicationDbContext();

        }
        public OdeljenjeController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: Odeljenje
        /// <summary>
        /// Index kontrola
        /// </summary>
        /// <returns>Vraca View za Index</returns>
        public ActionResult Index()
        {
            





            return View();
        }

        public JsonResult OdeljenjeTrajanje(int godina)
        {
            var kolekcijaOznaka = _context.Smerovi
                .Where(s => s.Trajanje >= godina)
                .Select(s => s.Oznake).ToList();

            var pov = new List<DTOOdeljenje>();

            foreach(var lista in kolekcijaOznaka)
            {
                foreach(var oznaka in lista)
                {
                    if( !pov.Any( o => o.Oznaka == oznaka.OznakaId))
                        pov.Add(new DTOOdeljenje {Oznaka = oznaka.OznakaId }); 
                }
            }

            pov = pov.OrderBy(o => o.Oznaka).ToList();

            return Json(pov, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OdeljenjeUcenici(int razred, int oznakaOdeljenja)
        {
            //var listaUcenika = _context.Ucenici
            //    .Where(u => u.Razred == razred && u.OdeljenjeId == idOdeljenja)
            //    .OrderBy(u => u.Prezime)
            //    .ThenBy(u => u.Ime)
            //    .ThenBy(u => u.ImeOca)
            //    .Select(u=> new DTOUcenikOdeljenja { ID= u.UcenikID, Ime=u.Ime, Prezime=u.Prezime, Fotografija=u.Fotografija })
            //    .ToList();

            var odeljenje = _context.Odeljenja
                .Include("Status")
                .SingleOrDefault(o => o.Status.Opis != "Arhivirano" && o.Razred == razred && o.OznakaID == oznakaOdeljenja);

            var podaci = new DTOOdeljenjeSaUcenicima();

            if (odeljenje != null)
            {
                podaci.Kreirano = odeljenje.Status.Opis == "Kreirano";
                podaci.Ucenici = odeljenje.Ucenici
                    .OrderBy(u => u.Prezime)
                    .ThenBy(u => u.Ime)
                    .ThenBy(u => u.ImeOca)
                    .Select(u => new DTOUcenikOdeljenja { ID = u.UcenikID, Ime = u.Ime, Prezime = u.Prezime, Fotografija = u.Fotografija })
                    .ToList();                
            }
            return Json(podaci, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public int KreirajOdeljenje(DTOOdeljenje OdeljenjeZaKreiranje)
        {
            var odeljenje = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == OdeljenjeZaKreiranje.Oznaka && o.Razred == OdeljenjeZaKreiranje.Razred);
            if (odeljenje == null)
                return -1;

            var ucenici = _context.Ucenici
                .Where(u => u.OdeljenjeId == odeljenje.Id)
                .OrderBy(u => u.Prezime)
                .ThenBy(u => u.Ime)
                .ThenBy(u => u.ImeOca)
                .ToList();

            int brojac = 1;

            foreach (Ucenik ucenik in ucenici)
            {
                ucenik.BrojUDnevniku = brojac++;
                ucenik.GenerisiJedinstveniBroj();
            }


            odeljenje.StatusID = 3; //StatudID 3 je Kreirano
            _context.SaveChanges();
            return 0;
        }

        public JsonResult MogucnostArhiviranja(int razred, int oznakaOdeljenja)
        {
            int razredSledeceGodine = razred + 1;

            if (razred != 4 && _context.Odeljenja.Any(o => o.OznakaID == oznakaOdeljenja && o.Razred == razredSledeceGodine && o.StatusID != 1))//StatusID 1 je Arhivirano
            {
                return Json(new { Moguce = false }, JsonRequestBehavior.AllowGet);
            }


            var odeljenje = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == oznakaOdeljenja && o.Razred == razred);

            foreach (var ucenik in odeljenje.Ucenici)
            {
                _context.ArhivaOdeljenja.Add(new ArhivaOdeljenja() { OdeljenjeID = odeljenje.Id, UcenikID = ucenik.UcenikID });
            }

            odeljenje.StatusID = 1;//StatusID 1 je Arhivirano

            if (razred != 4)
            {
                var novoOdeljenje = new Odeljenje()
                {
                    OznakaID = odeljenje.OznakaID,
                    Razred = razredSledeceGodine,
                    PocetakSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznakaOdeljenja,_context),
                    StatusID = 2,
                    KrajSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznakaOdeljenja,_context) + 1
                };


            }


            return Json(new { Moguce = true }, JsonRequestBehavior.AllowGet);
        }

    }
}