﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using System.Reflection;
using System.Web.Compilation;
using System.Reflection;
using Moq;
using System.Collections;
using System.Data.Entity;
using System.Collections;

namespace eDnevnikDev.Controllers
{
    public class EvidencijaController : Controller
    {
        ApplicationDbContext _context;
        public EvidencijaController()
        {
            _context = new ApplicationDbContext();
        }
        public class Ucenik2 { }

        // GET: Evidencija
        /// <summary>
        /// Vraca spisak svih ucenika po odeljenjima sortirane po azbucnom redu.Sve se renderuje na klijentskoj strani.
        /// NOT TESTED
        /// </summary>
        /// <returns>Spisak ucenika u odredjenom odeljenju</returns>
        public ActionResult Index()
        {
            // Assembly assem = typeof(Ucenik2).Assembly;
            // Type objType = typeof(Ucenik2);
            // var asd=assem.GetType("Ucenik2");
            // string qual = objType.AssemblyQualifiedName;

            //BuildManager.GetType("Ucenik2", true); 
            //string ime = "Ucenik2";
            //var type2 = Type.GetType(qual);
            // Object o = (Activator.CreateInstance(type2));
            // Type t = o.GetType();
            //Activator.CreateInstance(t);
            //var type = Type.GetType("eDnevnikDev.Models.Ucenik");
            //var o = (Activator.CreateInstance(type.AssemblyQualifiedName.ToString);

            string qualifiedName = typeof(Ucenik).AssemblyQualifiedName;
            Type elementType2 = Type.GetType(qualifiedName);

           
            Type listType1 = typeof(List<>).MakeGenericType(new[] { elementType2 });
            IList list = (IList)Activator.CreateInstance(listType1);

            //string temp = "Ucenik";
            //string UcenikQualifiedName = Type.GetType(temp);
            string UcenikQualifiedName2 = typeof(Ucenik).AssemblyQualifiedName;
            string UcenikQualifiedName3 = typeof(TipOcene).AssemblyQualifiedName;

            //FIND IN STRING MODELS.{CHANGE}

            Type elementType = Type.GetType(UcenikQualifiedName2);
            Type dbSetType = typeof(DbSet<>).MakeGenericType(new[] { elementType });

            Type listType = typeof
                (Mock<>).MakeGenericType(new[] { dbSetType });
            Mock mock = (Mock)Activator.CreateInstance(listType);

          ;

            //string MockQualifiedName = typeof(DbSet).AssemblyQualifiedName;
            //Type elementTypeDbSet = Type.GetType(UcenikQualifiedName);



            //string qualifiedName = typeof(Ucenik).AssemblyQualifiedName;
            //Type elementType = Type.GetType(qualifiedName);


            //Type MockType = typeof(Mock<>).MakeGenericType(new[] { elementType });//ggwp
            //var list = Activator.CreateInstance(MockType);

                return View();
            
        }
    }
}