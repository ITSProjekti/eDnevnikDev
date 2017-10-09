using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using System.Data.Entity;
using eDnevnikDev.DTOs;

namespace eDnevnikDev.Controllers
{
    public class EvidencijaController : Controller
    {
        ApplicationDbContext _context;
        public EvidencijaController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Evidencija
        /// <summary>
        /// Vraca spisak svih ucenika po odeljenjima sortirane po azbucnom redu.Sve se renderuje na klijentskoj strani.
        /// NOT TESTED
        /// </summary>
        /// <returns>Spisak ucenika u odredjenom odeljenju</returns>
        public ActionResult Index()
        {
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
                            TipOcenePredmetaId = p.TipOcenePredmetaId
                        });
                    }

                    return Json(predmetiDTO, JsonRequestBehavior.AllowGet);
                }


                return Json(new DTOPredmet(), JsonRequestBehavior.AllowGet);



            }


            return Json(new DTOPredmet(), JsonRequestBehavior.AllowGet);

        }

        public JsonResult VratiBrojcaneOcene(int? odeljenjeId, int? profesorId, int? predmetId, int? ucenikId)
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
                   var DTOocene = new List<DTOBrojcanaOcena>();

                    foreach (var c in casoviId)
                    {
                        var ocene = _context.Ocene
                                  .Where(o => o.CasId == c && o.UcenikId == ucenikId)
                                  .Select(o => o);

                        if(ocene!=null)
                        {
                            foreach (var o in ocene)
                            {
                                DTOocene.Add(new DTOBrojcanaOcena
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

                return Json(new DTOBrojcanaOcena(), JsonRequestBehavior.AllowGet);

             }


             return Json(new DTOBrojcanaOcena(), JsonRequestBehavior.AllowGet);

        }        
    }
}