using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using eDnevnikDev.ViewModel;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net;
using System.Data.Entity.Migrations;

namespace eDnevnikDev.Controllers
{
    /// <summary>
    /// Kontroler za upravljanje učenicima.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class UceniciController : Controller
    {
        ApplicationDbContext _context;
        ApplicationUserManager _userManager;

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

        public UceniciController(ApplicationDbContext _context, ApplicationUserManager userManager)
        {
            this._context = _context;
            this.UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {

            IEnumerable<Ucenik> ListaUcenika = _context.Ucenici.ToList();
            return View("Index", ListaUcenika);
        }
        /// <summary>
        ///
        /// Funkcija koja vraca view dodaj. <see cref="UcenikViewModel"/> 
        /// TO BE TESTED
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        public ActionResult Dodaj()
        {
            var ucenikVM = new UcenikViewModel
            {
                Gradovi = _context.Gradovi.OrderBy(g => g.Naziv).ToList(),
                Smerovi = _context.Smerovi.Include("Oznake").OrderBy(s => s.Trajanje).ToList(),
                Polovi = _context.Polovi.ToList()
            };

            return View("Dodaj", ucenikVM);
        }

        /// <summary>
        /// Cuvamo Ucenika. Provera da li je model ucenika validan sa unosa. Provera da li je slika validna i ubacivanje.
        /// Test name= UceniciController_Sacuvaj
        /// Prilikom registracije korisnika(ucenika) kreira se ucenik u tabeli Ucenik kao i korisnik
        /// u tabeli AspNetUsers
        /// Prilikom registracije prosledjuje se njegovo ime i redni broj zato sto se
        /// Username korisnika generise na sledeci nacin (ucenik1017), a Password (Ucenik.1017)
        /// <see cref="Ucenik"/>
        /// <see cref="UcenikViewModel"/>
        /// </summary>
        /// <param name="ucenikVM">The ucenik vm.</param>
        /// <returns>Vraca nas na Index stranicu Ucenika</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Sacuvaj(UcenikViewModel ucenikVM, HttpPostedFileBase upload)
        {
            //Proverava postajanje istog JMBG
            if (_context.Ucenici.Where(x => x.JMBG == ucenikVM.Ucenik.JMBG).Any())
                ModelState.AddModelError("Ucenik.JMBG", "JMBG vec postoji u bazi!");

            if (!ModelState.IsValid)
            {
                var podaci = new UcenikViewModel
                {
                    Ucenik = ucenikVM.Ucenik,
                    Gradovi = _context.Gradovi.OrderBy(g => g.Naziv).ToList(),
                    Smerovi = _context.Smerovi.Include("Oznake").OrderBy(s => s.Trajanje).ToList(),
                    Polovi = _context.Polovi.ToList()
                };

                return View("Dodaj", podaci);
            }

            var ucenik = ucenikVM.Ucenik;
            var oznaka = ucenikVM.Oznaka;
            var razred = ucenik.Razred;

            ucenik.RedniBroj = GenerisiRedniBrojUcenika(ucenik);
            ucenik.DatumUnosa = DateTime.Now;

            string username = ucenik.Ime.Replace(" ", String.Empty) + ucenik.RedniBroj;
            await RegistracijaUcenika(username, VratiImeUcenikaSaPrvimVelikimSlovom(ucenik.Ime), ucenik.RedniBroj);
            var idUserUcenik = _context.Users.SingleOrDefault(x => x.UserName == username).Id;
            //await UserManager.AddToRoleAsync(idUserUcenik, "Ucenik");
            ucenik.UserUcenikId = idUserUcenik;

            //Dodavanje fotografije za osobu
            if (upload != null && upload.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    ucenik.Fotografija = reader.ReadBytes(upload.ContentLength);
                }
            }

            _context.Ucenici.Add(ucenik);
            _context.SaveChanges();

            return RedirectToAction("Index", "Ucenici");
        }

        //GET
        /// <summary>
        /// Metoda koja vraca view izmeni za ucenika
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        public ActionResult Izmeni(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ucenik ucenik = _context.Ucenici.Find(id);

            if (ucenik == null)
            {
                return HttpNotFound();
            }

            UcenikViewModel ucenikVM = new UcenikViewModel()
            {
                Ucenik = ucenik,
                Gradovi = _context.Gradovi.OrderBy(g => g.Naziv).ToList(),
                Smerovi = _context.Smerovi.Include("Oznake").OrderBy(s => s.Trajanje).ToList(),
                Polovi = _context.Polovi.ToList()
            };

            return View(ucenikVM);
        }

        //POST
        /// <summary>
        /// Metoda vrsi izmenu ucenika
        /// </summary>
        /// <param name="ucenikVM">The ucenik vm.</param>
        /// <param name="upload">The upload.</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Izmeni(UcenikViewModel ucenikVM, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        ucenikVM.Ucenik.Fotografija = reader.ReadBytes(upload.ContentLength);
                    }
                }

                Ucenik ucenik = new Ucenik()
                {
                    UcenikID = ucenikVM.Ucenik.UcenikID,
                    Ime = ucenikVM.Ucenik.Ime,
                    Prezime = ucenikVM.Ucenik.Prezime,
                    ImeOca = ucenikVM.Ucenik.ImeOca,
                    PrezimeOca = ucenikVM.Ucenik.PrezimeOca,
                    ImeMajke = ucenikVM.Ucenik.ImeMajke,
                    PrezimeMajke = ucenikVM.Ucenik.PrezimeMajke,
                    JMBG = ucenikVM.Ucenik.JMBG,
                    Adresa = ucenikVM.Ucenik.Adresa,
                    MestoPrebivalista = ucenikVM.Ucenik.MestoPrebivalista,
                    BrojTelefonaRoditelja = ucenikVM.Ucenik.BrojTelefonaRoditelja,
                    MestoRodjenjaId = ucenikVM.Ucenik.MestoRodjenjaId,
                    Vanredan = ucenikVM.Ucenik.Vanredan,
                    RedniBroj = ucenikVM.Ucenik.RedniBroj,
                    PromenaLozinke = ucenikVM.Ucenik.PromenaLozinke,
                    SmerID = ucenikVM.Ucenik.SmerID,
                    OdeljenjeId = ucenikVM.Ucenik.OdeljenjeId,
                    Razred = ucenikVM.Ucenik.Razred,
                    DatumRodjenja = ucenikVM.Ucenik.DatumRodjenja,
                    JedinstveniBroj = ucenikVM.Ucenik.JedinstveniBroj,
                    Fotografija = ucenikVM.Ucenik.Fotografija,
                    BrojUDnevniku = ucenikVM.Ucenik.BrojUDnevniku,
                    UserUcenikId = ucenikVM.Ucenik.UserUcenikId,
                    DatumUnosa = ucenikVM.Ucenik.DatumUnosa,
                    PolId = ucenikVM.Ucenik.PolId
                };

                try
                {
                    _context.Ucenici.AddOrUpdate(ucenik);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }
            return RedirectToAction("Index");

        }

        /// <summary>
        /// Na formi za unos ucenika menja se opcija da li se ucenik dodaje u aktuelno odeljenje ili u odeljenje za sledecu skolsku godinu.
        /// </summary>
        /// <param name="razred"></param>
        /// <param name="oznaka"></param>
        /// <returns></returns>
        public JsonResult DaLiPostojiKreirano(int razred, int oznaka)
        {
            //Uzima se odeljenje koje je kreirano.ako ga ima.
            var odeljenje = _context.Odeljenja
                             .Include("Status")
                              .SingleOrDefault(o => o.OznakaID == oznaka && o.Razred == razred && o.StatusID == 3);
            //Ukoliko je null vraca se false, ako jeste vraca se true.
            return odeljenje == null ? Json(new { Kreirano = false }, JsonRequestBehavior.AllowGet) : Json(new { Kreirano = true }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult test()
        {
            var lista = new List<Smer>();
            var temp = _context.Smerovi.Include("Oznake");
            foreach (var item in temp)
            {
                lista.Add(item);
            }

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Kontroler generiše redni broj učenika u bazi
        /// Redni broj se sastoji iz dva dela (npr 1518)
        /// 15 predstavlja redni broj po kojem se učenik upisao
        /// 18 predstavlja godinu kada se upisao
        /// Svake školske godine broj učenika koji se upisuju kreće od 01 i ide redom sve do sledeće
        /// školske godine kada će ponovo kretati od 01
        /// </summary>
        /// <param name="ucenik">The ucenik.</param>
        /// <returns>redni broj učenika</returns>
        public string GenerisiRedniBrojUcenika(Ucenik ucenik)
        {
            string redniBroj;

            //Vracaju se svi ucenici kojima je godina unosa u bazu jednaka danasnjoj godini
            var ucenici = _context.Ucenici
                .Where(u => u.DatumUnosa.Year == DateTime.Now.Year)
                .Select(u => u);


            //Ukoliko postoje takvi ucenici
            if (ucenici.Count() > 0)
            {
                var maxRedniBroj = ucenici
                    .Select(u => u.RedniBroj).Max();

                //Zadnje dve cifre koje predstavljaju godinu se uklanjaju da bismo povecali redni broj za 1
                //bez godine
                int pom = int.Parse(maxRedniBroj);
                pom /= 100;
                pom++;

                //Ako je jednocifreni broj dodaje se 0 ispred
                if (pom.ToString().Length == 1)
                {
                    redniBroj = "0" + pom + DateTime.Now.Year % 100;
                }
                else
                {
                    redniBroj = pom + "" + DateTime.Now.Year % 100;
                }
            }
            //Ukoliko takvi ucenici ne postoje, kreira se prvi ucenik sa rednim brojem koji krece od 01
            else
            {
                redniBroj = "01" + DateTime.Now.Year % 100;
            }
            return redniBroj;
        }

        /// <summary>
        /// Metoda sluzi za registraciju ucenika
        /// Password se kreira tako sto se na ime ucenika doda tacka i redni broj
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="ime">The IME.</param>
        /// <param name="brojIndeksa">The broj indeksa.</param>
        /// <returns></returns>
        public async Task RegistracijaUcenika(string username, string ime, string brojIndeksa)
        {
            var user = new ApplicationUser { UserName = username.ToLower(), Email = username + "@gmail.com" };
            var result = await UserManager.CreateAsync(user, ime + "." + brojIndeksa);

        }

        /// <summary>
        /// Vraca se ime ucenika tako da prvo slovo bude veliko
        /// </summary>
        /// <param name="prezime">The prezime.</param>
        /// <returns></returns>
        public string VratiImeUcenikaSaPrvimVelikimSlovom(string ime)
        {
            switch (ime)
            {
                case null: throw new ArgumentNullException(nameof(ime));
                case "": throw new ArgumentException($"{nameof(ime)} ne moze da bude prazno", nameof(ime));
                default: return ime.First().ToString().ToUpper().Replace(" ", string.Empty) + ime.Substring(1).Replace(" ", string.Empty);
            }
        }
    }
}