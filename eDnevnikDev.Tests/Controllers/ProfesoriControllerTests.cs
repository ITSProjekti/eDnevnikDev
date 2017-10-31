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

namespace eDnevnikDev.Controllers.Tests
{
    [TestClass]
    public class ProfesoriControllerTests
    {
        [TestMethod]
        public void ProfesoriController_Index()
        {
            var data = new List<Profesor>
            {
                new Profesor() { ProfesorID = 1, Ime="Profesor",Prezime="Profesoric",Adresa="Adresa 1", Telefon="0654182374",Vanredan = true }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Profesor>>();
            mockSet.As<IQueryable<Profesor>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Profesor>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Profesor>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Profesor>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(c => c.Profesori).Returns(mockSet.Object);

            var kontroler = new ProfesoriController(mockContext.Object);

            var result = kontroler.Index() as ViewResult;
            var model = result.Model as IEnumerable<Profesor>;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Count());
            Assert.AreEqual("Index", result.ViewName);

        }

        [TestMethod()]
        public void ProfesoriController_Dodaj()
        {
            var data = new List<Predmet>
            {
                new Predmet() {NazivPredmeta="Matematika",PredmetID=1 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Predmet>>();
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(c => c.Predmeti).Returns(mockSet.Object);

            var kontroler = new ProfesoriController(mockContext.Object);

            var result = kontroler.Dodaj() as ViewResult;
            var model = result.Model as ViewModel.ProfesorViewModel;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Predmeti.Count());
            Assert.AreEqual("Dodaj", result.ViewName);

        }

        [TestMethod()]
        public async Task ProfesoriController_Sacuvaj()
        {

            var pred = new List<Predmet>()
                    {
                    new Predmet { PredmetID = 1, NazivPredmeta = "Mata" },
                    new Predmet { PredmetID = 2, NazivPredmeta = "Fizika" }
                     }.AsQueryable();



            Profesor prof = new Profesor()
            {
                ProfesorID = 1,
                Ime = "Marko",
                Prezime = "Markovic",
                Telefon = "064333333",
                Adresa = "Neka Adresa",
                Vanredan = true,
                RazredniStaresina = true,
                Predmeti = pred.ToList()

            };

            ProfesorViewModel pmv = new ProfesorViewModel()
            {
                Profesor = prof,
                PredmetiIDs = pred.Select(p => p.PredmetID).ToList()

            };


            var mockSet = new Mock<DbSet<Profesor>>();
            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(p => p.Profesori).Returns(mockSet.Object);


            var mockSetPredmeti = new Mock<DbSet<Predmet>>();
            mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(pred.Provider);
            mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(pred.Expression);
            mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(pred.ElementType);
            mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(pred.GetEnumerator());

            mockContext.Setup(p => p.Predmeti).Returns(mockSetPredmeti.Object);

            var service = new ProfesoriController(mockContext.Object);
            await service.Sacuvaj(pmv);
            mockSet.Verify(m => m.Add(It.IsAny<Profesor>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}