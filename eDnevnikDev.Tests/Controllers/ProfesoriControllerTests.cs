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
            Assert.AreEqual(1,model.Predmeti.Count());
            Assert.AreEqual("Dodaj", result.ViewName);

        }
    }
}