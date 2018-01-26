using eDnevnikDev.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Helpers
{
    public class SeedHelper
    {
       
        internal static void Seed(ApplicationDbContext context)
        {
            var RbPrveOznake = 1;

            var oznakaPrva = context.Oznake.SingleOrDefault(o => o.OznakaId == RbPrveOznake);

            if (oznakaPrva == null)
            {
                oznakaPrva = new Oznaka
                {
                    OznakaId = RbPrveOznake
                };

                context.Oznake.Add(oznakaPrva);
                context.SaveChanges();
            }


            var RbDrugeOznake = 2;

            var oznakaDruga = context.Oznake.SingleOrDefault(o => o.OznakaId == RbDrugeOznake);

            if (oznakaDruga == null)
            {
                oznakaDruga = new Oznaka
                {
                    OznakaId = RbDrugeOznake
                };

                context.Oznake.Add(oznakaDruga);
                context.SaveChanges();
            }


            var poljoprivredniTehnicarNaziv = "Пољопривредни техничар";

            var poljoprivredniTehnicar = context.Smerovi.SingleOrDefault(s => s.NazivSmera == poljoprivredniTehnicarNaziv);

            if(poljoprivredniTehnicar==null)
            {
                poljoprivredniTehnicar = new Smer
                {
                    SmerID = 1,
                    NazivSmera = poljoprivredniTehnicarNaziv,
                    Trajanje = 4,
                    
                };

                poljoprivredniTehnicar.Oznake.Add(oznakaPrva);
                poljoprivredniTehnicar.Oznake.Add(oznakaDruga);
                context.Smerovi.Add(poljoprivredniTehnicar);
                context.SaveChanges();


            }


            var RbTreceOznake = 3;

            var oznakaTreca = context.Oznake.SingleOrDefault(o => o.OznakaId == RbTreceOznake);

            if (oznakaTreca == null)
            {
                oznakaTreca = new Oznaka
                {
                    OznakaId = RbTreceOznake
                };

                context.Oznake.Add(oznakaTreca);
                context.SaveChanges();
            }


            var tehnicarHortikultureNaziv = "Техничар хортикултуре";

            var tehnicarHortikulture = context.Smerovi.SingleOrDefault(s => s.NazivSmera == tehnicarHortikultureNaziv);

            if (tehnicarHortikulture == null)
            {
                tehnicarHortikulture = new Smer
                {
                    SmerID = 2,
                    NazivSmera = tehnicarHortikultureNaziv,
                    Trajanje = 4
                };

                tehnicarHortikulture.Oznake.Add(oznakaTreca);
                context.Smerovi.Add(tehnicarHortikulture);
                context.SaveChanges();
            }



            var RbCetvrteOznake = 4;

            var oznakaCetvrta = context.Oznake.SingleOrDefault(o => o.OznakaId == RbCetvrteOznake);

            if (oznakaCetvrta == null)
            {
                oznakaCetvrta = new Oznaka
                {
                    OznakaId = RbCetvrteOznake
                };

                context.Oznake.Add(oznakaCetvrta);
                context.SaveChanges();
            }


            var RbPeteOznake = 5;

            var oznakaPeta = context.Oznake.SingleOrDefault(o => o.OznakaId == RbPeteOznake);

            if (oznakaPeta == null)
            {
                oznakaPeta = new Oznaka
                {
                    OznakaId = RbPeteOznake
                };

                context.Oznake.Add(oznakaPeta);
                context.SaveChanges();
            }


            var veterinarskiTehnicarNaziv = "Ветеринарски техничар";

            var veterinarskiTehnicar = context.Smerovi.SingleOrDefault(s => s.NazivSmera == veterinarskiTehnicarNaziv);

            if (veterinarskiTehnicar == null)
            {
                veterinarskiTehnicar = new Smer
                {
                    SmerID = 3,
                    NazivSmera = veterinarskiTehnicarNaziv,
                    Trajanje = 4
                };

                veterinarskiTehnicar.Oznake.Add(oznakaCetvrta);
                veterinarskiTehnicar.Oznake.Add(oznakaPeta);
                context.Smerovi.Add(veterinarskiTehnicar);
                context.SaveChanges();
            }



            var RbSesteOznake = 6;

            var oznakaSesta = context.Oznake.SingleOrDefault(o => o.OznakaId == RbSesteOznake);

            if (oznakaSesta == null)
            {
                oznakaSesta = new Oznaka
                {
                    OznakaId = RbSesteOznake
                };

                context.Oznake.Add(oznakaSesta);
                context.SaveChanges();
            }


            var prehrambeniTehnicarNaziv = "Прехрамбени техничар";

            var prehrambeniTehnicar = context.Smerovi.SingleOrDefault(s => s.NazivSmera == prehrambeniTehnicarNaziv);

            if (prehrambeniTehnicar == null)
            {
                prehrambeniTehnicar = new Smer
                {
                    SmerID = 4,
                    NazivSmera = prehrambeniTehnicarNaziv,
                    Trajanje = 4
                };


                prehrambeniTehnicar.Oznake.Add(oznakaSesta);
                context.Smerovi.Add(prehrambeniTehnicar);
                context.SaveChanges();
            }



            var RbSedmeOznake = 7;

            var oznakaSedma = context.Oznake.SingleOrDefault(o => o.OznakaId == RbSedmeOznake);

            if (oznakaSedma == null)
            {
                oznakaSedma = new Oznaka
                {
                    OznakaId = RbSedmeOznake
                };

                context.Oznake.Add(oznakaSedma);
                context.SaveChanges();
            }


            var mesarNaziv = "Месар";

            var mesar = context.Smerovi.SingleOrDefault(s => s.NazivSmera == mesarNaziv);

            if (mesar == null)
            {
                mesar = new Smer
                {
                    SmerID = 5,
                    NazivSmera = mesarNaziv,
                    Trajanje = 3
                };


                mesar.Oznake.Add(oznakaSedma);
                context.Smerovi.Add(mesar);
                context.SaveChanges();
            }


            var rukovodilacMehanicarPoljoprivredneTehnikeNaziv = "Руководилац-механичар пољопривредне технике";

            var rukovodilacMehanicarPoljoprivredneTehnike = context.Smerovi.SingleOrDefault(s => s.NazivSmera == rukovodilacMehanicarPoljoprivredneTehnikeNaziv);

            if (rukovodilacMehanicarPoljoprivredneTehnike == null)
            {
                rukovodilacMehanicarPoljoprivredneTehnike = new Smer
                {
                    SmerID = 6,
                    NazivSmera = rukovodilacMehanicarPoljoprivredneTehnikeNaziv,
                    Trajanje = 3
                };


                rukovodilacMehanicarPoljoprivredneTehnike.Oznake.Add(oznakaSedma);
                context.Smerovi.Add(rukovodilacMehanicarPoljoprivredneTehnike);
                context.SaveChanges();
            }
        }


        internal static void SeedTipOcene(ApplicationDbContext context)
        {
            var kontrolniNaziv = "Kontrolni";
            var tipOceneKontrolni = context.TipoviOcena
                .SingleOrDefault(t => t.Tip == kontrolniNaziv);

            if(tipOceneKontrolni==null)
            {
                tipOceneKontrolni = new TipOcene
                {
                    TipOceneId = 1,
                    Tip = kontrolniNaziv
                };

                context.TipoviOcena.Add(tipOceneKontrolni);
                context.SaveChanges();
            }

            var pismeniNaziv = "Pismeni";
            var tipOcenePismeni = context.TipoviOcena
                .SingleOrDefault(t => t.Tip == pismeniNaziv);

            if (tipOcenePismeni == null)
            {
                tipOcenePismeni = new TipOcene
                {
                    TipOceneId = 2,
                    Tip = pismeniNaziv
                };

                context.TipoviOcena.Add(tipOcenePismeni);
                context.SaveChanges();
            }

            var usmeniNaziv = "Kontrolni";
            var tipOceneUsmeni = context.TipoviOcena
                .SingleOrDefault(t => t.Tip == usmeniNaziv);

            if (tipOceneUsmeni == null)
            {
                tipOceneUsmeni = new TipOcene
                {
                    TipOceneId = 3,
                    Tip = usmeniNaziv
                };

                context.TipoviOcena.Add(tipOceneUsmeni);
                context.SaveChanges();
            }

            var zakljucnaNaziv = "Zakljucna";
            var tipOceneZakljucna = context.TipoviOcena
                .SingleOrDefault(t => t.Tip == zakljucnaNaziv);

            if (tipOceneZakljucna == null)
            {
                tipOceneZakljucna = new TipOcene
                {
                    TipOceneId = 4,
                    Tip = zakljucnaNaziv
                };

                context.TipoviOcena.Add(tipOceneZakljucna);
                context.SaveChanges();
            }

            var domaciNaziv = "Domaci";
            var tipOceneDomaci = context.TipoviOcena
                .SingleOrDefault(t => t.Tip == domaciNaziv);

            if (tipOceneDomaci == null)
            {
                tipOceneDomaci = new TipOcene
                {
                    TipOceneId = 5,
                    Tip = domaciNaziv
                };

                context.TipoviOcena.Add(tipOceneDomaci);
                context.SaveChanges();
            }
        }

        internal static void SeedTipPredmeta(ApplicationDbContext context)
        {
            var brojcanaNaziv = "Brojcana";
            var tipPredmetaBrojcana = context.TipoviOcenaPredmeta
                .SingleOrDefault(t => t.Tip == brojcanaNaziv);

            if(tipPredmetaBrojcana == null)
            {
                tipPredmetaBrojcana = new TipOcenePredmeta
                {
                    TipOcenePredmetaId = 1,
                    Tip = brojcanaNaziv
                };

                context.TipoviOcenaPredmeta.Add(tipPredmetaBrojcana);
                context.SaveChanges();
            }

            var opisnaNaziv = "Opisna";
            var tipPredmetaOpisna = context.TipoviOcenaPredmeta
                .SingleOrDefault(t => t.Tip == opisnaNaziv);

            if (tipPredmetaOpisna == null)
            {
                tipPredmetaOpisna = new TipOcenePredmeta
                {
                    TipOcenePredmetaId = 2,
                    Tip = opisnaNaziv
                };

                context.TipoviOcenaPredmeta.Add(tipPredmetaOpisna);
                context.SaveChanges();
            }
        }

    }
}