using eDnevnikDev.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.DTOs;
using System.Threading.Tasks;
using eDnevnikDev.ViewModel;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace eDnevnikDev.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext _context;
        public ApplicationUserManager _userManager;



        public RoleController()
        {
            _context = new ApplicationDbContext();
        }

        public RoleController(ApplicationDbContext context)
        {
            this._context = context;
        }

        //Koristi se za rad sa rolama
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
        /// Index metoda. Vraca view sa profesorima i ucenicima i njihovim rolama 
        /// Test name="" 
        /// </summary>
        /// <returns>Vraca Index View</returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Metoda vraca sve profesore sa njihovim rolama kada se pozove putem combobox-a sa Index view-a
        /// <see cref="DTOProfesor"/>
        /// Test name= ""
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> VratiProfesore()
        { 
            var profesori = _context.Profesori.ToList();

            if (profesori != null)
            {
                //Koristi se DTOProfesor
                var dtoProfesori = new List<DTOProfesor>();
                foreach (var p in profesori)
                {
                    dtoProfesori.Add(new DTOProfesor
                    {
                        Id = p.UserProfesorId,
                        Ime = p.Ime,
                        Prezime = p.Prezime,
                        //Lista svih rola za datog profesora
                        Role = (List<string>)await UserManager.GetRolesAsync(p.UserProfesorId)

                });


                }
                //Formatiranje JSON-a.
                //Vraca listu profesora
                return Json(dtoProfesori, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Formatiranje JSON-a.
                //Vraca prazan objekat klase DTOProfesor ukoliko ne postoje profesori u bazi
                return Json(new DTOProfesor(), JsonRequestBehavior.AllowGet);

            }



        }

        /// <summary>
        /// Metoda vraca sve ucenike sa njihovim rolama kada se pozove putem combobox-a sa Index view-a
        /// <see cref="DTOUcenik"/>
        /// Test name= ""
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> VratiUcenike()
        {

            var ucenici = _context.Ucenici.ToList();

            if (ucenici != null)
            {
                //Koristi se DTOUcenik
                var dtoUcenici = new List<DTOUcenik>();

                foreach (var u in ucenici)
                {
                    dtoUcenici.Add(new DTOUcenik
                    {
                        Id = u.UserUcenikId,
                        JMBG=u.JMBG,
                        Ime = u.Ime,
                        Prezime = u.Prezime,
                        //Lista svih rola za datog ucenika
                        Role = (List<string>)await UserManager.GetRolesAsync(u.UserUcenikId)

                });


                }
                //Formatiranje JSON-a.
                //Vraca listu ucenika
                return Json(dtoUcenici, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //Formatiranje JSON-a.
                //Vraca prazan objekat klase DTOProfesor ukoliko ne postoje ucenici u bazi
                return Json(new DTOUcenik(), JsonRequestBehavior.AllowGet);

            }


        }

        /// <summary>
        /// Metoda pronalazi profesora u bazi na osnovu prosledjenog id-a i proverava koje role ima profesor
        /// Test name="" 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Vraca PromeniPravoPristupaProfesora View</returns>
        public async Task<ActionResult> PromeniPravoPristupaProfesora(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Profesor profesor = _context.Profesori.SingleOrDefault(p => p.UserProfesorId == id);

            if(profesor == null)
            {
                return HttpNotFound();

            }

            //Koristi se ProfesorRoleViewModel
            var prvm = new ProfesorRoleViewModel
            {
                UserProfesorId = profesor.UserProfesorId,
                Ime = profesor.Ime,
                Prezime = profesor.Prezime
            };

            //Sve role za datog profesora
            var role = await UserManager.GetRolesAsync(profesor.UserProfesorId);

            foreach (var r in role)
            {
                //Provera da li je rola profesor dodeljenja
                if(r==RoleNames.ROLE_PROFESOR)
                {
                    prvm.RolaProfesor = true;
                }
                //Provera da li je rola editor dodeljenja
                else if (r==RoleNames.ROLE_EDITOR)
                {
                    prvm.RolaEditor = true;
                }
            }



                return View(prvm);

            }

        /// <summary>
        /// Metoda pronalazi ucenika u bazi na osnovu prosledjenog id-a i proverava koje role ima ucenik
        /// Test name="" 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Vraca PromeniPravoPristupaUcenika View</returns>
        public async Task<ActionResult> PromeniPravoPristupaUcenika(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Ucenik ucenik = _context.Ucenici.SingleOrDefault(u => u.UserUcenikId == id);

            if (ucenik == null)
            {
                return HttpNotFound();

            }

            //Koristi se ProfesorRoleViewModel
            var urvm = new UcenikRoleViewModel
            {
                UserUcenikId = ucenik.UserUcenikId,
                Ime = ucenik.Ime,
                Prezime = ucenik.Prezime,
                JMBG=ucenik.JMBG
                
            };

            //Sve role za datog profesora
            var role = await UserManager.GetRolesAsync(ucenik.UserUcenikId);

            foreach (var r in role)
            {
                //Provera da li je rola ucenik dodeljenja
                if (r == RoleNames.ROLE_UCENIK)
                {
                    urvm.RolaUcenik = true;
                }
                //Provera da li je rola editor dodeljenja
                else if (r == RoleNames.ROLE_EDITOR)
                {
                    urvm.RolaEditor = true;
                }
            }



            return View(urvm);

        }


        /// <summary>
        /// Metoda za dodavnje role
        /// <see cref="DTORola"/>
        /// Test name= ""
        /// </summary>
        /// <param name="dtoRola"></param>
        /// <returns></returns>
        public async Task DodajRolu(DTORola dtoRola)
        {
            // userManager se koristi za pristup bazi prilikom rada sa rolama
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            //Dodavanje role
            await userManager.AddToRoleAsync(dtoRola.KorisnikID, dtoRola.Rola);
        }


        /// <summary>
        /// Metoda za brisanje role
        /// <see cref="DTORola"/>
        /// Test name= ""
        /// </summary>
        /// <param name="dtoRola"></param>
        /// <returns></returns>
        public async Task ObrisiRolu(DTORola dtoRola)
        {
            // userManager se koristi za pristup bazi prilikom rada sa rolama
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            //Brisanje role
            await userManager.RemoveFromRoleAsync(dtoRola.KorisnikID, dtoRola.Rola);
        }



        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


    }
}