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

        /// <summary>
        /// Koristi se svude gde se bira godina, i popunjava se combobox sa listom odeljenja za tu godinu.
        /// Primer: 4-godina (1,2,3,4,5,6). 3-godina (1,2,3,4,5,6,7).
        /// <see cref="DTOOdeljenje"/>
        /// Test name= OdeljenjeController_OdeljenjeTrajanje
        /// </summary>
        /// <param name="godina"></param>
        /// <returns></returns>
        public JsonResult OdeljenjeTrajanje(int godina)
        {
            //Kreiranje kolekcije oznaka koje mogu ciniti odeljenja na godini koja je prosledjena kao parameta. O.O
            var kolekcijaOznaka = _context.Smerovi
                .Where(s => s.Trajanje >= godina)
                .Select(s => s.Oznake).ToList();

            //Koristi se DTOOdeljnje, iako je Razred visak. Ne menja stvari na frontendu.
            var pov = new List<DTOOdeljenje>();

            foreach(var lista in kolekcijaOznaka)
            {
                foreach(var oznaka in lista)
                {
                   if( !pov.Any( o => o.Oznaka == oznaka.OznakaId))
                        pov.Add(new DTOOdeljenje {Oznaka = oznaka.OznakaId }); 
                }
            }
            //Sortiranje.
            pov = pov.OrderBy(o => o.Oznaka).ToList();
            //Formatiranje JSON-a.
            return Json(pov, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Formatira se Json sa informacijom za koju skolsku godinu se ubacuju ucenici u odeljenje sa statusom u toku.
        /// <see cref="Odeljenje.PocetakSkolskeGodine"/>
        /// </summary>
        /// <param name="godina"></param>
        /// <param name="oznaka"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult OdeljenjeSkolskaGodina(int godina, int oznaka, int status = 2)
        {
            //Trazi odeljenje sa prosledjenom godinom oznakom i statusom u toku(Moze i hardcode).
            var pov = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == oznaka && o.Razred == godina && o.StatusID == status);

            //Ukoliko je pronadjeno odeljenje vraca se sledeca skolska godina
            if(pov != null)
                return Json(new { PocetakSkolskeGodine = pov.PocetakSkolskeGodine }, JsonRequestBehavior.AllowGet);


            return Json(new { PocetakSkolskeGodine = 0 }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Vraca listu ucenika u JSON-u, i sortira ih u zavisnosti od statusa odeljenja.
        /// NPR: odeljenje za koje je upis u toku sortira po prezimenu,dok odeljenje koje je vec kreirano sortira po
        /// broju u dnevniku
        /// <see cref="DTOOdeljenjeSaUcenicima"/>
        /// <see cref="DTOUcenikOdeljenja"/> 
        /// </summary>
        /// <param name="razred"></param>
        /// <param name="oznakaOdeljenja"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult OdeljenjeUcenici(int razred, int oznakaOdeljenja, int status)
        {
          
            //Pronalazi odeljenje.
            var odeljenje = _context.Odeljenja
                .Include("Status")
                .SingleOrDefault(o => o.StatusID == status && o.Razred == razred && o.OznakaID == oznakaOdeljenja);

            
            var podaci = new DTOOdeljenjeSaUcenicima();

            if (odeljenje != null)
            {
                //Ako je status u toku, prikazuju se ucenici ubaceni u to odeljenje sortirani po:
                //1. prezime, 2. ime, 3. ImeOca.
                if (status == 2)
                {
                    podaci.Ucenici = odeljenje.Ucenici
                        .OrderBy(u => u.Prezime)
                        .ThenBy(u => u.Ime)
                        .ThenBy(u => u.ImeOca)
                        .Select(u => new DTOUcenikOdeljenja { ID = u.UcenikID, Ime = u.Ime, Prezime = u.Prezime, Fotografija = u.Fotografija, BrojUDnevniku = u.BrojUDnevniku })
                        .ToList();
                }
                //Ako je status kreirano, sortiraju se ucenici po broju u dnevniku.
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
        /// <summary>
        /// Popounjava se tabela ArhivaOdeljenja u bazi. I postavlja se status odeljenja na 1.(Arhivirano).
        /// <see cref="ArhivaOdeljenja"/> 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>

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

        /// <summary>
        /// TO BE REFACTORED. BEWARE. STAY OUT.
        /// </summary>
        /// <param name="odeljenje"></param>
        /// <returns></returns>
        public int PremestiUSledecuGodinu(Odeljenje odeljenje)
        {
            //Ukoliko je 4. razred ucenici se ne premestaju nigde.
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


