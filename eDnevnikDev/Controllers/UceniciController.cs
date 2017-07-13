using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using eDnevnikDev.ViewModel;

namespace eDnevnikDev.Controllers
{
    /// <summary>
    /// Kontroler za upravljanje učenicima.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class UceniciController : Controller
    {
        ApplicationDbContext _context;
        /// <summary>
        /// Inicijalizuje instancu klase <see cref="UceniciController"/> class.
        /// Konstruktor kontrolera.
        /// </summary>
        public UceniciController()
        {
            _context = new ApplicationDbContext();

        }
        public UceniciController(ApplicationDbContext context)
        {
            this._context = context;

        }


        /// <summary>
        /// Destruktor za objekat klase aplicationDbContext.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Ucenici
        /// <summary>
        /// Indexes this instance.
        /// Pravi instancu liste ucenika i prosledjuje je u view;
        /// Test name=UceniciController_Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            IEnumerable<Ucenik> ListaUcenika = _context.Ucenici.ToList();
            return View("Index", ListaUcenika);
        }
        /// <summary>
        /// 
        /// Funkcija koja vraca view dodaj.
        /// </summary>
        /// <returns></returns>
        public ActionResult Dodaj()
        {
            var ucenikVM = new UcenikViewModel
            {
                Ucenik = new Ucenik { Ime = "Firas", Prezime = "Aburas", Adresa = "Adresa 1", BrojTelefonaRoditelja = "+381-11/1234567", ImeMajke = "Majka", ImeOca = "Otac", PrezimeMajke = "Prezime", PrezimeOca = "Prezime", JMBG = "1708993730202", MestoPrebivalista = "Beograd", MestoRodjenjaId = 3, DatumRodjenja = new DateTime(2011, 12, 5) },
                Gradovi = _context.Gradovi.OrderBy(g => g.Naziv).ToList(),
                Smerovi = _context.Smerovi.Include("Oznake").OrderBy(s => s.Trajanje).ToList()

            };


            return View("Dodaj", ucenikVM);
        }

        /// <summary>
        /// Cuvamo Ucenika
        /// </summary>
        /// <param name="ucenikVM">The ucenik vm.</param>
        /// <returns>Vraca nas na Index stranicu Ucenika</returns>
        [HttpPost]
        public ActionResult Sacuvaj(UcenikViewModel ucenikVM)
        {

            if (!ModelState.IsValid)
            {
                var podaci = new UcenikViewModel
                {
                    Ucenik = ucenikVM.Ucenik,
                    Gradovi = _context.Gradovi.OrderBy(g => g.Naziv).ToList(),
                    Smerovi = _context.Smerovi.Include("Odeljenja").OrderBy(s => s.Trajanje).ToList()
                };

                return View("Dodaj", podaci);
            }

            var ucenik = ucenikVM.Ucenik;
            var oznaka = ucenikVM.Oznaka;
            var razred = ucenik.Razred;

            var file = ucenikVM.File;

            if (file != null)
            {
                string pic = System.IO.Path.GetExtension(file.FileName);

                if (pic != ".jpg" && pic != ".jpeg" && pic != ".png")
                {
                    ModelState.AddModelError("File", "Neispravna slika");

                    var podaci = new UcenikViewModel
                    {
                        Ucenik = ucenikVM.Ucenik,
                        Gradovi = _context.Gradovi.OrderBy(g => g.Naziv).ToList(),
                        Smerovi = _context.Smerovi.Include("Odeljenja").OrderBy(s => s.Trajanje).ToList()
                    };

                    return View("Dodaj", podaci);
                }
            }


            var odeljenje = _context.Odeljenja
                .Include("Status")
                .SingleOrDefault(o => o.OznakaID == oznaka && o.Razred == razred && o.StatusID == ucenikVM.SmestiUNovoOdeljenje);

            if (odeljenje == null)
            {
                odeljenje = new Odeljenje()
                {
                    OznakaID = oznaka,
                    Razred = razred,
                    PocetakSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznaka,_context),
                    StatusID = 2,
                    KrajSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznaka,_context) + 1
                };
                
                _context.Odeljenja.Add(odeljenje);
                _context.SaveChanges();  
            }
            else if(odeljenje.StatusID == 3)
            {
                var poslednjiBrojUDnevniku = odeljenje.Ucenici.Max(u => u.BrojUDnevniku);

                ucenik.BrojUDnevniku = poslednjiBrojUDnevniku + 1;

                ucenik.Odeljenje = odeljenje;

                ucenik.GenerisiJedinstveniBroj();

            }

            ucenik.OdeljenjeId = odeljenje.Id;
            _context.Ucenici.Add(ucenik);
            _context.SaveChanges();


            if (file != null)
            {
                var id = Ucenik.GetMd5Hash(ucenik.JMBG);

                string pic = System.IO.Path.GetExtension(file.FileName);

                file.SaveAs(HttpContext.Server.MapPath("~/slike/") + id + pic);

                ucenik.Fotografija = id + pic;

                _context.SaveChanges();

            }

            return RedirectToAction("Index", "Ucenici");
        }

        public JsonResult DaLiPostojiKreirano(int razred,int oznaka)
        {
           var odeljenje =  _context.Odeljenja
                            .Include("Status")
                             .SingleOrDefault(o => o.OznakaID == oznaka && o.Razred == razred && o.StatusID == 3);
           return odeljenje == null ? Json(new { Kreirano = false }, JsonRequestBehavior.AllowGet) : Json(new { Kreirano = true }, JsonRequestBehavior.AllowGet);

        }
    }
}