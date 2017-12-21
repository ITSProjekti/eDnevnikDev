using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using eDnevnikDev.ViewModel;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Net;
using System.Data.Entity.Migrations;

namespace eDnevnikDev.Controllers
{
    public class ProfesoriController : Controller
    {
        ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfesoriController"/> class.
        /// </summary>
        public ProfesoriController()
        {
            _context = new ApplicationDbContext();
        }

        public ProfesoriController(ApplicationDbContext _context)
        {
            this._context = _context;
        }


        public ProfesoriController(ApplicationDbContext _context, ApplicationUserManager userManager)
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
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Profesori
        /// <summary>
        /// Uzima se lista profesora iz Baze. Test name=ProfesoriController_Index
        /// </summary>
        /// <returns>Vraca listu u View</returns>
        [Authorize(Roles = "Administrator, Editor")]
        public ActionResult Index(bool? dodatProfesor, bool? izmenjenProfesor)
        {
            if (dodatProfesor != null)
            {
                var model = new ListaProfesoraViewModel
                {
                    ListaProfesora = _context.Profesori.ToList(),
                    DodatProfesor = (bool)dodatProfesor
                };

                return View(model);
            }

            if (izmenjenProfesor != null)
            {
                var model = new ListaProfesoraViewModel
                {
                    ListaProfesora = _context.Profesori.ToList(),
                    IzmenjenProfesor = (bool)izmenjenProfesor
                };

                return View(model);
            }

            return View(new ListaProfesoraViewModel
            {
                ListaProfesora = _context.Profesori.ToList()
            });
        }
        /// <summary>
        /// Dodaje se Profesor u Listu Profesora. Test name=ProfesoriController_Dodaj
        /// </summary>
        /// <returns>Vraca novog profesora, <see cref="ProfesorViewModel"/></returns>
        [Authorize(Roles = "Administrator, Editor")]
        public ActionResult Dodaj(bool? greska)
        {
            if (greska != null)
            {
                var modelSaGreskom = new ProfesorViewModel
                {
                    Predmeti = _context.Predmeti.ToList(),
                    Polovi = _context.Polovi.ToList(),
                    Greska = (bool)greska
                };
                return View("Dodaj", modelSaGreskom);
            }
            var model = new ProfesorViewModel
            {
                Predmeti = _context.Predmeti.ToList(),
                Polovi = _context.Polovi.ToList()
            };

            return View("Dodaj", model);
        }

        /// <summary>
        /// Cuvamo Profesora. <see cref="Profesor"/> 
        /// Username profesora se kreira tako što se uzima njegovo ime, prezime i redni broj i to na sl način
        /// (profesor.profesor10)
        /// Password se kreira na isti način samo sa ubačenim velikim slovima na sl način (Profesor.Profesor10)
        /// Test name=ProfesoriController_Sacuvaj
        /// </summary>
        /// <param name="pvm">The PVM.</param>
        /// <returns>Vraca nas na Index stranu Profesora</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Editor")]
        public async Task<ActionResult> Sacuvaj(ProfesorViewModel pvm, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                Profesor profesor = pvm.Profesor;
                foreach (var pr in pvm.PredmetiIDs)
                {
                    Predmet predmet = _context.Predmeti.SingleOrDefault(p => p.PredmetID == pr);
                    profesor.Predmeti.Add(predmet);
                }

                profesor.RedniBroj = GenerisiRedniBrojProfesora();

                string ime = VratiImeProfesoraSaPrvimVelikimSlovom(profesor.Ime);
                string prezime = VratiPrezimeProfesoraSaPrvimVelikimSlovom(profesor.Prezime);

                string username = profesor.Ime.ToLower().Replace(" ", string.Empty) + "." + profesor.Prezime.ToLower().Replace(" ", string.Empty) + profesor.RedniBroj;

                await RegistracijaProfesora(username, ime, prezime, profesor.RedniBroj);

                var id = _context.Users.SingleOrDefault(x => x.UserName == username).Id;

                profesor.UserProfesorId = id;

                //Dodavanje fotografije 
                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        profesor.Fotografija = reader.ReadBytes(upload.ContentLength);
                    }
                }

                _context.Profesori.Add(profesor);
                _context.SaveChanges();


                return RedirectToAction("Index", "Profesori", new { dodatProfesor = true });
            }
            else
            {
                return RedirectToAction("Dodaj", new { greska = true });
            }
        }

        //GET
        /// <summary>
        /// Metoda koja vrava view Izmeni za profesora
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Editor")]
        public ActionResult Izmeni(int? id, bool? greska)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Profesor profesor = _context.Profesori.Find(id);

            if (profesor == null)
            {
                return HttpNotFound();
            }

            if(greska!=null)
            {
                ProfesorViewModel profesorVMGreska = new ProfesorViewModel()
                {
                    Profesor = profesor,
                    Polovi = _context.Polovi.ToList(),
                    Predmeti = _context.Predmeti.ToList(),
                    Greska = (bool)greska
                };

                return View(profesorVMGreska);
            }

            ProfesorViewModel profesorVM = new ProfesorViewModel()
            {
                Profesor = profesor,
                Polovi = _context.Polovi.ToList(),
                Predmeti = _context.Predmeti.ToList()
            };

            return View(profesorVM);
        }

        /// <summary>
        /// Metoda koja sluzi za izmenu profesora
        /// </summary>
        /// <param name="profesorVM">The profesor vm.</param>
        /// <param name="upload">The upload.</param>
        /// <returns></returns>
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Editor")]
        public ActionResult Izmeni(ProfesorViewModel profesorVM, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        profesorVM.Profesor.Fotografija = reader.ReadBytes(upload.ContentLength);
                    }
                }

                Profesor profesor = new Profesor()
                {
                    ProfesorID = profesorVM.Profesor.ProfesorID,
                    Licenca = profesorVM.Profesor.Licenca,
                    Zvanje = profesorVM.Profesor.Zvanje,
                    Ime = profesorVM.Profesor.Ime,
                    Prezime = profesorVM.Profesor.Prezime,
                    Telefon = profesorVM.Profesor.Telefon,
                    Adresa = profesorVM.Profesor.Adresa,
                    Vanredan = profesorVM.Profesor.Vanredan,
                    RazredniStaresina = profesorVM.Profesor.RazredniStaresina,
                    RedniBroj = profesorVM.Profesor.RedniBroj,
                    PromenaLozinke = profesorVM.Profesor.PromenaLozinke,
                    UserProfesorId = profesorVM.Profesor.UserProfesorId,
                    PolId = profesorVM.Profesor.PolId,
                    Fotografija = profesorVM.Profesor.Fotografija
                };

                try
                {
                    _context.Profesori.AddOrUpdate(profesor);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }


                //promenljiva koja je potrebna kako bismo profesoru koji se menja mogli da obrisemo sve
                //predmete koje predaje, kako bismo kasnije dodali sve nove koji su prosledjeni sa forme za izmenu
                var profesor1 = _context.Profesori
                    .SingleOrDefault(p => p.ProfesorID == profesorVM.Profesor.ProfesorID);

                //brisanje svih predmeta profesora koje on predaje
                foreach (var predmet in profesor1.Predmeti.ToList())
                {
                    profesor1.Predmeti.Remove(predmet);
                    _context.SaveChanges();
                }

                //dodavanje svih predmeta profesoru koje on predaje
                foreach (var predmetId in profesorVM.PredmetiIDs)
                {
                    Predmet predmet = _context.Predmeti
                        .SingleOrDefault(p => p.PredmetID == predmetId);

                    profesor1.Predmeti.Add(predmet);
                    _context.SaveChanges();

                }

                return RedirectToAction("Index", new { izmenjenProfesor = true });

            }
            else
            {
                return RedirectToAction("Izmeni", new {id=profesorVM.Profesor.ProfesorID, greska = true });
            }

        }

        /// <summary>
        /// Generiše se redni broj profesora 
        /// Svaki profesor ima svoj jedinstveni redni broj koji će se koristiti za username i password
        /// Prvi profesor će imati redni broj 4, a svakom sledećem će se inkremetirati za 5 ili 3
        /// Ukoliko je ukupan broj profesora neparan, sledećem profesoru koji se unosi će se redni broj
        /// inkrementirati za 3, a ukoliko je ukupan broj paran inkrementiraće se za 5
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Editor")]
        public int GenerisiRedniBrojProfesora()
        {
            int redniBroj;

            var profesori = _context.Profesori
                .Select(p => p);

            if (profesori.Count() > 0)
            {
                redniBroj = profesori
                   .Select(p => p.RedniBroj)
                   .Max();

                var idProf = profesori.Select(p => p.ProfesorID).Max();

                if (idProf % 2 == 0)
                {
                    redniBroj += 5;
                }
                else
                {
                    redniBroj += 3;
                }
            }
            else
            {
                redniBroj = 4;
            }

            return redniBroj;
        }

        /// <summary>
        /// Metoda služi za registraciju profesora. 
        /// Username profesora se kreira tako što se uzima njegovo ime, prezime i redni broj i to na sl način
        /// (profesor.profesor10)
        /// Password se kreira na isti način samo sa ubačenim velikim slovima na sl način (Profesor.Profesor10)
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="ime">The IME.</param>
        /// <param name="prezime">The prezime.</param>
        /// <param name="redniBroj">The redni broj.</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Editor")]
        public async Task RegistracijaProfesora(string username, string ime, string prezime, int redniBroj)
        {
            var user = new ApplicationUser { UserName = username, Email = username.ToLower() + "@gmail.com" };
            var result = await UserManager.CreateAsync(user, ime + "." + prezime + redniBroj);

        }

        /// <summary>
        /// Vraca se ime profesora tako da prvo slovo bude veliko
        /// </summary>
        /// <param name="ime">The IME.</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Editor")]
        public string VratiImeProfesoraSaPrvimVelikimSlovom(string ime)
        {
            switch (ime)
            {
                case null: throw new ArgumentNullException(nameof(ime));
                case "": throw new ArgumentException($"{nameof(ime)} ne moze da bude prazno", nameof(ime));
                default: return ime.First().ToString().ToUpper().Replace(" ", string.Empty) + ime.Substring(1).Replace(" ", string.Empty);
            }
        }

        /// <summary>
        /// Vraca se prezime profesora tako da prvo slovo bude veliko
        /// </summary>
        /// <param name="prezime">The prezime.</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Editor")]
        public string VratiPrezimeProfesoraSaPrvimVelikimSlovom(string prezime)
        {
            switch (prezime)
            {
                case null: throw new ArgumentNullException(nameof(prezime));
                case "": throw new ArgumentException($"{nameof(prezime)} ne moze da bude prazno", nameof(prezime));
                default: return prezime.First().ToString().ToUpper().Replace(" ", string.Empty) + prezime.Substring(1).Replace(" ", string.Empty);
            }
        }
    }
}