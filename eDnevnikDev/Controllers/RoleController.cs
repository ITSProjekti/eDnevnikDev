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

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> VratiProfesore()
        {
            
            var profesori = _context.Profesori.ToList();

            if (profesori != null)
            {
                var dtoProfesori = new List<DTOProfesor>();
                foreach (var p in profesori)
                {
                    List<string> roleProfesora = new List<string>();
                    roleProfesora = (List<string>)await UserManager.GetRolesAsync(p.UserProfesorId);

                    dtoProfesori.Add(new DTOProfesor
                    {
                        Id = p.UserProfesorId,
                        Ime = p.Ime,
                        Prezime = p.Prezime,
                        Role = roleProfesora

                    });


                }
                return Json(dtoProfesori, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new DTOProfesor(), JsonRequestBehavior.AllowGet);

            }



        }


        public async Task<JsonResult> VratiUcenike()
        {

            var ucenici = _context.Ucenici.ToList();
            if (ucenici != null)
            {
                var dtoUcenici = new List<DTOUcenik>();
                foreach (var u in ucenici)
                {
                    List<string> roleUcenika = new List<string>();
                    roleUcenika= (List<string>)await UserManager.GetRolesAsync(u.UserUcenikId);


                    dtoUcenici.Add(new DTOUcenik
                    {
                        Id = u.UserUcenikId,
                        JMBG=u.JMBG,
                        Ime = u.Ime,
                        Prezime = u.Prezime,
                        Role = roleUcenika

                    });


                }
                return Json(dtoUcenici, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new DTOUcenik(), JsonRequestBehavior.AllowGet);

            }


        }
     

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

            var prvm = new ProfesorRoleViewModel
            {
                UserProfesorId = profesor.UserProfesorId,
                Ime = profesor.Ime,
                Prezime = profesor.Prezime
            };

            var role = await UserManager.GetRolesAsync(profesor.UserProfesorId);

            foreach (var r in role)
            {
                if(r==RoleNames.ROLE_PROFESOR)
                {
                    prvm.RolaProfesor = true;
                }
                else if(r==RoleNames.ROLE_EDITOR)
                {
                    prvm.RolaEditor = true;
                }
            }



                return View(prvm);

            }

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

            var urvm = new UcenikRoleViewModel
            {
                UserUcenikId = ucenik.UserUcenikId,
                Ime = ucenik.Ime,
                Prezime = ucenik.Prezime,
                JMBG=ucenik.JMBG
                
            };

            var role = await UserManager.GetRolesAsync(ucenik.UserUcenikId);

            foreach (var r in role)
            {
                if (r == RoleNames.ROLE_UCENIK)
                {
                    urvm.RolaUcenik = true;
                }
                else if (r == RoleNames.ROLE_EDITOR)
                {
                    urvm.RolaEditor = true;
                }
            }



            return View(urvm);

        }



        public async Task DodajRolu(DTORola dtoRola)
        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            await userManager.AddToRoleAsync(dtoRola.KorisnikID, dtoRola.Rola);
        }


        public async Task ObrisiRolu(DTORola dtoRola)
        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

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