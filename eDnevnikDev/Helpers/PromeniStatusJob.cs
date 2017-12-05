using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Net;
using eDnevnikDev.Controllers;
using eDnevnikDev.Models;

namespace eDnevnikDev.Helpers
{
    public class PromeniStatusJob : IJob
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public void Execute(IJobExecutionContext context)
        {

            var skolskaGodina = _context.SkolskaGodine.SingleOrDefault(s => s.Aktuelna == true);

            if (skolskaGodina != null)
            {
                DateTime pocetakSkolskeGodine = skolskaGodina.PocetakSkolskeGodine;
                DateTime krajSkolskeGodine = skolskaGodina.KrajSkolskeGodine;
                DateTime trenutniDatum = DateTime.Now;

                OdeljenjeController odeljenje = new OdeljenjeController();

                if (odeljenje != null)
                {
                    if (trenutniDatum.Day == pocetakSkolskeGodine.Day + 1 && trenutniDatum.Month == pocetakSkolskeGodine.Month && trenutniDatum.Year == pocetakSkolskeGodine.Year)
                    {
                        odeljenje.PromeniStatusOdeljenjima();
                    }
                    else if (trenutniDatum.Day == krajSkolskeGodine.Day && trenutniDatum.Month == krajSkolskeGodine.Month && trenutniDatum.Year == krajSkolskeGodine.Year)
                    {
                        odeljenje.ArhivirajKreiranaOdeljenja();
                        skolskaGodina.Aktuelna = false;
                        _context.SaveChanges();
                    }
                }
            }

        }

    }
}