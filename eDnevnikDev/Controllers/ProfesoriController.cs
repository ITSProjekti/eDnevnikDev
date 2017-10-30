using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using eDnevnikDev.ViewModel;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

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
        public ActionResult Index()
        {
            IEnumerable<Profesor> ListaProfesora = _context.Profesori.ToList();
            return View("Index",ListaProfesora);
        }
        /// <summary>
        /// Dodaje se Profesor u Listu Profesora. Test name=ProfesoriController_Dodaj
        /// </summary>
        /// <returns>Vraca novog profesora, <see cref="ProfesorViewModel"/></returns>
        public ActionResult Dodaj()
        {
            var model = new ProfesorViewModel
            {
                Predmeti = _context.Predmeti.ToList()
            };
            return View("Dodaj",model);
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
        public async Task<ActionResult> Sacuvaj(ProfesorViewModel pvm)
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
                
                string username = profesor.Ime.ToLower() + "." + profesor.Prezime.ToLower() + profesor.RedniBroj;

                await RegistracijaProfesora(username, profesor.Ime, profesor.Prezime, profesor.RedniBroj);

                var id = _context.Users.SingleOrDefault(x => x.UserName == username).Id;

                //await UserManager.AddToRoleAsync(id, "Profesor");

                profesor.UserProfesorId = id;

                _context.Profesori.Add(profesor);
                _context.SaveChanges();


                return RedirectToAction("Index", "Profesori");
            }
            else
            {
                pvm.Predmeti = _context.Predmeti.ToList();
                return View("Dodaj", pvm);
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
        public int GenerisiRedniBrojProfesora()
        {
            var profesori = _context.Profesori
                .Select(p => p);

            int redniBroj = profesori
                   .Select(p => p.RedniBroj)
                   .Max();

            if (profesori.Count() > 0)
            {
                var idProf = profesori.Select(p => p.ProfesorID).Max();

                if(idProf%2==0)
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
        public async Task RegistracijaProfesora(string username, string ime, string prezime, int redniBroj)
        {
            var user = new ApplicationUser { UserName = username, Email=username+"@gmail.com" };
            var result = await UserManager.CreateAsync(user, ime + "." + prezime + redniBroj);
          
        }
    }
}