using Microsoft.VisualStudio.TestTools.UnitTesting;
using eDnevnikDev.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eDnevnikDev.Models;
using System.Data.Entity;
using Moq;
using System.Web.Mvc;
using eDnevnikDev.ViewModel;
using eDnevnikDev.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eDnevnikDev.Tests.Controllers
{
    [TestClass]
   public class OdeljenjeControllerTest
    {
        [TestMethod]
        public void OdeljenjeController_Index()
        {
            var controller = new OdeljenjeController();
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;

            Assert.AreEqual(result, viewResult);
        }

        //DONE
        [TestMethod]
        public void OdeljenjeController_OdeljenjeUcenici_VracaDvaOdsutna()
        {
            byte razred = 1;
            int oznakaOdeljenje = 1;
            int status = 3;
            var statusi = new List<Status>()
            {
                new Status() { StatusId=1,Opis="Arhivirano"},
                new Status() {StatusId=2,Opis="U toku" },
                new Status() {StatusId=3,Opis="Kreirano" }
            }.AsQueryable();

            var listaUcenika = new List<Ucenik>()
            {
                new Ucenik()
                {
                Adresa = "Adresa1",
                BrojTelefonaRoditelja = "064111222",
                DatumRodjenja = DateTime.Now,
                Ime = "Firas",
                Prezime = "Aburas",
                ImeMajke = "Mama",
                PrezimeMajke = "PrezimeMame",
                ImeOca = "Tata",
                PrezimeOca = "PrezimeTate",
                JedinstveniBroj = "1",
                JMBG = "1234567891012",
                MestoPrebivalista = "Beograd",
                MestoRodjenja = new Grad(),
                MestoRodjenjaId = 1,
                Odeljenje = new Odeljenje(),
                OdeljenjeId = oznakaOdeljenje,
                Razred = razred,
                Smer = new Smer(),
                SmerID = 1,
                UcenikID = 1,
                Vanredan = false },

                new Ucenik()
                { Adresa = "Adresa1",
                BrojTelefonaRoditelja = "064111222",
                DatumRodjenja = DateTime.Now,
                Ime = "Firas",
                Prezime = "Aburas",
                ImeMajke = "Mama",
                PrezimeMajke = "PrezimeMame",
                ImeOca = "Tata",
                PrezimeOca = "PrezimeTate",
                JedinstveniBroj = "1",
                JMBG = "1234567891012",
                MestoPrebivalista = "Beograd",
                MestoRodjenja = new Grad(),
                MestoRodjenjaId = 1,
                Odeljenje = new Odeljenje(),
                OdeljenjeId = oznakaOdeljenje,
                Razred = razred,
                Smer = new Smer(),
                SmerID = 1,
                UcenikID = 2,
                Vanredan = false }
            }.AsQueryable();
            var odeljenja = new List<Odeljenje>()
            {
                new Odeljenje() {Id=1,Oznaka=new Oznaka(),OznakaID=1,PocetakSkolskeGodine=2017,KrajSkolskeGodine=2018,Razred=1,Ucenici=listaUcenika.ToList(),Status=new Status(),StatusID=1 },
                new Odeljenje() {Id=2,Oznaka=new Oznaka(),OznakaID=6,PocetakSkolskeGodine=2018,KrajSkolskeGodine=2019,Razred=3,Ucenici=listaUcenika.ToList(),Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=3,Oznaka=new Oznaka(),OznakaID=3,PocetakSkolskeGodine=2017,KrajSkolskeGodine=2018,Razred=4,Ucenici=listaUcenika.ToList(),Status=new Status(),StatusID=2 },

            }.AsQueryable();

            var odsustva = new List<Odsustvo>(){
                new Odsustvo
                {
                    OdsustvoId = 1,
                    CasId = 1,
                    UcenikId=1,
                    Ucenik = listaUcenika.Single(x => x.UcenikID == 1)
                },

                new Odsustvo
                {
                    OdsustvoId = 2,
                    CasId = 1,
                    UcenikId = 2,
                    Ucenik = listaUcenika.Single(x => x.UcenikID == 2)
                }

            }.AsQueryable();

            var casovi = new List<Cas>(){
                new Cas
                {
                   CasId = 1,
                   Datum = DateTime.Today,
                   RedniBrojCasa = 1,
                   Odsustva = odsustva.ToList()
                }
            }.AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();

            var mockSetUcenik = new Mock<DbSet<Ucenik>>();
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.Provider).Returns(listaUcenika.Provider);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.Expression).Returns(listaUcenika.Expression);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.ElementType).Returns(listaUcenika.ElementType);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.GetEnumerator()).Returns(listaUcenika.GetEnumerator());
            foreach (var item in listaUcenika)
                mockSetUcenik.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Ucenici).Returns(mockSetUcenik.Object);

            var mockSetStatus = new Mock<DbSet<Status>>();
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.Provider).Returns(statusi.Provider);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.Expression).Returns(statusi.Expression);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.ElementType).Returns(statusi.ElementType);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.GetEnumerator()).Returns(statusi.GetEnumerator());

            foreach (var item in statusi)
                mockSetStatus.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Statusi).Returns(mockSetStatus.Object);

            var mockSetOdeljenje = new Mock<DbSet<Odeljenje>>();
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Provider).Returns(odeljenja.Provider);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Expression).Returns(odeljenja.Expression);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.ElementType).Returns(odeljenja.ElementType);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.GetEnumerator()).Returns(odeljenja.GetEnumerator());
            foreach (var item in odeljenja)
                mockSetOdeljenje.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Odeljenja).Returns(mockSetOdeljenje.Object);

            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.GetEnumerator()).Returns(casovi.GetEnumerator());
            mockContext.Setup(p => p.Casovi).Returns(mockSetCas.Object);

            var mockSetOdsustvo = new Mock<DbSet<Odsustvo>>();
            mockSetOdsustvo.As<IQueryable<Odsustvo>>().Setup(m => m.Provider).Returns(odsustva.Provider);
            mockSetOdsustvo.As<IQueryable<Odsustvo>>().Setup(m => m.Expression).Returns(odsustva.Expression);
            mockSetOdsustvo.As<IQueryable<Odsustvo>>().Setup(m => m.ElementType).Returns(odsustva.ElementType);
            mockSetOdsustvo.As<IQueryable<Odsustvo>>().Setup(m => m.GetEnumerator()).Returns(odsustva.GetEnumerator());
            mockContext.Setup(p => p.Odsustva).Returns(mockSetOdsustvo.Object);

            mockSetOdeljenje.Setup(x => x.Include("Status")).Returns(mockSetOdeljenje.Object);

            var odeljenjeController = new OdeljenjeController(mockContext.Object);
            var result = odeljenjeController.OdeljenjeUcenici(razred, oznakaOdeljenje, status) as JsonResult;


            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);

            string jsonString = JsonConvert.SerializeObject(result.Data);
            System.Diagnostics.Debug.WriteLine(jsonString);

            string jsonModified = jsonString;
            if (jsonString.Substring(0, 1) == "{" && jsonString.Substring(jsonString.Length - 1, 1) == "}")
            {
                jsonModified = jsonString.Insert(0, "[");
                jsonModified = jsonModified.Insert(jsonModified.Length, "]");
            }

            var jsonArray = Newtonsoft.Json.Linq.JArray.Parse(jsonModified);

            //var jsonArray = JArray.Parse(Helper.checkJsonJArray(jsonString));
            foreach (JObject item in jsonArray)
            {
                var ucenik = item.GetValue("Ucenici");
                foreach (JObject itemInner in ucenik)
                {
                    string id = itemInner["ID"].ToString();
                    string Ime = itemInner["Ime"].ToString();
                    string Prezime = itemInner["Prezime"].ToString();
                    string prisutan = itemInner["Prisutan"].ToString();


                    Ucenik temp = listaUcenika.FirstOrDefault(x => x.UcenikID == int.Parse(id));
                    Assert.AreEqual<string>(id, temp.UcenikID.ToString());
                    Assert.AreEqual<string>(Ime, temp.Ime.ToString());
                    Assert.AreEqual<string>(Prezime, temp.Prezime.ToString());
                    Assert.AreEqual<string>(prisutan, "false");
                }
            }
            Assert.AreEqual<int>(jsonArray.First.Count(), 2);

        }

        //DONE
        [TestMethod]
        public void OdeljenjeController_OdeljenjeUcenici_VracaJednogPrisutnogIJednogOdsutnog()
        {
            byte razred = 1;
            int oznakaOdeljenje = 1;
            int status = 3;
            var statusi = new List<Status>()
            {
                new Status() { StatusId=1,Opis="Arhivirano"},
                new Status() {StatusId=2,Opis="U toku" },
                new Status() {StatusId=3,Opis="Kreirano" }
            }.AsQueryable();

            var listaUcenika = new List<Ucenik>()
            {
                new Ucenik()
                {
                Adresa = "Adresa1",
                BrojTelefonaRoditelja = "064111222",
                DatumRodjenja = DateTime.Now,
                Ime = "Firas",
                Prezime = "Aburas",
                ImeMajke = "Mama",
                PrezimeMajke = "PrezimeMame",
                ImeOca = "Tata",
                PrezimeOca = "PrezimeTate",
                JedinstveniBroj = "1",
                JMBG = "1234567891012",
                MestoPrebivalista = "Beograd",
                MestoRodjenja = new Grad(),
                MestoRodjenjaId = 1,
                Odeljenje = new Odeljenje(),
                OdeljenjeId = oznakaOdeljenje,
                Razred = razred,
                Smer = new Smer(),
                SmerID = 1,
                UcenikID = 1,
                Vanredan = false },

                new Ucenik()
                { Adresa = "Adresa1",
                BrojTelefonaRoditelja = "064111222",
                DatumRodjenja = DateTime.Now,
                Ime = "Firas",
                Prezime = "Aburas",
                ImeMajke = "Mama",
                PrezimeMajke = "PrezimeMame",
                ImeOca = "Tata",
                PrezimeOca = "PrezimeTate",
                JedinstveniBroj = "1",
                JMBG = "1234567891012",
                MestoPrebivalista = "Beograd",
                MestoRodjenja = new Grad(),
                MestoRodjenjaId = 1,
                Odeljenje = new Odeljenje(),
                OdeljenjeId = oznakaOdeljenje,
                Razred = razred,
                Smer = new Smer(),
                SmerID = 1,
                UcenikID = 2,
                Vanredan = false }
            }.AsQueryable();
            var odeljenja = new List<Odeljenje>()
            {
                new Odeljenje() {Id=1,Oznaka=new Oznaka(),OznakaID=1,PocetakSkolskeGodine=2017,KrajSkolskeGodine=2018,Razred=1,Ucenici=listaUcenika.ToList(),Status=new Status(),StatusID=1 },
                new Odeljenje() {Id=2,Oznaka=new Oznaka(),OznakaID=6,PocetakSkolskeGodine=2018,KrajSkolskeGodine=2019,Razred=3,Ucenici=listaUcenika.ToList(),Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=3,Oznaka=new Oznaka(),OznakaID=3,PocetakSkolskeGodine=2017,KrajSkolskeGodine=2018,Razred=4,Ucenici=listaUcenika.ToList(),Status=new Status(),StatusID=2 },

            }.AsQueryable();

            var odsustva = new List<Odsustvo>(){
                
                new Odsustvo
                {
                    OdsustvoId = 2,
                    CasId = 1,
                    UcenikId = 2,
                    Ucenik = listaUcenika.Single(x => x.UcenikID == 2)
                }

            }.AsQueryable();

            var casovi = new List<Cas>(){
                new Cas
                {
                   CasId = 1,
                   Datum = DateTime.Today,
                   RedniBrojCasa = 1,
                   Odsustva = odsustva.ToList()
                }
            }.AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();

            var mockSetUcenik = new Mock<DbSet<Ucenik>>();
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.Provider).Returns(listaUcenika.Provider);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.Expression).Returns(listaUcenika.Expression);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.ElementType).Returns(listaUcenika.ElementType);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.GetEnumerator()).Returns(listaUcenika.GetEnumerator());
            foreach (var item in listaUcenika)
                mockSetUcenik.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Ucenici).Returns(mockSetUcenik.Object);

            var mockSetStatus = new Mock<DbSet<Status>>();
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.Provider).Returns(statusi.Provider);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.Expression).Returns(statusi.Expression);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.ElementType).Returns(statusi.ElementType);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.GetEnumerator()).Returns(statusi.GetEnumerator());

            foreach (var item in statusi)
                mockSetStatus.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Statusi).Returns(mockSetStatus.Object);

            var mockSetOdeljenje = new Mock<DbSet<Odeljenje>>();
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Provider).Returns(odeljenja.Provider);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Expression).Returns(odeljenja.Expression);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.ElementType).Returns(odeljenja.ElementType);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.GetEnumerator()).Returns(odeljenja.GetEnumerator());
            foreach (var item in odeljenja)
                mockSetOdeljenje.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Odeljenja).Returns(mockSetOdeljenje.Object);

            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.GetEnumerator()).Returns(casovi.GetEnumerator());
            mockContext.Setup(p => p.Casovi).Returns(mockSetCas.Object);

            var mockSetOdsustvo = new Mock<DbSet<Odsustvo>>();
            mockSetOdsustvo.As<IQueryable<Odsustvo>>().Setup(m => m.Provider).Returns(odsustva.Provider);
            mockSetOdsustvo.As<IQueryable<Odsustvo>>().Setup(m => m.Expression).Returns(odsustva.Expression);
            mockSetOdsustvo.As<IQueryable<Odsustvo>>().Setup(m => m.ElementType).Returns(odsustva.ElementType);
            mockSetOdsustvo.As<IQueryable<Odsustvo>>().Setup(m => m.GetEnumerator()).Returns(odsustva.GetEnumerator());
            mockContext.Setup(p => p.Odsustva).Returns(mockSetOdsustvo.Object);

            mockSetOdeljenje.Setup(x => x.Include("Status")).Returns(mockSetOdeljenje.Object);

            var odeljenjeController = new OdeljenjeController(mockContext.Object);
            var result = odeljenjeController.OdeljenjeUcenici(razred, oznakaOdeljenje, status) as JsonResult;


            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);

            string jsonString = JsonConvert.SerializeObject(result.Data);
            System.Diagnostics.Debug.WriteLine(jsonString);

            string jsonModified = jsonString;
            if (jsonString.Substring(0, 1) == "{" && jsonString.Substring(jsonString.Length - 1, 1) == "}")
            {
                jsonModified = jsonString.Insert(0, "[");
                jsonModified = jsonModified.Insert(jsonModified.Length, "]");
            }

            var jsonArray = Newtonsoft.Json.Linq.JArray.Parse(jsonModified);

            //var jsonArray = JArray.Parse(Helper.checkJsonJArray(jsonString));
            foreach (JObject item in jsonArray)
            {
                var ucenik = item.GetValue("Ucenici");
                foreach (JObject itemInner in ucenik)
                {
                    string id = itemInner["ID"].ToString();
                    string Ime = itemInner["Ime"].ToString();
                    string Prezime = itemInner["Prezime"].ToString();
                    string prisutan = itemInner["Prisutan"].ToString();


                    Ucenik temp = listaUcenika.FirstOrDefault(x => x.UcenikID == int.Parse(id));
                    Assert.AreEqual<string>(id, temp.UcenikID.ToString());
                    Assert.AreEqual<string>(Ime, temp.Ime.ToString());
                    Assert.AreEqual<string>(Prezime, temp.Prezime.ToString());
                    if (id == "1")
                    {
                        Assert.AreEqual<string>(prisutan, "true");
                    }
                    else
                    {
                        Assert.AreEqual<string>(prisutan, "false");
                    }
                    
                }
            }
            Assert.AreEqual<int>(jsonArray.First.Count(), 2);

        }

        [TestMethod]
        public void OdeljenjeController_OdeljenjeTrajanje()
        {
           
        }
    }

}
