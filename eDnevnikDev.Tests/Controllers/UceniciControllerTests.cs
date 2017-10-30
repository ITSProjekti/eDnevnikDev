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
using eDnevnikDev.ViewModel;

namespace eDnevnikDev.Controllers.Tests
{
    [TestClass]
    public class UceniciControllerTests
    {
        [TestMethod]
        public void UceniciController_Index()
        {
            var data = new List<Ucenik>
            {
                new Ucenik {UcenikID = 1, Ime = "Petar", Prezime = "Petrovic", Adresa = "Kneza Milosa 12", BrojTelefonaRoditelja = "+381-11/1234567", ImeMajke = "Jovana", ImeOca = "Jovana", JMBG = "1234567891234", MestoPrebivalista = "Blace", MestoRodjenjaId = 1, OdeljenjeId = 1, PrezimeMajke = "Petrovic", PrezimeOca = "Petrovic", Razred = 1, SmerID = 2, Vanredan = false }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Ucenik>>();
            mockSet.As<IQueryable<Ucenik>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Ucenik>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Ucenik>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Ucenik>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Ucenici).Returns(mockSet.Object);

            var kontroler = new UceniciController(mockContext.Object);

            var result = kontroler.Index() as ViewResult;
            var model = result.Model as IEnumerable<Ucenik>;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Count());
            Assert.AreEqual("Index", result.ViewName);

        }

        //TO BE DONE
        //[TestMethod]
        //public void UceniciController_Dodaj()
        //{
        //    var gradovi = new List<Grad>
        //    {
        //        new Grad { Id = 1, Naziv = "Beograd" }, new Grad { Id = 2, Naziv = "Nis" }, new Grad { Id = 3, Naziv = "Kragujevac" }, new Grad { Id = 4, Naziv = "Subotica" }, new Grad { Id = 5, Naziv = "Novi Sad" }
        //    }.AsQueryable();
        //    var smerovi = new List<Smer>
        //    {
        //        ?
        //    }.AsQueryable();


        //    var mockSetGradovi = new Mock<DbSet<Grad>>();
        //    mockSetGradovi.As<IQueryable<Grad>>().Setup(m => m.Provider).Returns(gradovi.Provider);
        //    mockSetGradovi.As<IQueryable<Grad>>().Setup(m => m.Expression).Returns(gradovi.Expression);
        //    mockSetGradovi.As<IQueryable<Grad>>().Setup(m => m.ElementType).Returns(gradovi.ElementType);
        //    mockSetGradovi.As<IQueryable<Grad>>().Setup(m => m.GetEnumerator()).Returns(gradovi.GetEnumerator());

        //    var mockSetSmerovi = new Mock<DbSet<Smer>>();
        //    mockSetSmerovi.As<IQueryable<Smer>>().Setup(m => m.Provider).Returns(smerovi.Provider);
        //    mockSetSmerovi.As<IQueryable<Smer>>().Setup(m => m.Expression).Returns(smerovi.Expression);
        //    mockSetSmerovi.As<IQueryable<Smer>>().Setup(m => m.ElementType).Returns(smerovi.ElementType);
        //    mockSetSmerovi.As<IQueryable<Smer>>().Setup(m => m.GetEnumerator()).Returns(smerovi.GetEnumerator());

        //    mockSetSmerovi.Setup(m => m.Include(It.IsAny<String>())).Returns(mockSetSmerovi.Object);

        //    var mockContext = new Mock<ApplicationDbContext>();
        //    mockContext.Setup(c => c.Gradovi).Returns(mockSetGradovi.Object);
        //    mockContext.Setup(c => c.Smerovi).Returns(mockSetSmerovi.Object);

        //    var kontroler = new UceniciController(mockContext.Object);

        //    var result = kontroler.Dodaj() as ViewResult;
        //    var model = result.Model as UcenikViewModel;
        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(5, model.Gradovi.Count());
        //    Assert.AreEqual(2, model.Smerovi.Count());
        //    Assert.AreEqual("Dodaj", result.ViewName);

        //}

        //TO BE DONE
        [TestMethod]
        public void UceniciController_Sacuvaj()
        {

            var grad = new List<Grad>()
            {
                new Grad {Id=1,Naziv="Beograd" },
                new Grad {Id=2,Naziv="Novi Sad" }
            }.AsQueryable();

            var smer = new List<Smer>()
            {
                new Smer {SmerID=1,NazivSmera="Mesar",Trajanje=3 },
                new Smer {SmerID=2,NazivSmera="Tehnicar",Trajanje=6 }
            }.AsQueryable();

            Ucenik uce = new Ucenik()
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
                OdeljenjeId = 1,
                Razred = 1,
                Smer = new Smer(),
                SmerID = 1,
                UcenikID = 1,
                Vanredan = false

            };


            var ucenik = new UcenikViewModel()
            {
                Ucenik = uce,
                Gradovi = grad,
                Smerovi = smer
            };

            var mockSetGradovi = new Mock<DbSet<Grad>>();
            mockSetGradovi.As<IQueryable<Grad>>().Setup(m => m.Provider).Returns(grad.Provider);
            mockSetGradovi.As<IQueryable<Grad>>().Setup(m => m.Expression).Returns(grad.Expression);
            mockSetGradovi.As<IQueryable<Grad>>().Setup(m => m.ElementType).Returns(grad.ElementType);
            mockSetGradovi.As<IQueryable<Grad>>().Setup(m => m.GetEnumerator()).Returns(grad.GetEnumerator());

            var mockSetSmerovi = new Mock<DbSet<Smer>>();
            mockSetSmerovi.As<IQueryable<Smer>>().Setup(m => m.Provider).Returns(smer.Provider);
            mockSetSmerovi.As<IQueryable<Smer>>().Setup(m => m.Expression).Returns(smer.Expression);
            mockSetSmerovi.As<IQueryable<Smer>>().Setup(m => m.ElementType).Returns(smer.ElementType);
            mockSetSmerovi.As<IQueryable<Smer>>().Setup(m => m.GetEnumerator()).Returns(smer.GetEnumerator());




            var mockSet = new Mock<DbSet<Ucenik>>();
            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(p => p.Ucenici).Returns(mockSet.Object);

            var services = new UceniciController(mockContext.Object);
            //services.Sacuvaj(ucenik);

            mockSet.Verify(p => p.Add(It.IsAny<Ucenik>()), Times.Once());
            mockContext.Verify(p => p.SaveChanges(), Times.Once());

        }
    }
}