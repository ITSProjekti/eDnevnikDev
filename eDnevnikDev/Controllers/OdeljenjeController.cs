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
        /// Index kontrola. Test name= OdeljenjeController_Index()
        /// </summary>
        /// <returns>Vraca Index View</returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Ukoliko je odeljenje vec kreirano tojest pocela je skolska godina, prikazuju se svi ucenici koji se trenutno
        /// nalaze u odeljenju koje je trenutno u toku u odredjenoj skolskoj godini. Status: U toku.
        /// Test name=PregledKreiranihTest()
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
            //Kreiranje kolekcije oznaka koje mogu ciniti odeljenja na godini koja je prosledjena kao parametar. O.O
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
            var datum = DateTime.Today;
            var casovi = _context.Casovi.
                Where(x => x.Datum == datum);

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
                    var uc = new List<DTOUcenikOdeljenja>();
                    if (casovi.Count() > 0)
                    {
                        int a = casovi.Max(x => x.RedniBrojCasa);
                        var cas = casovi.FirstOrDefault(x => x.RedniBrojCasa == a);

                        foreach (var item in odeljenje.Ucenici)
                        {
                            var odsutan = item.Odsustva.Where(x => x.CasId == cas.CasId).SingleOrDefault(x => x.UcenikId == item.UcenikID);

                            if (odsutan == null)
                            {
                                uc.Add(new DTOUcenikOdeljenja { ID = item.UcenikID, Ime = item.Ime, Prezime = item.Prezime, Fotografija = item.Fotografija, BrojUDnevniku = item.BrojUDnevniku, Prisutan = true });
                            }
                            else
                            {
                                uc.Add(new DTOUcenikOdeljenja { ID = item.UcenikID, Ime = item.Ime, Prezime = item.Prezime, Fotografija = item.Fotografija, BrojUDnevniku = item.BrojUDnevniku, Prisutan = false });
                            }
                        }

                        podaci.Ucenici = uc.OrderBy(x => x.BrojUDnevniku).ToList();

                    }
                    else
                    {
                        podaci.Ucenici = odeljenje.Ucenici
                            .OrderBy(u => u.BrojUDnevniku)
                            .Select(u => new DTOUcenikOdeljenja { ID = u.UcenikID, Ime = u.Ime, Prezime = u.Prezime, Fotografija = u.Fotografija, BrojUDnevniku = u.BrojUDnevniku, Prisutan = true })
                            .ToList();
                    }

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
        /// Akcija za premestanje ucenika u sledeci razred u odeljenje sa istom oznakom kao i proslo,i prebacivanje ucenika u to novo odeljenje prilikom
        /// arhiviranja trenutnog odeljenje usled zavrsetka skolske godine. 
        /// </summary>
        /// <param name="odeljenje"></param>
        /// <returns></returns>
        public int PremestiUSledecuGodinu(Odeljenje odeljenje)
        {
            var pom = _context.Odeljenja.Include("Oznaka").SingleOrDefault(o => o.Id == odeljenje.Id);

            //Ukoliko je 4. razred ucenici se ne premestaju nigde,ili ukoliko je odljenje na trecoj godini na trogodisnjem smeru.
            if (odeljenje.Razred == 4 || pom.Oznaka.Smerovi.Any(x => x.Trajanje == 3 && odeljenje.Razred == 3)) 
                return 0;

            //objekat u kom se cuva odeljenje iste oznake za sledecu godinu,u koje treba ucenici automatksi da se premeste.
            var sledecaGodinaOdeljenjeUToku = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == odeljenje.OznakaID && o.Razred == odeljenje.Razred + 1 && o.StatusID == 2); // Status 2 je U toku

            //Ukoliko ne postoji odeljenje koje je kreirano i u toku,kreira se novo sa godinom upisa jednom vecom od prosle godine.
            ///<see cref="Odeljenje.SledecaSkolskaGodina(int, int, ApplicationDbContext)"/>
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
            //Dodavanje ucenika u isto odeljenje sledeceg razreda sa statusom u toku.
            foreach (var ucenik in odeljenje.Ucenici)
            {
                ucenik.OdeljenjeId = sledecaGodinaOdeljenjeUToku.Id;
            }

            _context.SaveChanges();

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OdeljenjeZaKreiranje"></param>
        /// <returns></returns>
        [HttpPost]
        public int KreirajOdeljenje(DTOOdeljenje OdeljenjeZaKreiranje)
        {
            //Objekat vec kreiranog odeljenja.
            var tekuceKreiranoOdeljenje = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == OdeljenjeZaKreiranje.Oznaka && o.Razred == OdeljenjeZaKreiranje.Razred && o.StatusID == 3); //Status 3 je Kreirano

            //Ukoliko postoji,to odeljenje se arhivira i ucenici iz njega se premestaju u sledecu godinu status odeljenja u toku.
            if(tekuceKreiranoOdeljenje != null)
            {
                ArhivirajOdeljenje(tekuceKreiranoOdeljenje);
                PremestiUSledecuGodinu(tekuceKreiranoOdeljenje);
            }
            
            //Tekuce odeljenje koje trenutno ima status u toku a treba se kreira.
            var odeljenjeZaPromenuStatusa = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == OdeljenjeZaKreiranje.Oznaka && o.Razred == OdeljenjeZaKreiranje.Razred && o.StatusID == 2); //Status 2 je U toku
            
            //Ukoliko je null doslo je do nepoklapanja interfejsa na frontendu i podataka u bazi. 
            if (odeljenjeZaPromenuStatusa == null)
                return -1;

            //Svi ucenici koji su bili u odeljenju u toku se dodaju u listu,sortirani po
            //Prezime, Ime, Ime oca
            var ucenici = _context.Ucenici
                .Where(u => u.OdeljenjeId == odeljenjeZaPromenuStatusa.Id)
                .OrderBy(u => u.Prezime)
                .ThenBy(u => u.Ime)
                .ThenBy(u => u.ImeOca)
                .ToList();

            //Brojac radi dodavanja broja u dnevniku, i generisanja jedinstvenog broja.
            ///<see cref="Ucenik.GenerisiJedinstveniBroj"/>
            int brojac = 1;

            foreach (Ucenik ucenik in ucenici)
            {
                ucenik.BrojUDnevniku = brojac++;
                ucenik.GenerisiJedinstveniBroj();
            }

            //Promena statusa odeljenja na "Kreirano".
            odeljenjeZaPromenuStatusa.StatusID = 3; //StatudID 3 je Kreirano
            _context.SaveChanges();
            return 0;
        }
        /// <summary>
        /// Upisuje cas i odsutne ucenike u bazu.
        /// </summary>
        /// <param name="odsutni">Lista sa ID-evima od odsutnih ucenika</param>
        /// <returns></returns>
        public JsonResult UpisiOdsutne(int[] odsutni)
        {
            //Hardcode -> drugi story resava ovaj deo
            var datum = DateTime.Today;
            var casId = 7;
            var opis = "Cas 2";
            var profesor = 1;
            var predmet = 1;
            var odeljenje = 4;
            var polugodiste = 1;
            var tromesecje = 1;
            var redniBrojPredmete = 3;
            var redniBrojCasa = 2;

            Cas cas = new Cas() {CasId = casId, Datum = datum, Opis = opis, ProfesorId = profesor,
                PredmetId = predmet, OdeljenjeId = odeljenje, Polugodiste = polugodiste,
                Tromesecje = tromesecje, RedniBrojPredmeta = redniBrojPredmete, RedniBrojCasa = redniBrojCasa };

            _context.Casovi.Add(cas);
            _context.SaveChanges();

            foreach (var odsutan in odsutni)
            {
                Odsustvo o = new Odsustvo();
                o.CasId = casId;
                o.UcenikId = odsutan;

                _context.Odsustva.Add(o);
                _context.SaveChanges();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Vraca redni broj casa koji je sledeci po redu da se odrzi.
        /// </summary>
        /// <param name="odeljenje">odeljenje koje se dobija iz combobox-a.</param>
        /// <param name="razred">Razred koji se dobija iz combobox-a.</param>
        /// <returns></returns>
        public JsonResult RedniBrojCasa(int odeljenje, int razred)
        {
            var datum = DateTime.Today;
            var izabranoOdeljenje = _context.Odeljenja.Where(x => x.OznakaID == odeljenje).Single(x => x.Razred == razred);
            var casovi = _context.Casovi.
                Where(x => x.Datum == datum && x.OdeljenjeId == izabranoOdeljenje.Id);
            // vraca najveci redni broj casa, zato sto je taj poslednji odrzan
            int a = casovi.Max(x => x.RedniBrojCasa);

            // a+1 -> povecava redni broj casa, zato sto je to sledeci cas koji treba da se odrzi
            return Json(a+1, JsonRequestBehavior.AllowGet);
        }

    }
}


