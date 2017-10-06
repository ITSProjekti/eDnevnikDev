using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eDnevnikDev.Models;
using eDnevnikDev.DTOs;
using System.Data.Entity.Migrations;
using System.Web.Helpers;
using eDnevnikDev.Helpers;

namespace eDnevnikDev.Controllers
{
    public class NapomeneController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public NapomeneController(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        /// <summary>
        /// Ukoliko napomena za odredjenog ucenika na odredjenom casu vec postoji,
        /// odradiće se njen update. Ukoliko ne postoji kreiraće se nova napomena u bazi.
        /// </summary>
        /// <param name="dtoNapomena"></param>
        /// <returns></returns>
        [HttpPost]
//[ValidateHeaderAntiForgeryToken]
        public void DodajIliIzmeniNapomenu(DTONapomena dtoNapomena)
        {
            if (dtoNapomena != null)
            {
                var napomena = new Napomena
                {
                    NapomenaId = dtoNapomena.NapomenaId,
                    Opis = dtoNapomena.Opis,
                    UcenikId = dtoNapomena.UcenikId,
                    ProfesorId = dtoNapomena.ProfesorId,
                    CasId = dtoNapomena.CasId
                };

                _context.Napomene.AddOrUpdate(napomena);
                _context.SaveChanges();
            }
        }



        /// <summary>
        /// Služi da se, pri otvaranju popup-a napomene za određenog učenika na određenom času,
        /// u tekstu napomene ispiše prethodno unet tekst napomene ukoliko ta napomena već
        /// postoji u bazi. Ukoliko ne postoji, prostor za unos teksta napomene će biti prazan i
        /// pri čuvanju unetog, kreira se nova napomena u bazi. U suprotnom bi se samo
        /// uradio update napomene 
        /// </summary>
        /// <param name="ucenikId"></param>
        /// <param name="casId"></param>
        /// <returns></returns>
        public JsonResult VratiNapomenu(int ucenikId, int casId)
        {
            var napomena = _context.Napomene
                .Where(n => n.UcenikId == ucenikId)
                .SingleOrDefault(n => n.CasId == casId);

            if (napomena != null)
            {
                //Koristi se klasa DTONapomena
                var dtoNapomena = new DTONapomena
                {
                    NapomenaId = napomena.NapomenaId,
                    Opis = napomena.Opis,
                    ProfesorId = napomena.ProfesorId,
                    UcenikId = napomena.UcenikId,
                    CasId = napomena.CasId
                };

                return Json(dtoNapomena, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new DTONapomena(), JsonRequestBehavior.AllowGet);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}