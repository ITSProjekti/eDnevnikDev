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
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            IEnumerable<Ucenik> ListaUcenika = _context.Ucenici.ToList();
            return View("Index", ListaUcenika);
        }
        /// <summary>
        /// Dodajs this instance.
        /// Funkcija koja vraca view dodaj.
        /// </summary>
        /// <returns></returns>
        public ActionResult Dodaj()
        {
            var ucenikVM = new UcenikViewModel
            {
                Gradovi = _context.Gradovi.OrderBy(g => g.Naziv).ToList(),
                Smerovi = _context.Smerovi.Include("Odeljenja").OrderBy(s => s.Trajanje).ToList()

            };

            


            return View("Dodaj", ucenikVM);
        }


        
    }
}