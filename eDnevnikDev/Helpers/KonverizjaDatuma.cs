using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace eDnevnikDev.Helpers
{
    public static class KonverizjaDatuma
    {
        public static DateTime izSrpskogUAmericki(string srpskiDatum)
        {
            DateTime americkiDatum;
            int dan, mesec, godina;

            try
            {
                char[] karakteri = { '.', '/', '-' };
                string[] niz = srpskiDatum.Split('.', '/', '-');

                dan = Int32.Parse(niz[0]);
                mesec = Int32.Parse(niz[1]);
                godina = Int32.Parse(niz[2]);
            }
            catch (Exception e)
            {
                throw e;
            }

            americkiDatum = new DateTime(godina, mesec, dan);
            return americkiDatum;
        }


        public static string izAmerickogUSrpski(DateTime americkiDatum)
        {
            string srpskiDatum;
            string dan, mesec, godina;

            dan = americkiDatum.Day.ToString();
            mesec = americkiDatum.Month.ToString();
            godina = americkiDatum.Year.ToString();

            srpskiDatum = dan + "." + mesec + "." + godina + ".";

            return srpskiDatum;
        }
    }
}