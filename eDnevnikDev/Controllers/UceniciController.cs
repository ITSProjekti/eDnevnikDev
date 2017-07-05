﻿using System;
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
                Ucenik = new Ucenik { Ime = "Firas", Prezime = "Aburas", Adresa = "Adresa 1", BrojTelefonaRoditelja = "+381-11/1234567", ImeMajke = "Majka", ImeOca = "Otac", PrezimeMajke = "Prezime", PrezimeOca = "Prezime", JMBG = "1708993730202", MestoPrebivalista = "Beograd", MestoRodjenjaId = 3, DatumRodjenja = new DateTime(2011,12,5) },
                Gradovi = _context.Gradovi.OrderBy(g => g.Naziv).ToList(),
                Smerovi = _context.Smerovi.Include("Odeljenja").OrderBy(s => s.Trajanje).ToList()

            };

     
            return View("Dodaj", ucenikVM);
        }

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

            _context.Ucenici.Add(ucenik);

            _context.SaveChanges();

            if ( file != null)
            {
                var id = Ucenik.GetMd5Hash(ucenik.JMBG);

                string pic = System.IO.Path.GetExtension(file.FileName);

                file.SaveAs(HttpContext.Server.MapPath("~/slike/") + id + pic);

                ucenik.Fotografija = id + pic;

                _context.SaveChanges();

            }

            return RedirectToAction("Index", "Ucenici");
        }



    }
}