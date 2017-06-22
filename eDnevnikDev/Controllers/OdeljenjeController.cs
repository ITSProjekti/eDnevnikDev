﻿using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.DTOs;

namespace eDnevnikDev.Controllers
{
    public class OdeljenjeController : Controller
    {
        ApplicationDbContext _context;

        public OdeljenjeController()
        {
            _context = new ApplicationDbContext();

        }
        public OdeljenjeController(ApplicationDbContext context)
        {
            this._context = context;

        }

        // GET: Odeljenje
        /// <summary>
        /// Index kontrola
        /// </summary>
        /// <returns>Vraca View za Index</returns>
        public ActionResult Index()
        {
            





            return View();
        }

        public JsonResult OdeljenjeTrajanje(int godina)
        {
            var kolekcijaOdeljenja = _context.Smerovi.Where(s => s.Trajanje >= godina).Select(s => s.Odeljenja).ToList();

            var pov = new List<DTOOdeljenje>();

            foreach(var lista in kolekcijaOdeljenja)
            {
                foreach(var odeljenje in lista)
                {
                    if( !pov.Any( o => o.Oznaka == odeljenje.Oznaka))
                        pov.Add(new DTOOdeljenje {Id = odeljenje.Id, Oznaka = odeljenje.Oznaka });                    
                }
            }

            pov = pov.OrderBy(o => o.Oznaka).ToList();

            return Json(pov, JsonRequestBehavior.AllowGet);
        }
    }
}