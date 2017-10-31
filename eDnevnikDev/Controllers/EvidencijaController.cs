using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using System.Data.Entity;
using eDnevnikDev.DTOs;
using System.Reflection;
using System.Web.Compilation;
using Moq;
using System.Collections;
using eDnevnikDev.ViewModel;

namespace eDnevnikDev.Controllers
{
    public class EvidencijaController : Controller
    {
        ApplicationDbContext _context;
        public EvidencijaController()
        {
            _context = new ApplicationDbContext();
        }
        public EvidencijaController(ApplicationDbContext Context)
        {
            _context = Context;
        }
        public class Ucenik2 { }

        // GET: Evidencija
        /// <summary>
        /// Vraca spisak svih ucenika po odeljenjima sortirane po azbucnom redu.Sve se renderuje na klijentskoj strani.
        /// NOT TESTED
        /// </summary>
        /// <returns>Spisak ucenika u odredjenom odeljenju</returns>
        public ActionResult Index1()
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



            //string MockQualifiedName = typeof(DbSet).AssemblyQualifiedName;
            //Type elementTypeDbSet = Type.GetType(UcenikQualifiedName);



            //string qualifiedName = typeof(Ucenik).AssemblyQualifiedName;
            //Type elementType = Type.GetType(qualifiedName);


            //Type MockType = typeof(Mock<>).MakeGenericType(new[] { elementType });//ggwp
            //var list = Activator.CreateInstance(MockType);


           return View();
            
        }

        public ActionResult Collapse()
        {
            return View();
        }




        public JsonResult VratiPredmenete(int? odeljenjeId, int? profesorId)
        {

            if(odeljenjeId!=null && profesorId!=null)
            {
                var profesorPredmeti = _context.Profesori
                          .SingleOrDefault(p => p.ProfesorID == profesorId)
                          .Predmeti.Select(x => x);

                var odeljenjePredmeti = _context.Odeljenja
                                      .SingleOrDefault(o => o.Id == odeljenjeId)
                                      .Predmeti.Select(x => x);

                if(profesorPredmeti!=null && odeljenjePredmeti!=null)
                {

                    var predmetiProfesorOdeljenje = profesorPredmeti.Intersect(odeljenjePredmeti);

                    List<DTOPredmet> predmetiDTO = new List<DTOPredmet>();

                    foreach (var p in predmetiProfesorOdeljenje)
                    {
                        predmetiDTO.Add(new DTOPredmet
                        {
                            PredmetId = p.PredmetID,
                            NazivPredmeta = p.NazivPredmeta,
                          //  TipOcenePredmetaId = p.TipOcenePredmetaId
                        });
                    }

                    return Json(predmetiDTO, JsonRequestBehavior.AllowGet);
                }


                return Json(new DTOPredmet(), JsonRequestBehavior.AllowGet);



            }


            return Json(new DTOPredmet(), JsonRequestBehavior.AllowGet);

        }

        public JsonResult VratiOcene(int? odeljenjeId, int? profesorId, int? predmetId, int? ucenikId)
        {

            if (odeljenjeId != null && profesorId != null && predmetId!=null)
            {
                             
                  var casoviId = _context.Casovi
                              .Where(c => c.ProfesorId == profesorId 
                               && c.OdeljenjeId==odeljenjeId 
                               && c.PredmetId==predmetId)
                               .Select(c=>c.CasId);

                if(casoviId!=null)
                {
                   var DTOocene = new List<DTOOcena>();

                    foreach (var c in casoviId)
                    {
                        var ocene = _context.Ocene
                                  .Where(o => o.CasId == c && o.UcenikId == ucenikId)
                                  .Select(o => o);

                        if(ocene!=null)
                        {
                            foreach (var o in ocene)
                            {
                                DTOocene.Add(new DTOOcena
                                {
                                    Ocena = (int)o.Oznaka,
                                    TipOcene = o.TipOcene.Tip,
                                    Komentar = o.Napomena
                                });
                            }

                        }

                        
                    }

                    return Json(DTOocene, JsonRequestBehavior.AllowGet);

                }

                return Json(new DTOOcena(), JsonRequestBehavior.AllowGet);

             }


             return Json(new DTOOcena(), JsonRequestBehavior.AllowGet);

        }        
    }
}