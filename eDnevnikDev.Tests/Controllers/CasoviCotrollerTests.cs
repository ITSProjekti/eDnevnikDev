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

namespace eDnevnikDev.Controllers.Tests
{
    [TestClass()]
    public class CasoviControllerTests
    {
        //srecno!
        //preko 100 linija koda
        [Ignore]
        [TestMethod()]
        public void CreateTest()
        {
            Assert.Fail();
        }

        [Ignore]
        [TestMethod()]
        public void CreateTest1()
        {
            Assert.Fail();
        }

        //identity
        [Ignore]
        [TestMethod()]
        public void VratiPredmeteTest()
        {
            Assert.Fail();
        }

        //DONE
        [TestMethod()]
        public void VratiRedniBrojPredmetaAkoNijePrvoPredavanjeTest()
        {
            byte razred = 1;
            int oznakaOdeljenje = 1;
            int predmetId = 1;

            var mockContext = new Mock<ApplicationDbContext>();

            var statusi = new List<Status>()
            {
                new Status() { StatusId=1,Opis="Arhivirano"},
                new Status() {StatusId=2,Opis="U toku" },
                new Status() {StatusId=3,Opis="Kreirano" }
            }.AsQueryable();

            var mockSetStatus = new Mock<DbSet<Status>>();
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.Provider).Returns(statusi.Provider);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.Expression).Returns(statusi.Expression);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.ElementType).Returns(statusi.ElementType);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.GetEnumerator()).Returns(statusi.GetEnumerator());

            foreach (var item in statusi)
                mockSetStatus.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Statusi).Returns(mockSetStatus.Object);

            var predmeti = new List<Predmet>
            {
                new Predmet() {NazivPredmeta="Matematika",PredmetID=predmetId }
            }.AsQueryable();

            var mockSetPredmet = new Mock<DbSet<Predmet>>();
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(predmeti.Provider);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(predmeti.Expression);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(predmeti.ElementType);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(predmeti.GetEnumerator());
            foreach (var item in predmeti)
                mockSetPredmet.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Predmeti).Returns(mockSetPredmet.Object);

            var odeljenja = new List<Odeljenje>()
            {
                new Odeljenje() {Id=1,Oznaka=new Oznaka(),OznakaID=1,PocetakSkolskeGodine=2017, Predmeti=predmeti.ToList(), KrajSkolskeGodine=2018,Razred=1, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=2,Oznaka=new Oznaka(),OznakaID=6,PocetakSkolskeGodine=2018, Predmeti=predmeti.ToList(), KrajSkolskeGodine=2019,Razred=3, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=3,Oznaka=new Oznaka(),OznakaID=3,PocetakSkolskeGodine=2017, Predmeti=predmeti.ToList(), KrajSkolskeGodine=2018,Razred=4, Status=new Status(),StatusID=2 },

            }.AsQueryable();

            var mockSetOdeljenje = new Mock<DbSet<Odeljenje>>();
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Provider).Returns(odeljenja.Provider);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Expression).Returns(odeljenja.Expression);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.ElementType).Returns(odeljenja.ElementType);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.GetEnumerator()).Returns(odeljenja.GetEnumerator());

            foreach (var item in odeljenja)
                mockSetOdeljenje.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Odeljenja).Returns(mockSetOdeljenje.Object);

            var casovi = new List<Cas>
            {
                new Cas()
                {
                    CasId = 1,
                    Datum = DateTime.Today,
                    Naziv = "Uvod",
                    Odeljenje = odeljenja.First(),
                    RedniBrojCasa = 1,
                    Opis = "Ponavljanje gradiva",
                    RedniBrojPredmeta = 1,
                    OdeljenjeId=1,
                    PredmetId=predmetId,
                    Predmet = predmeti.First()
                }
            }.AsQueryable();

            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.GetEnumerator()).Returns(casovi.GetEnumerator());
            foreach (var item in casovi)
                mockSetCas.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Casovi).Returns(mockSetCas.Object);

            var casoviController = new CasoviController(mockContext.Object);

            var result = casoviController.VratiRedniBrojPredmeta(razred, oznakaOdeljenje, predmetId) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);
            Assert.AreEqual(2, result.Data);

        }

        //DONE
        [TestMethod()]
        public void VratiRedniBrojPredmetaAkoJePrvoPredavanjeTest()
        {
            byte razred = 1;
            int oznakaOdeljenje = 1;
            int predmetId = 1;

            var mockContext = new Mock<ApplicationDbContext>();

            var statusi = new List<Status>()
            {
                new Status() { StatusId=1,Opis="Arhivirano"},
                new Status() {StatusId=2,Opis="U toku" },
                new Status() {StatusId=3,Opis="Kreirano" }
            }.AsQueryable();

            var mockSetStatus = new Mock<DbSet<Status>>();
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.Provider).Returns(statusi.Provider);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.Expression).Returns(statusi.Expression);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.ElementType).Returns(statusi.ElementType);
            mockSetStatus.As<IQueryable<Status>>().Setup(m => m.GetEnumerator()).Returns(statusi.GetEnumerator());

            foreach (var item in statusi)
                mockSetStatus.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Statusi).Returns(mockSetStatus.Object);

            var predmeti = new List<Predmet>
            {
                new Predmet() {NazivPredmeta="Matematika",PredmetID=predmetId }
            }.AsQueryable();

            var mockSetPredmet = new Mock<DbSet<Predmet>>();
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(predmeti.Provider);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(predmeti.Expression);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(predmeti.ElementType);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(predmeti.GetEnumerator());
            foreach (var item in predmeti)
                mockSetPredmet.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Predmeti).Returns(mockSetPredmet.Object);

            var odeljenja = new List<Odeljenje>()
            {
                new Odeljenje() {Id=1,Oznaka=new Oznaka(),OznakaID=1,PocetakSkolskeGodine=2017, Predmeti=predmeti.ToList(), KrajSkolskeGodine=2018,Razred=1, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=2,Oznaka=new Oznaka(),OznakaID=6,PocetakSkolskeGodine=2018, Predmeti=predmeti.ToList(), KrajSkolskeGodine=2019,Razred=3, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=3,Oznaka=new Oznaka(),OznakaID=3,PocetakSkolskeGodine=2017, Predmeti=predmeti.ToList(), KrajSkolskeGodine=2018,Razred=4, Status=new Status(),StatusID=2 },

            }.AsQueryable();

            var mockSetOdeljenje = new Mock<DbSet<Odeljenje>>();
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Provider).Returns(odeljenja.Provider);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.Expression).Returns(odeljenja.Expression);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.ElementType).Returns(odeljenja.ElementType);
            mockSetOdeljenje.As<IQueryable<Odeljenje>>().Setup(m => m.GetEnumerator()).Returns(odeljenja.GetEnumerator());

            foreach (var item in odeljenja)
                mockSetOdeljenje.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Odeljenja).Returns(mockSetOdeljenje.Object);

            var casovi = new List<Cas>
            {
                new Cas()
                {
                    CasId = 1,
                    Datum = DateTime.Today,
                    Naziv = "Uvod",
                    Odeljenje = odeljenja.SingleOrDefault(x => x.Id == 2),
                    RedniBrojCasa = 1,
                    Opis = "Ponavljanje gradiva",
                    RedniBrojPredmeta = 1,
                    OdeljenjeId=2,
                    PredmetId=predmetId,
                    Predmet = predmeti.First()
                }
            }.AsQueryable();

            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.GetEnumerator()).Returns(casovi.GetEnumerator());
            foreach (var item in casovi)
                mockSetCas.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Casovi).Returns(mockSetCas.Object);

            var casoviController = new CasoviController(mockContext.Object);

            var result = casoviController.VratiRedniBrojPredmeta(razred, oznakaOdeljenje, predmetId) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);
            Assert.AreEqual(1, result.Data);

        }

        //DONE
        [TestMethod()]
        public void ProveraPostojanjaRednogBrojaCasaKojiPostojiTest()
        {
            byte razred = 1;
            int oznakaOdeljenje = 1;
            int status = 3;
            int predmetId = 1;
            int rbCasa = 1;
            var mockContext = new Mock<ApplicationDbContext>();


            var statusi = new List<Status>()
            {
                new Status() { StatusId=1,Opis="Arhivirano"},
                new Status() {StatusId=2,Opis="U toku" },
                new Status() {StatusId=3,Opis="Kreirano" }
            }.AsQueryable();

            var odeljenja = new List<Odeljenje>()
            {
                new Odeljenje() {Id=1,Oznaka=new Oznaka(),OznakaID=1,PocetakSkolskeGodine=2017, KrajSkolskeGodine=2018,Razred=1, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=2,Oznaka=new Oznaka(),OznakaID=6,PocetakSkolskeGodine=2018, KrajSkolskeGodine=2019,Razred=3, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=3,Oznaka=new Oznaka(),OznakaID=3,PocetakSkolskeGodine=2017, KrajSkolskeGodine=2018,Razred=4, Status=new Status(),StatusID=2 },

            }.AsQueryable();

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

            var casovi = new List<Cas>
            {
                new Cas()
                {
                    CasId = 1,
                    Datum = DateTime.Today,
                    Naziv = "Uvod",
                    Odeljenje = odeljenja.First(),
                    RedniBrojCasa = rbCasa,
                    Opis = "Ponavljanje gradiva",
                    RedniBrojPredmeta = 1,
                    OdeljenjeId=1
                }
            }.AsQueryable();

            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.GetEnumerator()).Returns(casovi.GetEnumerator());
            foreach (var item in casovi)
                mockSetCas.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Casovi).Returns(mockSetCas.Object);

            var casoviController = new CasoviController(mockContext.Object);

            var result = casoviController.ProveraPostojanjaRednogBrojaCasa(razred, oznakaOdeljenje, rbCasa) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);
            Assert.AreEqual(0, result.Data);

        }

        //DONE
        [TestMethod()]
        public void ProveraPostojanjaRednogBrojaCasaKojiNePostojiTest()
        {
            byte razred = 1;
            int oznakaOdeljenje = 1;
            int status = 3;
            int predmetId = 1;
            int rbCasa = 1;
            var mockContext = new Mock<ApplicationDbContext>();


            var statusi = new List<Status>()
            {
                new Status() { StatusId=1,Opis="Arhivirano"},
                new Status() {StatusId=2,Opis="U toku" },
                new Status() {StatusId=3,Opis="Kreirano" }
            }.AsQueryable();

            var odeljenja = new List<Odeljenje>()
            {
                new Odeljenje() {Id=1,Oznaka=new Oznaka(),OznakaID=1,PocetakSkolskeGodine=2017, KrajSkolskeGodine=2018,Razred=1, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=2,Oznaka=new Oznaka(),OznakaID=6,PocetakSkolskeGodine=2018, KrajSkolskeGodine=2019,Razred=3, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=3,Oznaka=new Oznaka(),OznakaID=3,PocetakSkolskeGodine=2017, KrajSkolskeGodine=2018,Razred=4, Status=new Status(),StatusID=2 },

            }.AsQueryable();

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

            var casovi = new List<Cas>
            {
                new Cas()
                {
                    CasId = 1,
                    Datum = DateTime.Today,
                    Naziv = "Uvod",
                    Odeljenje = odeljenja.First(),
                    RedniBrojCasa = 2,
                    Opis = "Ponavljanje gradiva",
                    RedniBrojPredmeta = 1,
                    OdeljenjeId=1
                }
            }.AsQueryable();

            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.GetEnumerator()).Returns(casovi.GetEnumerator());
            foreach (var item in casovi)
                mockSetCas.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Casovi).Returns(mockSetCas.Object);

            var casoviController = new CasoviController(mockContext.Object);

            var result = casoviController.ProveraPostojanjaRednogBrojaCasa(razred, oznakaOdeljenje, rbCasa) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);
            Assert.AreEqual(1, result.Data);

        }

        //DONE
        [TestMethod()]
        public void ProveraPostojanjaRednogBrojaPredmetaKojiVecPostojiTest()
        {

            byte razred = 1;
            int oznakaOdeljenje = 1;
            int status = 3;
            int predmetId = 1;
            int rbPredmeta = 1;

            var predmeti = new List<Predmet>
            {
                new Predmet() {NazivPredmeta="Matematika",PredmetID=predmetId }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Predmet>>();
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(predmeti.Provider);
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(predmeti.Expression);
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(predmeti.ElementType);
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(predmeti.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(c => c.Predmeti).Returns(mockSet.Object);

            var statusi = new List<Status>()
            {
                new Status() { StatusId=1,Opis="Arhivirano"},
                new Status() {StatusId=2,Opis="U toku" },
                new Status() {StatusId=3,Opis="Kreirano" }
            }.AsQueryable();

            var odeljenja = new List<Odeljenje>()
            {
                new Odeljenje() {Id=1,Oznaka=new Oznaka(),OznakaID=1,PocetakSkolskeGodine=2017, Predmeti = predmeti.ToList(),KrajSkolskeGodine=2018,Razred=1, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=2,Oznaka=new Oznaka(),OznakaID=6,PocetakSkolskeGodine=2018, Predmeti = predmeti.ToList(),KrajSkolskeGodine=2019,Razred=3, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=3,Oznaka=new Oznaka(),OznakaID=3,PocetakSkolskeGodine=2017, Predmeti = predmeti.ToList(),KrajSkolskeGodine=2018,Razred=4, Status=new Status(),StatusID=2 },

            }.AsQueryable();

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

            var casovi = new List<Cas>
            {
                new Cas()
                {
                    CasId = 1,
                    Datum = DateTime.Today,
                    Naziv = "Uvod",
                    Odeljenje = odeljenja.First(),
                    RedniBrojCasa = 1,
                    Opis = "Ponavljanje gradiva",
                    Predmet = predmeti.First(),
                    RedniBrojPredmeta = rbPredmeta
                }
            }.AsQueryable();

            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.GetEnumerator()).Returns(casovi.GetEnumerator());
            foreach (var item in casovi)
                mockSetCas.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Casovi).Returns(mockSetCas.Object);

            var casoviController = new CasoviController(mockContext.Object);

            var result = casoviController.ProveraPostojanjaRednogBrojaPredmeta(razred, oznakaOdeljenje, predmetId, rbPredmeta) as JsonResult;


            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);
            Assert.AreEqual(0, result.Data);

        }

        //DONE
        [TestMethod()]
        public void ProveraPostojanjaRednogBrojaPredmetaKojiNePostojiTest()
        {

            byte razred = 1;
            int oznakaOdeljenje = 1;
            int status = 3;
            int predmetId = 1;
            int rbPredmeta = 1;

            var predmeti = new List<Predmet>
            {
                new Predmet() {NazivPredmeta="Matematika",PredmetID=predmetId }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Predmet>>();
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(predmeti.Provider);
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(predmeti.Expression);
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(predmeti.ElementType);
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(predmeti.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(c => c.Predmeti).Returns(mockSet.Object);

            var statusi = new List<Status>()
            {
                new Status() { StatusId=1,Opis="Arhivirano"},
                new Status() {StatusId=2,Opis="U toku" },
                new Status() {StatusId=3,Opis="Kreirano" }
            }.AsQueryable();

            var odeljenja = new List<Odeljenje>()
            {
                new Odeljenje() {Id=1,Oznaka=new Oznaka(),OznakaID=1,PocetakSkolskeGodine=2017, Predmeti = predmeti.ToList(),KrajSkolskeGodine=2018,Razred=1, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=2,Oznaka=new Oznaka(),OznakaID=6,PocetakSkolskeGodine=2018, Predmeti = predmeti.ToList(),KrajSkolskeGodine=2019,Razred=3, Status=new Status(),StatusID=3 },
                new Odeljenje() {Id=3,Oznaka=new Oznaka(),OznakaID=3,PocetakSkolskeGodine=2017, Predmeti = predmeti.ToList(),KrajSkolskeGodine=2018,Razred=4, Status=new Status(),StatusID=2 },

            }.AsQueryable();

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

            var casovi = new List<Cas>
            {
                new Cas()
                {
                    CasId = 1,
                    Datum = DateTime.Today,
                    Naziv = "Uvod",
                    Odeljenje = odeljenja.First(),
                    RedniBrojCasa = 2,
                    Opis = "Ponavljanje gradiva",
                    Predmet = predmeti.First(),
                    RedniBrojPredmeta = 2
                }
            }.AsQueryable();

            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(m => m.GetEnumerator()).Returns(casovi.GetEnumerator());
            foreach (var item in casovi)
                mockSetCas.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Casovi).Returns(mockSetCas.Object);

            var casoviController = new CasoviController(mockContext.Object);

            var result = casoviController.ProveraPostojanjaRednogBrojaPredmeta(razred, oznakaOdeljenje, predmetId, rbPredmeta) as JsonResult;


            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);
            Assert.AreEqual(1, result.Data);

        }

        [Ignore]
        [TestMethod()]
        public void VratiOceneTest()
        {
            Assert.Fail();
        }

        [Ignore]
        [TestMethod()]
        public void VratiPredmeteIDTest()
        {
            Assert.Fail();
        }
    }
}