using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.ViewModel;
using eDnevnikDev.Helpers;
using System.Data.Entity.Migrations;

namespace eDnevnikDev.Controllers
{
    public class UpisSkolskeGodineController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public UpisSkolskeGodineController()
        {

        }

        public UpisSkolskeGodineController(ApplicationDbContext db)
        {
            _context = db;
        }

        // GET: UpisSkolskeGodine/Create
        public ActionResult Create()
        {
            return View(new UpisTromesecjaStringViewModel());
        }
        public ActionResult RokZaKreiranjeSkolskeGodine()
        {
            var model = new UpisTromesecjaStringViewModel
            {
                RokZaKreiranjeSkolskeGodine = true
            };

            return View("Create", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UpisTromesecjaStringViewModel srpskiDatumi)
        {
            UpisTromesecjaViewModel podaci = new UpisTromesecjaViewModel()
            {
                PrvoPocetak = KonverizjaDatuma.izSrpskogUAmericki(srpskiDatumi.PrvoPocetak),
                PrvoKraj = KonverizjaDatuma.izSrpskogUAmericki(srpskiDatumi.PrvoKraj),
                DrugoPocetak = KonverizjaDatuma.izSrpskogUAmericki(srpskiDatumi.DrugoPocetak),
                DrugoKraj = KonverizjaDatuma.izSrpskogUAmericki(srpskiDatumi.DrugoKraj),
                TrecePocetak = KonverizjaDatuma.izSrpskogUAmericki(srpskiDatumi.TrecePocetak),
                TreceKraj = KonverizjaDatuma.izSrpskogUAmericki(srpskiDatumi.TreceKraj),
                CetvrtoPocetak = KonverizjaDatuma.izSrpskogUAmericki(srpskiDatumi.CetvrtoPocetak),
                CetvrtoKraj = KonverizjaDatuma.izSrpskogUAmericki(srpskiDatumi.CetvrtoKraj),
                Poruka = srpskiDatumi.Poruka
            };
            
            var poruka = ProveraGodina(podaci);
            if (poruka != "")
            {
                srpskiDatumi.Poruka = poruka;
                return View(srpskiDatumi);
            }

            try
            {
                try
                {
                    var poslednjaSkolskaGodina = _context.SkolskaGodine.Single(x => x.PocetakSkolskeGodine.Year == DateTime.Now.Year);
                    if (poslednjaSkolskaGodina != null)
                    {
                        srpskiDatumi.Poruka = "Godina je vec upisana";
                        return View(srpskiDatumi);
                    }
                }
                catch (Exception)
                {
                    
                }

                //Pronalazi se stara skolska godina
                var staraSkolskaGodina = _context.SkolskaGodine.SingleOrDefault(s => s.Aktuelna == true);

                if(staraSkolskaGodina !=null)
                {
                    //Vrednost stare skolske godine se setuje na false jer se upisuje nova
                    staraSkolskaGodina.Aktuelna = false;
                    _context.SkolskaGodine.AddOrUpdate(staraSkolskaGodina);
                    _context.SaveChanges();
                }
              

                var skolskaGodina = new SkolskaGodina() { PocetakSkolskeGodine = podaci.PrvoPocetak, KrajSkolskeGodine = podaci.CetvrtoKraj, Aktuelna = true };
                _context.SkolskaGodine.Add(skolskaGodina);
                _context.SaveChanges();

                //uzimamo id od skolske godine da bi mogli da unesemo polugodista
                var skolskaGodinaId = _context.SkolskaGodine.Single(x => x.PocetakSkolskeGodine == podaci.PrvoPocetak).SkolskaGodinaId;

                var odeljenja = _context.Odeljenja.Where(o => o.SkolskaGodinaId == null && o.StatusID == 2)
                                                  .Select(o=>o)
                                                  .ToList();
                if(odeljenja!=null)
                {
                    //Dodeljuje se skolska godina odeljenima koja nemaju skolsku godinu
                    for (int i = 0; i < odeljenja.Count(); i++)
                    {
                        var odeljenje = odeljenja.ElementAt(i);
                        odeljenje.SkolskaGodinaId = skolskaGodina.SkolskaGodinaId;
                        _context.Odeljenja.AddOrUpdate(odeljenje);
                        _context.SaveChanges();

                    }


                }

                var prvoPolugodiste = new Polugodiste()
                {
                    PocetakPolugodista = podaci.PrvoPocetak,
                    KrajPolugodista = podaci.DrugoKraj,
                    SkolskaGodinaId = skolskaGodinaId,
                    TipPolugodista = 1
                };

                _context.Polugodista.Add(prvoPolugodiste);
                _context.SaveChanges();

                var drugoPolugodiste = new Polugodiste()
                {
                    PocetakPolugodista = podaci.TrecePocetak,
                    KrajPolugodista = podaci.CetvrtoKraj,
                    SkolskaGodinaId = skolskaGodinaId,
                    TipPolugodista = 2
                };

                _context.Polugodista.Add(drugoPolugodiste);
                _context.SaveChanges();

                // uzimamo id od polugodista da bi mogli da unesemo tromesecja
                var prvoPolugodisteId = _context.Polugodista.Single(x => x.PocetakPolugodista == podaci.PrvoPocetak).PolugodisteId;
                var drugoPolugodisteId = _context.Polugodista.Single(x => x.PocetakPolugodista == podaci.TrecePocetak).PolugodisteId;

                var prvoTromesecje = new Tromesecje()
                {
                    PolugodisteId = prvoPolugodisteId,
                    PocetakTromesecja = podaci.PrvoPocetak,
                    KrajTromesecja = podaci.PrvoKraj,
                    TipTromesecja = 1
                };

                _context.Tromesecja.Add(prvoTromesecje);
                _context.SaveChanges();

                var drugoTromesecje = new Tromesecje()
                {
                    PolugodisteId = prvoPolugodisteId,
                    PocetakTromesecja = podaci.DrugoPocetak,
                    KrajTromesecja = podaci.DrugoKraj,
                    TipTromesecja = 2
                };

                _context.Tromesecja.Add(drugoTromesecje);
                _context.SaveChanges();

                var treceTromesecje = new Tromesecje()
                {
                    PolugodisteId = drugoPolugodisteId,
                    PocetakTromesecja = podaci.TrecePocetak,
                    KrajTromesecja = podaci.TreceKraj,
                    TipTromesecja = 3
                };

                _context.Tromesecja.Add(treceTromesecje);
                _context.SaveChanges();

                var cetvrtoTromesecje = new Tromesecje()
                {
                    PolugodisteId = drugoPolugodisteId,
                    PocetakTromesecja = podaci.CetvrtoPocetak,
                    KrajTromesecja = podaci.CetvrtoKraj,
                    TipTromesecja = 4
                };

                _context.Tromesecja.Add(cetvrtoTromesecje);
                _context.SaveChanges();

            }
            catch (Exception)
            {
                srpskiDatumi.Poruka = "Greška pri upisu u bazu";
                return View(srpskiDatumi);
            }

            poruka = "uspesno";
            srpskiDatumi.Poruka = poruka;
            return View(srpskiDatumi);
        }

        //GET
        public ActionResult Edit()
        {
            SkolskaGodina skolskaGodina = _context.SkolskaGodine
                .SingleOrDefault(s => s.Aktuelna == true);

            Polugodiste prvoPolugodiste = _context.Polugodista
                .SingleOrDefault(p => p.SkolskaGodinaId == skolskaGodina.SkolskaGodinaId && p.TipPolugodista==1);

            Polugodiste drugoPolugodiste = _context.Polugodista
                .SingleOrDefault(p => p.SkolskaGodinaId == skolskaGodina.SkolskaGodinaId && p.TipPolugodista == 2);

            Tromesecje prvoTromesecje = _context.Tromesecja
                .SingleOrDefault(t => t.PolugodisteId == prvoPolugodiste.PolugodisteId && t.TipTromesecja == 1);

            Tromesecje drugoTromesecje = _context.Tromesecja
                .SingleOrDefault(t => t.PolugodisteId == prvoPolugodiste.PolugodisteId && t.TipTromesecja == 2);

            Tromesecje treceTromesecje = _context.Tromesecja
                .SingleOrDefault(t => t.PolugodisteId == drugoPolugodiste.PolugodisteId && t.TipTromesecja == 3);

            Tromesecje cetvrtoTromesecje = _context.Tromesecja
                .SingleOrDefault(t => t.PolugodisteId == drugoPolugodiste.PolugodisteId && t.TipTromesecja == 4);

            if (skolskaGodina==null)
            {
                return HttpNotFound();
            }

            UpisTromesecjaStringViewModel tromesecjeVM = new UpisTromesecjaStringViewModel()
            {
                PrvoPocetak = KonverizjaDatuma.izAmerickogUSrpski(prvoTromesecje.PocetakTromesecja),
                PrvoKraj = KonverizjaDatuma.izAmerickogUSrpski(prvoTromesecje.KrajTromesecja),
                DrugoPocetak = KonverizjaDatuma.izAmerickogUSrpski(drugoTromesecje.PocetakTromesecja),
                DrugoKraj = KonverizjaDatuma.izAmerickogUSrpski(drugoTromesecje.KrajTromesecja),
                TrecePocetak = KonverizjaDatuma.izAmerickogUSrpski(treceTromesecje.PocetakTromesecja),
                TreceKraj = KonverizjaDatuma.izAmerickogUSrpski(treceTromesecje.KrajTromesecja),
                CetvrtoPocetak = KonverizjaDatuma.izAmerickogUSrpski(cetvrtoTromesecje.PocetakTromesecja),
                CetvrtoKraj = KonverizjaDatuma.izAmerickogUSrpski(cetvrtoTromesecje.KrajTromesecja)
            };
                
            return View(tromesecjeVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(UpisTromesecjaStringViewModel tromesecja)
        {
            SkolskaGodina skolskaGodina = _context.SkolskaGodine
                .SingleOrDefault(s => s.Aktuelna == true);

            Polugodiste prvoPolugodiste = _context.Polugodista
                .SingleOrDefault(p => p.SkolskaGodinaId == skolskaGodina.SkolskaGodinaId && p.TipPolugodista == 1);

            Polugodiste drugoPolugodiste = _context.Polugodista
                .SingleOrDefault(p => p.SkolskaGodinaId == skolskaGodina.SkolskaGodinaId && p.TipPolugodista == 2);

            Tromesecje prvoTromesecje = _context.Tromesecja
                .SingleOrDefault(t => t.PolugodisteId == prvoPolugodiste.PolugodisteId && t.TipTromesecja == 1);

            Tromesecje drugoTromesecje = _context.Tromesecja
                .SingleOrDefault(t => t.PolugodisteId == prvoPolugodiste.PolugodisteId && t.TipTromesecja == 2);

            Tromesecje treceTromesecje = _context.Tromesecja
                .SingleOrDefault(t => t.PolugodisteId == drugoPolugodiste.PolugodisteId && t.TipTromesecja == 3);

            Tromesecje cetvrtoTromesecje = _context.Tromesecja
                .SingleOrDefault(t => t.PolugodisteId == drugoPolugodiste.PolugodisteId && t.TipTromesecja == 4);

            if (ModelState.IsValid)
            {
                skolskaGodina.PocetakSkolskeGodine = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.PrvoPocetak);
                skolskaGodina.KrajSkolskeGodine = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.CetvrtoKraj);

                prvoPolugodiste.PocetakPolugodista= KonverizjaDatuma.izSrpskogUAmericki(tromesecja.PrvoPocetak);
                prvoPolugodiste.KrajPolugodista = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.DrugoKraj);
                drugoPolugodiste.PocetakPolugodista = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.TrecePocetak);
                drugoPolugodiste.KrajPolugodista = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.CetvrtoKraj);

                prvoTromesecje.PocetakTromesecja= KonverizjaDatuma.izSrpskogUAmericki(tromesecja.PrvoPocetak);
                prvoTromesecje.KrajTromesecja = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.PrvoKraj);
                drugoTromesecje.PocetakTromesecja = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.DrugoPocetak);
                drugoTromesecje.KrajTromesecja = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.DrugoKraj);
                treceTromesecje.PocetakTromesecja = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.TrecePocetak);
                treceTromesecje.KrajTromesecja = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.TreceKraj);
                cetvrtoTromesecje.PocetakTromesecja = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.CetvrtoPocetak);
                cetvrtoTromesecje.KrajTromesecja = KonverizjaDatuma.izSrpskogUAmericki(tromesecja.CetvrtoKraj);

            }

            try
            {
                _context.SkolskaGodine.AddOrUpdate(skolskaGodina);
                _context.SaveChanges();

                _context.Polugodista.AddOrUpdate(prvoPolugodiste);
                _context.SaveChanges();

                _context.Polugodista.AddOrUpdate(drugoPolugodiste);
                _context.SaveChanges();

                _context.Tromesecja.AddOrUpdate(prvoTromesecje);
                _context.SaveChanges();

                _context.Tromesecja.AddOrUpdate(drugoTromesecje);
                _context.SaveChanges();

                _context.Tromesecja.AddOrUpdate(treceTromesecje);
                _context.SaveChanges();

                _context.Tromesecja.AddOrUpdate(cetvrtoTromesecje);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            string poruka = "uspesno";
            tromesecja.Poruka = poruka;
            return View(tromesecja);
        }

        public string ProveraRedosledaUpisanihGodina(UpisTromesecjaViewModel tromesecja)
        {
            //provera da li je redosled datuma dobar
            if (tromesecja.PrvoPocetak < tromesecja.PrvoKraj
                && tromesecja.PrvoKraj < tromesecja.DrugoPocetak
                && tromesecja.DrugoPocetak < tromesecja.DrugoKraj
                && tromesecja.DrugoKraj < tromesecja.TrecePocetak
                && tromesecja.TrecePocetak < tromesecja.TreceKraj
                && tromesecja.TreceKraj < tromesecja.CetvrtoPocetak
                && tromesecja.CetvrtoPocetak < tromesecja.CetvrtoKraj)
            {

            }
            else
            {
                return "Datumi nisu lepo raspoređeni";
            }

            return "";
        }

        public string ProveraGodina(UpisTromesecjaViewModel godine)
        {
            var godina = DateTime.Now.Year;

            if (godine.PrvoPocetak.Year == DateTime.Now.Year && godine.CetvrtoKraj.Year == ++godina)
            {

            }
            else
            {
                return "Početna godina mora biti trenutna godina, a krajnja mora biti sledeća";
            }

            ProveraRedosledaUpisanihGodina(godine);

            return "";
        }
    }
}