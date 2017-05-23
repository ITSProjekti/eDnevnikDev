﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;

namespace eDnevnikDev.Controllers
{
    public class UceniciController : Controller
    {
        ApplicationDbContext _context;
        public UceniciController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Ucenici
        public ActionResult Index()
        {

            IEnumerable<Ucenik> ListaUcenika = _context.Ucenici.ToList();
            return View(ListaUcenika);
        }
    }
}