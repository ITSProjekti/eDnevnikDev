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
using System.Web;
using System.Linq.Expressions;

namespace eDnevnikDev.Controllers.Tests
{
    [TestClass()]
    public class PredmetiControllerTests
    {
        /// <summary>
        /// Testira akciju <see cref="PredmetController_Index"/>, <see cref="PredmetiController"/>.
        /// </summary>
        [TestMethod()]
        public void PredmetController_Index()
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

            var kontroler = new PredmetiController(mockContext.Object);

            var result = kontroler.Index() as ViewResult;
            var model = result.Model as IEnumerable<Predmet>;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Count());
            Assert.AreEqual("Index", result.ViewName);

        }

        /// <summary>
        /// Testira akciju <see cref="PredmetiController_Dodaj"/>, <see cref="PredmetiController"/>.
        /// </summary>
        [TestMethod()]
        public void PredmetiController_Dodaj()
        {
            var kontroler = new PredmetiController();
            var rezultat = kontroler.Dodaj() as ViewResult;
            Assert.AreEqual("Dodaj", rezultat.ViewName);
        }

        /// <summary>
        /// Testira akciju <see cref="PredmetiController_SacuvajPredmet"/>, <see cref="PredmetiController"/>.
        /// </summary>
        [TestMethod()]
        public void PredmetiController_SacuvajPredmet()
        {
            var predmet = new Predmet();
            predmet.NazivPredmeta = "Matematika";

            var mockSet = new Mock<DbSet<Predmet>>();

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(p => p.Predmeti).Returns(mockSet.Object);

            var services = new PredmetiController(mockContext.Object);
            services.SacuvajPredmet(predmet);

            mockSet.Verify(p => p.Add(It.IsAny<Predmet>()), Times.Once());
              mockContext.Verify(p => p.SaveChanges(), Times.Once());
          


        }

    }
}