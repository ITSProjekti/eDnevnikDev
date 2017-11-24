using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.ViewModel;
using eDnevnikDev.Helpers;

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

                var skolskaGodina = new SkolskaGodina() { PocetakSkolskeGodine = podaci.PrvoPocetak, KrajSkolskeGodine = podaci.CetvrtoKraj, Aktuelna = true };
                _context.SkolskaGodine.Add(skolskaGodina);
                _context.SaveChanges();

                //uzimamo id od skolske godine da bi mogli da unesemo polugodista
                var skolskaGodinaId = _context.SkolskaGodine.Single(x => x.PocetakSkolskeGodine == podaci.PrvoPocetak).SkolskaGodinaId;

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
            catch (Exception e)
            {
                srpskiDatumi.Poruka = "Greška pri upisu u bazu";
                return View(srpskiDatumi);
            }

            poruka = "uspesno";
            srpskiDatumi.Poruka = poruka;
            return View(srpskiDatumi);
        }

        private string ProveraGodina(UpisTromesecjaViewModel godine)
        {
            var godina = DateTime.Now.Year;

            if (godine.PrvoPocetak.Year == DateTime.Now.Year && godine.CetvrtoKraj.Year == ++godina)
            {

            }
            else
            {
                return "Početna godina mora biti trenutna godina, a krajnja mora biti sledeća";
            }

            //provera da li je redosled datuma dobar
            if (godine.PrvoPocetak < godine.PrvoKraj
                && godine.PrvoKraj < godine.DrugoPocetak
                && godine.DrugoPocetak < godine.DrugoKraj
                && godine.DrugoKraj < godine.TrecePocetak
                && godine.TrecePocetak < godine.TreceKraj
                && godine.TreceKraj < godine.CetvrtoPocetak
                && godine.CetvrtoPocetak < godine.CetvrtoKraj)
            {

            }
            else
            {
                return "Datumi nisu lepo rasporedjeni";
            }




            return "";


        }
        


    }
}