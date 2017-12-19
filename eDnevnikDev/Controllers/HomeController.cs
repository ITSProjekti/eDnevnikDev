using eDnevnikDev.Helpers;
using eDnevnikDev.Models;
using eDnevnikDev.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDnevnikDev.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public HomeController()
        {

        }

        public HomeController(ApplicationDbContext db)
        {
            this._context = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
        [Authorize(Roles = "Administrator")]
        public ActionResult IndexAdmin()
        {
            int skolskaGodinaId = _context.SkolskaGodine.OrderByDescending(x => x.SkolskaGodinaId).First().SkolskaGodinaId;
            int prvoPolugodisteId = _context.Polugodista.SingleOrDefault(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 1).PolugodisteId;
            int drugoPolugodisteId = _context.Polugodista.SingleOrDefault(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 2).PolugodisteId;

            AdminIndexViewModel podaci = new AdminIndexViewModel()
            {
                Decaci = _context
                         .Ucenici
                         .Where(x => x.StatusUcenika.StatusUcenikaId == 1 && x.Pol.PolId == 2)
                         .Count(),
                Devojcice = _context
                            .Ucenici
                            .Where(x => x.StatusUcenika.StatusUcenikaId == 1 && x.Pol.PolId == 1)
                            .Count(),
                PocetakPrvogPolugodista = KonverizjaDatuma.izAmerickogUSrpski(_context.Polugodista.SingleOrDefault(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 1).PocetakPolugodista),
                KrajPrvogPolugodista = KonverizjaDatuma.izAmerickogUSrpski(_context.Polugodista.SingleOrDefault(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 1).KrajPolugodista),
                PocetakDrugogPolugodista = KonverizjaDatuma.izAmerickogUSrpski(_context.Polugodista.SingleOrDefault(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 2).PocetakPolugodista),
                KrajDrugogPolugodista = KonverizjaDatuma.izAmerickogUSrpski(_context.Polugodista.SingleOrDefault(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 2).KrajPolugodista),
                UkupanBrojUcenika = _context.Ucenici.Where(x => x.StatusUcenika.StatusUcenikaId == 1).Count(),
                PocetakPrvogTromesecja = KonverizjaDatuma.izAmerickogUSrpski(_context.Tromesecja.SingleOrDefault(x => x.PolugodisteId == prvoPolugodisteId && x.TipTromesecja == 1).PocetakTromesecja),
                KrajPrvogTromesecja = KonverizjaDatuma.izAmerickogUSrpski(_context.Tromesecja.SingleOrDefault(x => x.PolugodisteId == prvoPolugodisteId && x.TipTromesecja == 1).KrajTromesecja),
                PocetakDrugogTromesecja = KonverizjaDatuma.izAmerickogUSrpski(_context.Tromesecja.SingleOrDefault(x => x.PolugodisteId == prvoPolugodisteId && x.TipTromesecja == 2).PocetakTromesecja),
                KrajDrugogTromesecja = KonverizjaDatuma.izAmerickogUSrpski(_context.Tromesecja.SingleOrDefault(x => x.PolugodisteId == prvoPolugodisteId && x.TipTromesecja == 2).KrajTromesecja),
                PocetakTrecegTromesecja = KonverizjaDatuma.izAmerickogUSrpski(_context.Tromesecja.SingleOrDefault(x => x.PolugodisteId == drugoPolugodisteId && x.TipTromesecja == 3).PocetakTromesecja),
                KrajTrecegTromesecja = KonverizjaDatuma.izAmerickogUSrpski(_context.Tromesecja.SingleOrDefault(x => x.PolugodisteId == drugoPolugodisteId && x.TipTromesecja == 3).KrajTromesecja),
                PocetakCetvrtogTromesecja = KonverizjaDatuma.izAmerickogUSrpski(_context.Tromesecja.SingleOrDefault(x => x.PolugodisteId == drugoPolugodisteId && x.TipTromesecja == 4).PocetakTromesecja),
                KrajCetvrtogTromesecja = KonverizjaDatuma.izAmerickogUSrpski(_context.Tromesecja.SingleOrDefault(x => x.PolugodisteId == drugoPolugodisteId && x.TipTromesecja == 4).KrajTromesecja),
            };


            return View(podaci);
        }
    }
}