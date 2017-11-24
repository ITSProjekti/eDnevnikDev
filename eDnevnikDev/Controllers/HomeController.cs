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
            AdminIndexViewModel podaci = new AdminIndexViewModel()
            {
                Decaci = 156,
                Devojcice = 259,
                PocetakPrvogPolugodista = KonverizjaDatuma.izAmerickogUSrpski(_context.Polugodista.Single(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 1).PocetakPolugodista),
                KrajPrvogPolugodista = KonverizjaDatuma.izAmerickogUSrpski(_context.Polugodista.Single(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 1).KrajPolugodista),
                PocetakDrugogPolugodista = KonverizjaDatuma.izAmerickogUSrpski(_context.Polugodista.Single(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 2).PocetakPolugodista),
                KrajDrugogPolugodista = KonverizjaDatuma.izAmerickogUSrpski(_context.Polugodista.Single(x => x.SkolskaGodinaId == skolskaGodinaId && x.TipPolugodista == 2).KrajPolugodista),
                UkupanBrojUcenika = _context.Ucenici.Where(x => x.StatusUcenika.StatusUcenikaId == 1).Count()
            };


            return View(podaci);
        }
    }
}