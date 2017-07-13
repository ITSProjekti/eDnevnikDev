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
        /// Index kontrola. Test name= OdeljenjeController_Index
        /// </summary>
        /// <returns>Vraca Index View</returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Ukoliko je odeljenje vec kreirano tojest pocela je skolska godina, prikazuju se svi ucenici koji se trenutno
        /// nalaze u odeljenju koje je trenutno u toku u odredjenoj skolskoj godini. Status: U toku.
        /// NOT TESTED
        /// </summary>
        /// <returns></returns>
        public ActionResult PregledKreiranih()
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

        public JsonResult OdeljenjeSkolskaGodina(int godina, int oznaka, int status = 2)
        {
            var pov = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == oznaka && o.Razred == godina && o.StatusID == status);

            if(pov != null)
                return Json(new { PocetakSkolskeGodine = pov.PocetakSkolskeGodine }, JsonRequestBehavior.AllowGet);


            return Json(new { PocetakSkolskeGodine = 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OdeljenjeUcenici(int razred, int oznakaOdeljenja, int status)
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
                .SingleOrDefault(o => o.StatusID == status && o.Razred == razred && o.OznakaID == oznakaOdeljenja);

            var podaci = new DTOOdeljenjeSaUcenicima();

            if (odeljenje != null)
            {
                //podaci.Kreirano = odeljenje.Status.Opis == "Kreirano";
                if (status == 2)
                {
                    podaci.Ucenici = odeljenje.Ucenici
                        .OrderBy(u => u.Prezime)
                        .ThenBy(u => u.Ime)
                        .ThenBy(u => u.ImeOca)
                        .Select(u => new DTOUcenikOdeljenja { ID = u.UcenikID, Ime = u.Ime, Prezime = u.Prezime, Fotografija = u.Fotografija, BrojUDnevniku = u.BrojUDnevniku })
                        .ToList();
                }
                else
                {
                    podaci.Ucenici = odeljenje.Ucenici
                        .OrderBy(u => u.BrojUDnevniku)
                        .Select(u => new DTOUcenikOdeljenja { ID = u.UcenikID, Ime = u.Ime, Prezime = u.Prezime, Fotografija = u.Fotografija, BrojUDnevniku = u.BrojUDnevniku })
                        .ToList();
                }
                              
            }
            return Json(podaci, JsonRequestBehavior.AllowGet);

        }

        public int ArhivirajOdeljenje(Odeljenje o)
        {
            foreach (var ucenik in o.Ucenici)
            {
                _context.ArhivaOdeljenja.Add(new ArhivaOdeljenja() { OdeljenjeID = o.Id, UcenikID = ucenik.UcenikID });
            }

            o.StatusID = 1; // Status 1 je Arhivirano

            _context.SaveChanges();

            return 0;
        }

        public int PremestiUSledecuGodinu(Odeljenje odeljenje)
        {
            if (odeljenje.Razred == 4)
                return 0;

            var sledecaGodinaOdeljenjeUToku = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == odeljenje.OznakaID && o.Razred == odeljenje.Razred + 1 && o.StatusID == 2); // Status 2 je U toku

            if (sledecaGodinaOdeljenjeUToku == null)
            {
                sledecaGodinaOdeljenjeUToku = new Odeljenje()
                {
                    OznakaID = odeljenje.OznakaID,
                    Razred = odeljenje.Razred+1,
                    PocetakSkolskeGodine = Odeljenje.SledecaSkolskaGodina(odeljenje.Razred + 1, odeljenje.OznakaID, _context),
                    StatusID = 2,
                    KrajSkolskeGodine = Odeljenje.SledecaSkolskaGodina(odeljenje.Razred + 1, odeljenje.OznakaID, _context) + 1
                };

                _context.Odeljenja.Add(sledecaGodinaOdeljenjeUToku);
                _context.SaveChanges();
            }

            foreach (var ucenik in odeljenje.Ucenici)
            {
                ucenik.OdeljenjeId = sledecaGodinaOdeljenjeUToku.Id;
            }

            _context.SaveChanges();

            return 0;
        }


        [HttpPost]
        public int KreirajOdeljenje(DTOOdeljenje OdeljenjeZaKreiranje)
        {
            var tekuceKreiranoOdeljenje = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == OdeljenjeZaKreiranje.Oznaka && o.Razred == OdeljenjeZaKreiranje.Razred && o.StatusID == 3); //Status 3 je Kreirano

            if(tekuceKreiranoOdeljenje != null)
            {
                ArhivirajOdeljenje(tekuceKreiranoOdeljenje);
                PremestiUSledecuGodinu(tekuceKreiranoOdeljenje);
            }

            var odeljenjeZaPromenuStatusa = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == OdeljenjeZaKreiranje.Oznaka && o.Razred == OdeljenjeZaKreiranje.Razred && o.StatusID == 2); //Status 2 je U toku
            
            if (odeljenjeZaPromenuStatusa == null)
                return -1;

            var ucenici = _context.Ucenici
                .Where(u => u.OdeljenjeId == odeljenjeZaPromenuStatusa.Id)
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


            odeljenjeZaPromenuStatusa.StatusID = 3; //StatudID 3 je Kreirano
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


