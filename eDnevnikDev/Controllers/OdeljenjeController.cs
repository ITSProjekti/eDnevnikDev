using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.DTOs;
using eDnevnikDev.ViewModel;
using System.Data.Entity.Migrations;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

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
        private ActionResult Index()
        {
            var skolskGodina = _context.SkolskaGodine.Where(s => s.Aktuelna == true)
                                                    .Select(s => new SkolskaGodinaViewModel
                                                    {
                                                        PocetakSkolskeGodine = s.PocetakSkolskeGodine.Year,
                                                        KrajSkolskeGodine = s.KrajSkolskeGodine.Year
                                                    })
                                                    .SingleOrDefault();
            if (skolskGodina != null)
            {
                return View(skolskGodina);
            }

            return View(new SkolskaGodinaViewModel());

        }
        /// <summary>
        /// Ukoliko je odeljenje vec kreirano tojest pocela je skolska godina, prikazuju se svi ucenici koji se trenutno
        /// nalaze u odeljenju koje je trenutno u toku u odredjenoj skolskoj godini. Status: U toku.
        /// Test name=PregledKreiranihTest()
        /// </summary>
        /// <returns></returns>
        private ActionResult PregledKreiranih()
        {
            var skolskGodina = _context.SkolskaGodine.Where(s => s.Aktuelna == true)
                                                     .Select(s => new SkolskaGodinaViewModel
                                                     {
                                                         PocetakSkolskeGodine = s.PocetakSkolskeGodine.Year,
                                                         KrajSkolskeGodine = s.KrajSkolskeGodine.Year
                                                     })
                                                     .SingleOrDefault();
            if (skolskGodina != null)
            {
                return View(skolskGodina);
            }

            return View(new SkolskaGodinaViewModel());
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

            foreach (var lista in kolekcijaOznaka)
            {
                foreach (var oznaka in lista)
                {
                    if (!pov.Any(o => o.Oznaka == oznaka.OznakaId))
                        pov.Add(new DTOOdeljenje { Oznaka = oznaka.OznakaId });
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
        private JsonResult OdeljenjeSkolskaGodina(int godina, int oznaka, int status = 2)
        {
            //Trazi odeljenje sa prosledjenom godinom oznakom i statusom u toku(Moze i hardcode).
            var pov = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == oznaka && o.Razred == godina && o.StatusID == status);

            //Ukoliko je pronadjeno odeljenje vraca se sledeca skolska godina
            if (pov != null)
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
        private JsonResult OdeljenjeUcenici(int razred, int oznakaOdeljenja, int status)
        {
            var datum = DateTime.Today;
            var casovi = _context.Casovi.
                Where(x => x.Datum == datum);

            //Pronalazi odeljenje.
            var odeljenje = _context.Odeljenja
                .Include("Status")
                .SingleOrDefault(o => o.StatusID == status && o.Razred == razred && o.OznakaID == oznakaOdeljenja);

            var polovi = _context.Polovi.ToList();
            var statusiUcenika = _context.StatusiUcenika.ToList();

            var podaci = new DTOOdeljenjeSaUcenicima();

            if (odeljenje != null && polovi != null && polovi.Count() > 0 && statusiUcenika != null && statusiUcenika.Count() > 0)
            {
                //Ako je status u toku, prikazuju se ucenici ubaceni u to odeljenje sortirani po:
                //1. prezime, 2. ime, 3. ImeOca.
                if (status == 2)
                {

                    var ucenici = odeljenje.Ucenici
                                           .OrderBy(u => u.Prezime)
                                           .ThenBy(u => u.Ime)
                                           .ThenBy(u => u.ImeOca)
                                           .Select(u => u)
                                           .ToList();


                    if (ucenici != null && ucenici.Count() > 0)
                    {
                        podaci.Ucenici = new List<DTOUcenikOdeljenja>();

                        foreach (var ucenik in ucenici)
                        {
                            var dtoUcenik = new DTOUcenikOdeljenja();
                            dtoUcenik.ID = ucenik.UcenikID;
                            dtoUcenik.Ime = ucenik.Ime;
                            dtoUcenik.Prezime = ucenik.Prezime;
                            dtoUcenik.Pol = polovi.Where(p => p.PolId == ucenik.PolId)
                                                  .SingleOrDefault()
                                                  .Naziv;

                            if (ucenik.Fotografija != null)
                            {
                                dtoUcenik.Fotografija = Convert.ToBase64String(ucenik.Fotografija);
                            }
                            dtoUcenik.Status = statusiUcenika.Where(s => s.StatusUcenikaId == ucenik.StatusUcenikaId)
                                                             .SingleOrDefault()
                                                             .Opis;

                            podaci.Ucenici.Add(dtoUcenik);
                        }
                    }

                }
                //Ako je status kreirano, sortiraju se ucenici po broju u dnevniku.
                else
                {
                    var uc = new List<DTOUcenikOdeljenja>();
                    if (casovi.Count() > 0)
                    {
                        int a = casovi.Max(x => x.RedniBrojCasa);
                        var cas = casovi.FirstOrDefault(x => x.RedniBrojCasa == a);

                        foreach (var ucenik in odeljenje.Ucenici)
                        {
                            var dtoUcenik = new DTOUcenikOdeljenja();
                            dtoUcenik.ID = ucenik.UcenikID;
                            dtoUcenik.Ime = ucenik.Ime;
                            dtoUcenik.Prezime = ucenik.Prezime;
                            dtoUcenik.BrojUDnevniku = ucenik.BrojUDnevniku;
                            dtoUcenik.Pol = polovi.Where(p => p.PolId == ucenik.PolId)
                                                  .SingleOrDefault()
                                                  .Naziv;

                            if (ucenik.Fotografija != null)
                            {
                                dtoUcenik.Fotografija = Convert.ToBase64String(ucenik.Fotografija);
                            }

                            dtoUcenik.Status = statusiUcenika.Where(s => s.StatusUcenikaId == ucenik.StatusUcenikaId)
                                                             .SingleOrDefault()
                                                             .Opis;

                            var odsutan = ucenik.Odsustva.Where(x => x.CasId == cas.CasId).SingleOrDefault(x => x.UcenikId == ucenik.UcenikID);

                            if (odsutan == null)
                            {
                                dtoUcenik.Prisutan = true;
                            }
                            else
                            {
                                dtoUcenik.Prisutan = true;
                            }

                            uc.Add(dtoUcenik);
                        }

                        podaci.Ucenici = uc.OrderBy(x => x.BrojUDnevniku).ToList();

                    }
                    else
                    {
                        var ucenici = odeljenje.Ucenici
                                          .OrderBy(u => u.BrojUDnevniku)
                                          .Select(u => u)
                                          .ToList();


                        if (ucenici != null && ucenici.Count() > 0)
                        {
                            podaci.Ucenici = new List<DTOUcenikOdeljenja>();

                            foreach (var ucenik in ucenici)
                            {
                                var dtoUcenik = new DTOUcenikOdeljenja();
                                dtoUcenik.ID = ucenik.UcenikID;
                                dtoUcenik.Ime = ucenik.Ime;
                                dtoUcenik.Prezime = ucenik.Prezime;
                                dtoUcenik.BrojUDnevniku = ucenik.BrojUDnevniku;
                                dtoUcenik.Pol = polovi.Where(p => p.PolId == ucenik.PolId)
                                                      .SingleOrDefault()
                                                      .Naziv;
                                dtoUcenik.Prisutan = true;

                                if (ucenik.Fotografija != null)
                                {
                                    dtoUcenik.Fotografija = Convert.ToBase64String(ucenik.Fotografija);
                                }

                                dtoUcenik.Status = statusiUcenika.Where(s => s.StatusUcenikaId == ucenik.StatusUcenikaId)
                                                             .SingleOrDefault()
                                                             .Opis;

                                podaci.Ucenici.Add(dtoUcenik);
                            }
                        }

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

        private int ArhivirajOdeljenje(Odeljenje o)
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
        private int PremestiUSledecuGodinu(Odeljenje odeljenje)
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
                    Razred = odeljenje.Razred + 1,
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
                if(ucenik.StatusUcenikaId==1)//Status 1 je Aktivan
                {
                    ucenik.OdeljenjeId = sledecaGodinaOdeljenjeUToku.Id;
                }
            }

            _context.SaveChanges();

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OdeljenjeZaKreiranje"></param>
        /// <returns></returns>
        //[HttpPost]
        //public int KreirajOdeljenje(DTOOdeljenje OdeljenjeZaKreiranje)
        //{
        //    //Objekat vec kreiranog odeljenja.
        //    var tekuceKreiranoOdeljenje = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == OdeljenjeZaKreiranje.Oznaka && o.Razred == OdeljenjeZaKreiranje.Razred && o.StatusID == 3); //Status 3 je Kreirano

        //    //Ukoliko postoji,to odeljenje se arhivira i ucenici iz njega se premestaju u sledecu godinu status odeljenja u toku.
        //    if (tekuceKreiranoOdeljenje != null)
        //    {
        //        ArhivirajOdeljenje(tekuceKreiranoOdeljenje);
        //        PremestiUSledecuGodinu(tekuceKreiranoOdeljenje);
        //    }

        //    //Tekuce odeljenje koje trenutno ima status u toku a treba se kreira.
        //    var odeljenjeZaPromenuStatusa = _context.Odeljenja.SingleOrDefault(o => o.OznakaID == OdeljenjeZaKreiranje.Oznaka && o.Razred == OdeljenjeZaKreiranje.Razred && o.StatusID == 2); //Status 2 je U toku

        //    //Ukoliko je null doslo je do nepoklapanja interfejsa na frontendu i podataka u bazi. 
        //    if (odeljenjeZaPromenuStatusa == null)
        //        return -1;

        //    //Svi ucenici koji su bili u odeljenju u toku se dodaju u listu,sortirani po
        //    //Prezime, Ime, Ime oca
        //    var ucenici = _context.Ucenici
        //        .Where(u => u.OdeljenjeId == odeljenjeZaPromenuStatusa.Id)
        //        .OrderBy(u => u.Prezime)
        //        .ThenBy(u => u.Ime)
        //        .ThenBy(u => u.ImeOca)
        //        .ToList();

        //    //Brojac radi dodavanja broja u dnevniku, i generisanja jedinstvenog broja.
        //    ///<see cref="Ucenik.GenerisiJedinstveniBroj"/>
        //    int brojac = 1;

        //    foreach (Ucenik ucenik in ucenici)
        //    {
        //        ucenik.BrojUDnevniku = brojac++;
        //        ucenik.GenerisiJedinstveniBroj();
        //    }

        //    //Promena statusa odeljenja na "Kreirano".
        //    odeljenjeZaPromenuStatusa.StatusID = 3; //StatudID 3 je Kreirano
        //    _context.SaveChanges();
        //    return 0;
        //}
        /// <summary>
        /// Upisuje cas i odsutne ucenike u bazu.
        /// </summary>
        /// <param name="odsutni">Lista sa ID-evima od odsutnih ucenika</param>
        /// <returns></returns>
        private JsonResult UpisiOdsutne(int[] odsutni)
        {
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

            Cas cas = new Cas()
            {
                CasId = casId,
                Datum = datum,
                Opis = opis,
                ProfesorId = profesor,
                PredmetId = predmet,
                OdeljenjeId = odeljenje,
                Polugodiste = polugodiste,
                Tromesecje = tromesecje,
                RedniBrojPredmeta = redniBrojPredmete,
                RedniBrojCasa = redniBrojCasa
            };

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
        private JsonResult RedniBrojCasa(int odeljenje, int razred)
        {
            var datum = DateTime.Today;
            var izabranoOdeljenje = _context.Odeljenja.Where(x => x.OznakaID == odeljenje && x.StatusID == 3).Single(x => x.Razred == razred);
            var casovi = _context.Casovi. // Ne povlaci casove pa se javlja greska u x.RedniBrojCasa!!!
                Where(x => x.Datum == datum && x.OdeljenjeId == izabranoOdeljenje.Id);
            // vraca najveci redni broj casa, zato sto je taj poslednji odrzan
            int maxCas;
            try
            {
                maxCas = casovi.Max(x => x.RedniBrojCasa);
            }
            catch (Exception)
            {

                maxCas = 0;
            }


            try
            {
                maxCas = casovi.Max(x => x.RedniBrojCasa);
            }
            catch
            {
                maxCas = 0;
            }
            // a+1 -> povecava redni broj casa, zato sto je to sledeci cas koji treba da se odrzi
            return Json(maxCas + 1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// <see cref="NerasporedjenUcenikViewModel"/>
        /// <see cref="KreirajOdeljenjeViewModel"/>
        /// </summary>
        /// <returns>KreirajOdeljenje View</returns>
        private ActionResult KreirajOdeljenje()
        {

            var model = new KreirajOdeljenjeViewModel
            {
                ListaNerasporedjenihUcenika = _context.Ucenici.Where(u => u.OdeljenjeId == null)
                                                      .Where(u => u.StatusUcenikaId == 1)//Status 1 je Aktivan
                                                      .Select(u => new NerasporedjenUcenikViewModel
                                                      {
                                                          UcenikId = u.UcenikID,
                                                          Ime = u.Ime,
                                                          Prezime = u.Prezime,
                                                          JMBG = u.JMBG,
                                                          Smer = u.Smer.NazivSmera
                                                      })
                                                      .ToList()

            };

            return View(model);
        }

        /// <summary>
        /// Vraca redni broj casa koji je sledeci po redu da se odrzi.
        /// <see cref=" DTORasporedjenUcenik"/>
        /// </summary>
        /// <param name="razred">razred koji se dobija iz combobox-a.</param>
        /// <param name="oznaka">oznaka kodeljenja koja se dobija iz combobox-a.</param>
        /// <returns></returns>
        private JsonResult VratiUcenikeZaOdeljenje(int? razred, int? oznaka)
        {
            if (razred != null && oznaka != null)
            {
                //ID odeljenja koje se pronalazi na osnovu razreda i oznake i koje nije arhivirano
                int? odeljenjeId = _context.Odeljenja.Where(o => o.Razred == razred && o.OznakaID == oznaka && o.StatusID != 1)//StatusID 1 je arhivirano odeljenje
                                                     .Select(o => o.Id)
                                                     .SingleOrDefault();


                if (odeljenjeId != null && odeljenjeId > 0)
                {

                    //Lista ucenika koji su upisani u konkretno odeljenje
                    List<DTORasporedjenUcenik> listaRasporedjenihUcenika = _context.Ucenici
                                      .Where(u => u.OdeljenjeId == odeljenjeId)
                                      .Where(u => u.StatusUcenikaId == 1)//Status 1 je Aktivan
                                      .Select(u => new DTORasporedjenUcenik
                                      {
                                          UcenikId = u.UcenikID,
                                          JMBG = u.JMBG,
                                          Ime = u.Ime,
                                          Prezime = u.Prezime,
                                          OdeljenjeId = (int)u.OdeljenjeId,
                                          Smer = u.Smer.NazivSmera
                                      }).ToList();


                    if (listaRasporedjenihUcenika.Count() > 0)
                    {
                        //Ukoliko ima ucenika u odeljenju vraca se lista ucenika
                        return Json(listaRasporedjenihUcenika, JsonRequestBehavior.AllowGet);
                    }

                    //Vracanje prazne liste, ukoliko nema ucenika u odeljenju
                    return Json(new List<DTORasporedjenUcenik>(), JsonRequestBehavior.AllowGet);
                }

                //Vracanje prazne liste, ukoliko je doslo do greske
                return Json(new List<DTORasporedjenUcenik>(), JsonRequestBehavior.AllowGet);

            }
            //Vracanje prazne liste, ukoliko je doslo do greske
            return Json(new List<DTORasporedjenUcenik>(), JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Metoda dodeljuje uceniku odeljenje
        /// </summary>
        /// <param name="ucenikId"></param>
        /// <param name="razred"></param>
        /// <param name="oznaka"></param>
        /// <returns></returns>
        private void DodajUcenikaUOdeljenje(int ucenikId, int razred, int oznaka)
        {
            var skolskaGodina = _context.SkolskaGodine.SingleOrDefault(s => s.Aktuelna == true);

            if (skolskaGodina != null)
            {
                DateTime pocetakSkolskeGodine = skolskaGodina.PocetakSkolskeGodine.Date;
                DateTime krajSkolskeGodine = skolskaGodina.KrajSkolskeGodine.Date;
                DateTime trenutniDatum = DateTime.Now.Date;

                int status = 0;

                //Ako nije pocela skolska godina status je 2
                if (trenutniDatum < pocetakSkolskeGodine)
                {
                    status = 2;//Status 2 oznacava da je upis ucenika u toku
                }
                //Ako je pocela skolska godina status je 3
                else if (trenutniDatum >= pocetakSkolskeGodine && trenutniDatum <= krajSkolskeGodine)
                {
                    status = 3;//Status 3 oznacava vanredan upis ucenika u odeljenje i tom prilikom ucenik dolazi na kraj dnevnika
                }
                else if (trenutniDatum > krajSkolskeGodine)
                {
                    status = 2;//Status 2 oznacava da je upis ucenika u toku jer je zavrsena skolska godina
                               // i vrsi se upis za sledecu skolsku godinu
                }



                var ucenik = _context.Ucenici.Where(u => u.UcenikID == ucenikId)
                                             .SingleOrDefault();

                if (ucenik != null)
                {

                    //Dodeljuje se odeljenje sa statusom koji je izabran na formi.
                    var odeljenje = _context.Odeljenja
                        .Include("Status")
                        .SingleOrDefault(o => o.OznakaID == oznaka && o.Razred == razred && o.StatusID == status);

                    //Ukoliko odeljenje nije ni kreirano u bazi bez obzira na status,kreira se.
                    if (odeljenje == null)
                    {
                        //Ukoliko je kreirana nova skolska godina njen Id se dodeljuje odeljenju
                        if (skolskaGodina.PocetakSkolskeGodine.Year == DateTime.Now.Year)
                        {
                            odeljenje = new Odeljenje()
                            {
                                OznakaID = oznaka,
                                Razred = razred,
                                PocetakSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznaka, _context),
                                StatusID = 2,
                                KrajSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznaka, _context) + 1,
                                SkolskaGodinaId = skolskaGodina.SkolskaGodinaId
                            };
                        }
                        //Ukoliko nije kreirana nova skolska godina njen Id se dodeljuje odeljenju kada se bude kreirala
                        else
                        {
                            odeljenje = new Odeljenje()
                            {
                                OznakaID = oznaka,
                                Razred = razred,
                                PocetakSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznaka, _context),
                                StatusID = 2,
                                KrajSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznaka, _context) + 1
                            };
                        }


                        _context.Odeljenja.Add(odeljenje);
                        _context.SaveChanges();
                    }

                    else if (odeljenje.StatusID == 3)   //Status 3 = kreirano.
                    {
                        var poslednjiBrojUDnevniku = odeljenje.Ucenici.Max(u => u.BrojUDnevniku);

                        ucenik.BrojUDnevniku = poslednjiBrojUDnevniku + 1; //Ucenik se dodaje na kraj dnevnika.

                        ucenik.Odeljenje = odeljenje;

                        ucenik.GenerisiJedinstveniBroj();

                    }

                    ucenik.OdeljenjeId = odeljenje.Id;
                    _context.Ucenici.AddOrUpdate(ucenik);
                    _context.SaveChanges();
                }

                if (status == 3)
                {
                    /*Metoda se pozvia da proveri da li postoji odeljenje 
                      kojem nije promenjen status iz "Upis u toku" u "Kreirano odeljenje",
                      do ovoga moze doci ukoliko je neko odeljenje prvi put kreirano nakon 
                      pocetka skolske.
                    */
                    PromeniStatusOdeljenjima();
                }
            }
        }

        /// <summary>
        /// Metoda uklanja uceniku odeljenje
        /// </summary>
        /// <param name="ucenikId"></param>
        /// <returns></returns>
        private void IzbaciUcenikaIzOdeljenja(int ucenikId)
        {
            var ucenik = _context.Ucenici.Where(u => u.UcenikID == ucenikId)
                                         .SingleOrDefault();

            if (ucenik != null)
            {
                //Posto se ucenik izbacuje iz odeljenja, sledece vrednosti se postavljaju na NULL vrednost
                ucenik.OdeljenjeId = null;
                ucenik.JedinstveniBroj = null;
                ucenik.BrojUDnevniku = null;

                _context.Ucenici.AddOrUpdate(ucenik);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Metoda vrsi raspodelu ucenika u odeljenja
        /// <see cref="KreirajOdeljenjeViewModel"/>
        /// <see cref="NerasporedjenUcenikViewModel"/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Vraca KreirajOdeljenje View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        private ActionResult KreirajOdeljenje(KreirajOdeljenjeViewModel model)
        {
            //Ovaj deo koda ce se izvrsiti ukoliko dodje do greske prilikom prenosa parametara sa forme
            if (model.ListaRasporedjenihUcenika == null && model.Razred == 0 && model.Odeljenje == 0)
            {
                var viewModelGreska = new KreirajOdeljenjeViewModel
                {
                    ListaNerasporedjenihUcenika = _context.Ucenici.Where(u => u.OdeljenjeId == null)
                                             .Select(u => new NerasporedjenUcenikViewModel
                                             {
                                                 UcenikId = u.UcenikID,
                                                 Ime = u.Ime,
                                                 Prezime = u.Prezime,
                                                 JMBG = u.JMBG,
                                                 Smer = u.Smer.NazivSmera
                                             })
                                             .ToList(),

                    //Postavlja se vrednost "true" jer je doslo do greske i prekinuto izvrsavanje metode
                    NisuDodatiUceniciUOdeljenje = true

                };

                return View(viewModelGreska);

            }

            //Ovaj deo koda ce se izvrsiti kada se prosledi lista ucenika koji su rasporedjeni u odeljenje na formi
            if (model.ListaRasporedjenihUcenika != null && model.Razred > 0 && model.Odeljenje > 0)
            {
                //Provera da li postoji odeljenje
                var odeljenje = _context.Odeljenja.Where(o => o.Razred == model.Razred && o.OznakaID == model.Odeljenje && o.StatusID != 1)
                                                     .SingleOrDefault();


                List<Ucenik> listaUcenika = null;

                if (odeljenje != null)
                {
                    //Lista ucenika koji su vec upisani u odeljenje
                    listaUcenika = _context.Ucenici.Where(u => u.OdeljenjeId == odeljenje.Id)
                                                   .Select(u => u)
                                                   .ToList();
                }

                //Lista u koju se smestaju ID-evi ucenika koji su vec upisani u odeljenje
                List<int> listaRasporedjenihUcenikaBazaID = new List<int>();

                if (listaUcenika != null)
                {
                    foreach (var ucenik in listaUcenika)
                    {
                        if (ucenik.OdeljenjeId != null)
                        {
                            listaRasporedjenihUcenikaBazaID.Add(ucenik.UcenikID);
                        }
                    }
                }

                //Lista u koji se smestaju ID-evi ucenika koji su prosledjeni sa forme
                List<int> listaRasporedjenihUcenikaFormaID = new List<int>();

                foreach (var ucenik in model.ListaRasporedjenihUcenika)
                {
                    listaRasporedjenihUcenikaFormaID.Add(ucenik.UcenikId);
                }

                //Lista ucenika koji treba da se dodaju u odeljenje
                IEnumerable<int> listaUcenikaZaDodavanjeID = listaRasporedjenihUcenikaFormaID.Except(listaRasporedjenihUcenikaBazaID);


                if (listaUcenikaZaDodavanjeID != null && listaUcenikaZaDodavanjeID.Count() > 0)
                {
                    foreach (var id in listaUcenikaZaDodavanjeID)
                    {
                        DodajUcenikaUOdeljenje(id, model.Razred, model.Odeljenje);//Poziv metode za dodavanje ucenika u odeljenje
                    }
                }

                //Lista ucenika koji treba da se izbace iz odeljenja
                IEnumerable<int> listaUcenikaZaBrisanjeID = listaRasporedjenihUcenikaBazaID.Except(listaRasporedjenihUcenikaFormaID);

                if (listaUcenikaZaBrisanjeID != null && listaUcenikaZaBrisanjeID.Count() > 0)
                {
                    foreach (var id in listaUcenikaZaBrisanjeID)
                    {
                        IzbaciUcenikaIzOdeljenja(id);//Poziv metode za izbacivanje ucenika iz odeljenja

                    }
                }
            }
            //Ovaj deo koda ce se izvrsiti ukoliko se prosledi prazna lista ucenika sa forme
            //Prosledjuje se odeljenje i svi ucenici iz odeljenja ce biti izbaceni
            else if (model.ListaRasporedjenihUcenika == null && model.Razred > 0 && model.Odeljenje > 0)
            {
                //Provera postojanja odeljenja
                var odeljenje = _context.Odeljenja.Where(o => o.Razred == model.Razred && o.OznakaID == model.Odeljenje && o.StatusID != 1)
                                                     .SingleOrDefault();

                //Lista ucenika koji idu u prosledjeno odeljenje
                List<Ucenik> listaUcenika = null;

                if (odeljenje != null)
                {

                    listaUcenika = _context.Ucenici.Where(u => u.OdeljenjeId == odeljenje.Id)
                                                   .Select(u => u)
                                                   .ToList();
                }

                //Lista u koju se smestaju ID-evi ucenika koji su  upisani u odeljenje i koji treba da se izbace iz odeljenja
                List<int> listaUcenikaZaBrisanjeBazaID = new List<int>();

                if (listaUcenika != null)
                {
                    foreach (var ucenik in listaUcenika)
                    {
                        if (ucenik.OdeljenjeId != null)
                        {
                            listaUcenikaZaBrisanjeBazaID.Add(ucenik.UcenikID);
                        }
                    }
                }


                if (listaUcenikaZaBrisanjeBazaID != null && listaUcenikaZaBrisanjeBazaID.Count() > 0)
                {
                    foreach (var id in listaUcenikaZaBrisanjeBazaID)
                    {
                        IzbaciUcenikaIzOdeljenja(id);//Poziv metode za izbacivanje ucenika iz odeljenja

                    }
                }
            }

            //View model koji vraca preostale nerasporedjene ucenike, odeljenje i potvrdu da su ucenici uspesno smesteni u odeljenje
            var viewModel = new KreirajOdeljenjeViewModel
            {
                ListaNerasporedjenihUcenika = _context.Ucenici.Where(u => u.OdeljenjeId == null)
                                           .Select(u => new NerasporedjenUcenikViewModel
                                           {
                                               UcenikId = u.UcenikID,
                                               Ime = u.Ime,
                                               Prezime = u.Prezime,
                                               JMBG = u.JMBG,
                                               Smer = u.Smer.NazivSmera
                                           })
                                           .ToList(),

                //Potvrda da su ucenici smesteni u odeljenje
                DodatiUceniciUOdeljenje = true,
                Razred = model.Razred,
                Odeljenje = model.Odeljenje

            };

            return View(viewModel);
        }


        /// <summary>
        /// Metoda vrsi arhiviranje odeljenja.
        /// Metoda se poziva automatski kada je kraj skolske godine.
        /// </summary>
        /// <returns></returns>
        public void ArhivirajKreiranaOdeljenja()
        {
            //Odeljenja za arhiviranje
            var odeljenja = _context.Odeljenja.Where(o => o.StatusID == 3) //Status 3 je Kreirano
                                              .Select(o => o)
                                              .ToList();

            if (odeljenja != null && odeljenja.Count() > 0)
            {
                foreach (var odeljenje in odeljenja)
                {
                    //Objekat vec kreiranog odeljenja.
                    var tekuceKreiranoOdeljenje = _context.Odeljenja.SingleOrDefault(o => o.Id == odeljenje.Id);

                    //Ukoliko postoji,to odeljenje se arhivira i ucenici iz njega se premestaju u sledecu godinu status odeljenja u toku.
                    if (tekuceKreiranoOdeljenje != null)
                    {
                        ArhivirajOdeljenje(tekuceKreiranoOdeljenje);
                        PremestiUSledecuGodinu(tekuceKreiranoOdeljenje);
                    }
                }
            }
        }




        /// <summary>
        /// Metoda vrsi promenu statusa odeljenja.
        /// Metoda se poziva automatski kada je pocetak skolske godine.
        /// </summary>
        /// <returns></returns>
        public void PromeniStatusOdeljenjima()
        {
            //Odeljenja za promenu statusa
            var odeljenja = _context.Odeljenja.Where(o => o.StatusID == 2) //Status 2 je U toku
                                              .Select(o => o)
                                              .ToList();

            if (odeljenja != null && odeljenja.Count() > 0)
            {
                foreach (var odeljenje in odeljenja)
                {
                    //Tekuce odeljenje koje trenutno ima status u toku a treba se kreira.
                    var odeljenjeZaPromenuStatusa = _context.Odeljenja.SingleOrDefault(o => o.Id == odeljenje.Id);

                    if (odeljenjeZaPromenuStatusa != null)
                    {
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
                    }
                }
            }
        }
    }
}


