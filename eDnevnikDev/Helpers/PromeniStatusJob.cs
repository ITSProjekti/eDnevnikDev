using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Net;
using eDnevnikDev.Controllers;

namespace eDnevnikDev.Helpers
{
    public class PromeniStatusJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            DateTime pocetakSkolskeGodine = Convert.ToDateTime("11/12/2017").Date;
            DateTime krajSkolskeGodine = Convert.ToDateTime("11/13/2017").Date;
            DateTime trenutniDatum = DateTime.Now.Date;

            OdeljenjeController odeljenje = new OdeljenjeController();

            if (odeljenje != null)
            {
                if (trenutniDatum == pocetakSkolskeGodine)
                {
                    odeljenje.PromeniStatusOdeljenjima();
                }
                else if(trenutniDatum==krajSkolskeGodine)
                {
                    odeljenje.ArhivirajKreiranaOdeljenja();
                }
            }

            //odeljenje.ArhivirajKreiranaOdeljenja();
            //odeljenje.PromeniStatusOdeljenjima();
        }
    }
}