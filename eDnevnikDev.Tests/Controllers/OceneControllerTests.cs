using Microsoft.VisualStudio.TestTools.UnitTesting;
using eDnevnikDev.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eDnevnikDev.Models;
using Moq;
using System.Data.Entity;
using System.Web.Mvc;
using eDnevnikDev.Tests.helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eDnevnikDev.Controllers.Tests
{
    [TestClass()]
    public class OceneControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DetailsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void vratiOceneTest()
        {
            byte razred = 1;
            int oznakaOdeljenje = 13;
            int casId = 6;
            int profesorId = 2;
            int sifraUcenika = 18;
            
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
                UcenikID = sifraUcenika,
                Vanredan = false }
            }.AsQueryable();

            var casovi = new List<Cas>()
            {
                new Cas
                {
                    CasId = casId
                },

                new Cas
                {
                    CasId = 6
                }

            }.AsQueryable();

            var ocene = new List<Ocena>()
            {
                new Ocena
                {
                    Cas = casovi.FirstOrDefault(),
                    Ucenik = listaUcenika.FirstOrDefault(),
                    Oznaka = 5,
                    Napomena = "Kontrolni 1",
                    TipOceneId = 1,
                    OcenaId = 1
                },
                
            }.AsQueryable();

            var odeljenja = new List<Odeljenje>()
            {
                new Odeljenje() {Id=11,Oznaka=new Oznaka(),OznakaID=1,PocetakSkolskeGodine=2017,KrajSkolskeGodine=2018,Razred=1,Ucenici=listaUcenika.ToList(),Status=new Status(),StatusID=1 },
                new Odeljenje() {Id=12,Oznaka=new Oznaka(),OznakaID=6,PocetakSkolskeGodine=2018,KrajSkolskeGodine=2019,Razred=3,Ucenici=listaUcenika.ToList(),Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=13,Oznaka=new Oznaka(),OznakaID=3,PocetakSkolskeGodine=2017,KrajSkolskeGodine=2018,Razred=4,Ucenici=listaUcenika.ToList(),Status=new Status(),StatusID=2 },

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

            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.GetEnumerator()).Returns(casovi.GetEnumerator());
            foreach (var item in casovi)
                mockSetCas.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Casovi).Returns(mockSetCas.Object);

            var mockSetOcene = new Mock<DbSet<Ocena>>();
            mockSetOcene.As<IQueryable<Ocena>>().Setup(m => m.Provider).Returns(ocene.Provider);
            mockSetOcene.As<IQueryable<Ocena>>().Setup(m => m.Expression).Returns(ocene.Expression);
            mockSetOcene.As<IQueryable<Ocena>>().Setup(m => m.ElementType).Returns(ocene.ElementType);
            mockSetOcene.As<IQueryable<Ocena>>().Setup(m => m.GetEnumerator()).Returns(ocene.GetEnumerator());
            foreach (var item in ocene)
                mockSetOcene.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Ocene).Returns(mockSetOcene.Object);

            var mockSetOdeljenje = new Mock<DbSet<Odeljenje>>();
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Provider).Returns(odeljenja.Provider);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Expression).Returns(odeljenja.Expression);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.ElementType).Returns(odeljenja.ElementType);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.GetEnumerator()).Returns(odeljenja.GetEnumerator());
            foreach (var item in odeljenja)
                mockSetOdeljenje.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Odeljenja).Returns(mockSetOdeljenje.Object);

            var oceneController = new OceneController(mockContext.Object);
            var result = oceneController.vratiOcene(casId, profesorId, sifraUcenika) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);

            string jsonString = JsonConvert.SerializeObject(result.Data);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var jsonArray = JArray.Parse(Helper.checkJsonJArray(jsonString));

            foreach (JObject item in jsonArray)
            {
                string oznaka = item["oznaka"].ToString();
                Assert.AreEqual<string>(oznaka, "5");
            }
            Assert.AreEqual<int>(jsonArray.Count(), 1);
        }
    }
}