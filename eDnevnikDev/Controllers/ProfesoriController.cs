﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using eDnevnikDev.ViewModel;

namespace eDnevnikDev.Controllers
{
    public class ProfesoriController : Controller
    {
        ApplicationDbContext _context;

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
        /// Cuvamo Profesora. Test name=ProfesoriController_Sacuvaj
        /// </summary>
        /// <param name="pvm">The PVM.</param>
        /// <returns>Vraca nas na Index stranu Profesora</returns>
        [HttpPost]
        public ActionResult Sacuvaj(ProfesorViewModel pvm)
        {

            if (ModelState.IsValid)
            {
                Profesor profesor = pvm.Profesor;
                foreach (var pr in pvm.PredmetiIDs)
                {
                    Predmet predmet = _context.Predmeti.SingleOrDefault(p => p.PredmetID == pr);
                    profesor.Predmeti.Add(predmet);
                }
                _context.Profesori.Add(pvm.Profesor);

                _context.SaveChanges();

                return RedirectToAction("Index", "Profesori");
            }
            else
            {
                pvm.Predmeti = _context.Predmeti.ToList();
                return View("Dodaj", pvm);
            }
        }
    }
}