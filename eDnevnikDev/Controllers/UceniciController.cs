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
        /// Cuvamo Ucenika. Provera da li je model ucenika validan sa unosa. Provera da li je slika validna i ubacivanje.
        /// Ubacivanje ucenika u odeljenje sa odredjenim statusom. status 2 = u toku, status 3 = kreirano.
        /// Kreiranje novog odeljenja u bazi ukoliko jos uvek ne postoji.
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
                    Smerovi = _context.Smerovi.Include("Oznake").OrderBy(s => s.Trajanje).ToList()
                };

                return View("Dodaj", podaci);
            }

            var ucenik = ucenikVM.Ucenik;
            var oznaka = ucenikVM.Oznaka;
            var razred = ucenik.Razred;

            

            //var file = ucenikVM.File;

            //Provera slike i vracanje na view ukoliko je NEVALIDNA. ne ubacuje ovde. save = line 164.
            //if (file != null)
            //{
            //    string pic = System.IO.Path.GetExtension(file.FileName);

            //    if (pic != ".jpg" && pic != ".jpeg" && pic != ".png")
            //    {
            //        ModelState.AddModelError("File", "Neispravna slika");

            //        var podaci = new UcenikViewModel
            //        {
            //            Ucenik = ucenikVM.Ucenik,
            //            Gradovi = _context.Gradovi.OrderBy(g => g.Naziv).ToList(),
            //            Smerovi = _context.Smerovi.Include("Odeljenja").OrderBy(s => s.Trajanje).ToList()
            //        };

            //        return View("Dodaj", podaci);
            //    }
            //}

            //Dodeljuje se odeljenje sa statusom koji je izabran na formi.
            var odeljenje = _context.Odeljenja
                .Include("Status")
                .SingleOrDefault(o => o.OznakaID == oznaka && o.Razred == razred && o.StatusID == ucenikVM.SmestiUNovoOdeljenje);

            //Ukoliko odeljenje nije ni kreirano u bazi bez obzira na status,kreira se.
            if (odeljenje == null)
            {
                odeljenje = new Odeljenje()
                {
                    OznakaID = oznaka,
                    Razred = razred,
                    PocetakSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznaka,_context),
                    StatusID = 2, //Status 2 = u toku.
                    KrajSkolskeGodine = Odeljenje.SledecaSkolskaGodina(razred, oznaka,_context) + 1
                };
                
                _context.Odeljenja.Add(odeljenje);
                _context.SaveChanges();  
            }

            else if(odeljenje.StatusID == 3)   //Status 3 = kreirano.
            {
                var poslednjiBrojUDnevniku = odeljenje.Ucenici.Max(u => u.BrojUDnevniku);

                ucenik.BrojUDnevniku = poslednjiBrojUDnevniku + 1; //Ucenik se dodaje na kraj dnevnika.

                ucenik.Odeljenje = odeljenje;

                ucenik.GenerisiJedinstveniBroj();

            }

            ucenik.OdeljenjeId = odeljenje.Id;

            ucenik.RedniBroj = GenerisiRedniBrojUcenika(ucenik);

            string username = ucenik.Ime.ToLower() + ucenik.RedniBroj;

            await RegistracijaUcenika(username, ucenik.Ime, ucenik.RedniBroj);

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

            //Ubacivanje slike u fotografija property ucenika i save.
            //if (file != null)
            //{
            //    var id = Ucenik.GetMd5Hash(ucenik.JMBG);

            //    string pic = System.IO.Path.GetExtension(file.FileName);

            //    file.SaveAs(HttpContext.Server.MapPath("~/slike/") + id + pic);

            //    ucenik.Fotografija = id + pic;

            //    _context.SaveChanges();

            //}

            return RedirectToAction("Index", "Ucenici");
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
                
            return Json(lista,JsonRequestBehavior.AllowGet);
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

            //Vracaju se ucenici koji pohadjaju bilo koje odeljenje, ali da se pocetak godine podudara 
            //sa ucenikom koji se upisuje
            var ucenici = _context.Ucenici
                .Where(u => u.Odeljenje.PocetakSkolskeGodine == ucenik.Odeljenje.PocetakSkolskeGodine)
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
                if(pom.ToString().Length==1)
                {
                    redniBroj = "0" + pom + ucenik.Odeljenje.PocetakSkolskeGodine % 100;
                }
                else
                {
                    redniBroj = pom + "" + ucenik.Odeljenje.PocetakSkolskeGodine % 100;
                }
            }
            //Ukoliko takvi ucenici ne postoje, kreira se prvi ucenik sa rednim brojem koji krece od 01
            else
            {
                redniBroj = "01" + ucenik.Odeljenje.PocetakSkolskeGodine % 100;
            }

            return redniBroj;
        }

        public async Task RegistracijaUcenika(string username, string ime, string brojIndeksa)
        {
            var user = new ApplicationUser { UserName = username, Email = username+"@gmail.com" };
            var result = await UserManager.CreateAsync(user, ime + "." + brojIndeksa);

        }
    }
}