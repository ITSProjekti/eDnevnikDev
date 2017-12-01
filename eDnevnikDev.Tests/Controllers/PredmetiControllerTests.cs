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
using eDnevnikDev.ViewModel;
using Newtonsoft.Json.Linq;
using eDnevnikDev.Tests.helpers;
using Newtonsoft.Json;

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
            var predmet = new KreiranjePredmetaViewModel();
            predmet.NazivPredmeta = "Matematika";

            var mockSet = new Mock<DbSet<Predmet>>();

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(p => p.Predmeti).Returns(mockSet.Object);

            var services = new PredmetiController(mockContext.Object);
            services.SacuvajPredmet(predmet);

            mockSet.Verify(p => p.Add(It.IsAny<Predmet>()), Times.Once());
            mockContext.Verify(p => p.SaveChanges(), Times.Once());
        }

       
        // DONE
        [TestMethod()]
        public void PrikaziPredmeteSaTipovimaOcena()
        {
            var tipoviOcenaPredmeta = new List<TipOcenePredmeta>()
            {
                new TipOcenePredmeta() {TipOcenePredmetaId=1, Tip="Numericka"},
                new TipOcenePredmeta() {TipOcenePredmetaId=2, Tip="Opisna" }
            }.AsQueryable();

            var predmeti = new List<Predmet>()
            {
                new Predmet() {PredmetID=1, NazivPredmeta="Matematika", TipOcenePredmetaId=1, TipOcenePredmeta=tipoviOcenaPredmeta.SingleOrDefault(x=>x.TipOcenePredmetaId==1) },
                new Predmet() {PredmetID=2, NazivPredmeta="Srpski jezik", TipOcenePredmetaId=1, TipOcenePredmeta=tipoviOcenaPredmeta.SingleOrDefault(x=>x.TipOcenePredmetaId==1) },
                new Predmet() {PredmetID=3, NazivPredmeta="Programiranje", TipOcenePredmetaId=1, TipOcenePredmeta=tipoviOcenaPredmeta.SingleOrDefault(x=>x.TipOcenePredmetaId==1) },
                new Predmet() {PredmetID=4, NazivPredmeta="Veronauka", TipOcenePredmetaId=2, TipOcenePredmeta=tipoviOcenaPredmeta.SingleOrDefault(x=>x.TipOcenePredmetaId==2)}
            }.AsQueryable();
            

            var mockSetPredmet = new Mock<DbSet<Predmet>>();
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(predmeti.Provider);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(predmeti.Expression);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(predmeti.ElementType);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(predmeti.GetEnumerator());

            var mockSetTipOcenePredmeta = new Mock<DbSet<TipOcenePredmeta>>();
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.Provider).Returns(tipoviOcenaPredmeta.Provider);
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.Expression).Returns(tipoviOcenaPredmeta.Expression);
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.ElementType).Returns(tipoviOcenaPredmeta.ElementType);
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.GetEnumerator()).Returns(tipoviOcenaPredmeta.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();

            foreach (var item in predmeti)
                mockSetPredmet.Setup(p => p.Add(item));
            
            mockContext.Setup(c => c.Predmeti).Returns(mockSetPredmet.Object);

            foreach (var item in tipoviOcenaPredmeta)
                mockSetTipOcenePredmeta.Setup(p => p.Add(item));

            mockContext.Setup(c => c.TipoviOcenaPredmeta).Returns(mockSetTipOcenePredmeta.Object);


            var kontroler = new PredmetiController(mockContext.Object);

            var result = kontroler.PrikaziPredmeteSaTipovimaOcena() as ViewResult;
            var model = result.Model as PredmetTipOcenePredmetaViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(predmeti.FirstOrDefault(), model.Predmeti.FirstOrDefault());
            Assert.AreEqual(tipoviOcenaPredmeta.FirstOrDefault(), model.TipoviOcenaPredmeta.FirstOrDefault());
        }

        /// TO BE DONE
        [TestMethod()]
        public void IzmenaTipaOcenaUPredmetu()
        {
            var predmetId = 1;
            var tipOcenePredmetaId = 1;

            var tipoviOcenaPredmeta = new List<TipOcenePredmeta>()
            {
                new TipOcenePredmeta() {TipOcenePredmetaId=1, Tip="Numericka"},
                new TipOcenePredmeta() {TipOcenePredmetaId=2, Tip="Opisna" }
            }.AsQueryable();

            var predmeti = new List<Predmet>()
            {
                new Predmet() {PredmetID=1, NazivPredmeta="Matematika", TipOcenePredmetaId=1, TipOcenePredmeta=tipoviOcenaPredmeta.SingleOrDefault(x=>x.TipOcenePredmetaId==1) },
                new Predmet() {PredmetID=2, NazivPredmeta="Srpski jezik", TipOcenePredmetaId=1, TipOcenePredmeta=tipoviOcenaPredmeta.SingleOrDefault(x=>x.TipOcenePredmetaId==1) },
                new Predmet() {PredmetID=3, NazivPredmeta="Programiranje", TipOcenePredmetaId=1, TipOcenePredmeta=tipoviOcenaPredmeta.SingleOrDefault(x=>x.TipOcenePredmetaId==1) },
                new Predmet() {PredmetID=4, NazivPredmeta="Veronauka", TipOcenePredmetaId=2, TipOcenePredmeta=tipoviOcenaPredmeta.SingleOrDefault(x=>x.TipOcenePredmetaId==2)}
            }.AsQueryable();


            var mockSetPredmet = new Mock<DbSet<Predmet>>();
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(predmeti.Provider);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(predmeti.Expression);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(predmeti.ElementType);
            mockSetPredmet.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(predmeti.GetEnumerator());

            var mockSetTipOcenePredmeta = new Mock<DbSet<TipOcenePredmeta>>();
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.Provider).Returns(tipoviOcenaPredmeta.Provider);
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.Expression).Returns(tipoviOcenaPredmeta.Expression);
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.ElementType).Returns(tipoviOcenaPredmeta.ElementType);
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.GetEnumerator()).Returns(tipoviOcenaPredmeta.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();

            foreach (var item in predmeti)
                mockSetPredmet.Setup(p => p.Add(item));

            mockContext.Setup(c => c.Predmeti).Returns(mockSetPredmet.Object);

            foreach (var item in tipoviOcenaPredmeta)
                mockSetTipOcenePredmeta.Setup(p => p.Add(item));

            mockContext.Setup(c => c.TipoviOcenaPredmeta).Returns(mockSetTipOcenePredmeta.Object);

            //_mockUserRepository.Setup(mr => mr.Update(It.IsAny<int>(), It.IsAny<string>()))
            //       .Returns(true);

            var kontroler = new PredmetiController(mockContext.Object);

            kontroler.IzmenaTipaOcenaUPredmetu(predmetId, tipOcenePredmetaId);

            //mockSetPredmet.Verify(p => p.Add(It.IsAny<Predmet>()), Times.Once());
            mockContext.Verify(p => p.SaveChanges(), Times.Once());

        }

        //DONE
        [TestMethod]
        public void VratiSveTipoveOcenaPredmeta()
        {
            var tipoviOcenaPredmeta = new List<TipOcenePredmeta>()
            {
                new TipOcenePredmeta() {TipOcenePredmetaId=1, Tip="Brojcana" },
                new TipOcenePredmeta() {TipOcenePredmetaId=2, Tip="Numericka" }
            }.AsQueryable();

            var mockSetTipOcenePredmeta = new Mock<DbSet<TipOcenePredmeta>>();
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.Provider).Returns(tipoviOcenaPredmeta.Provider);
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.Expression).Returns(tipoviOcenaPredmeta.Expression);
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.ElementType).Returns(tipoviOcenaPredmeta.ElementType);
            mockSetTipOcenePredmeta.As<IQueryable<TipOcenePredmeta>>().Setup(m => m.GetEnumerator()).Returns(tipoviOcenaPredmeta.GetEnumerator());


            var mockContext = new Mock<ApplicationDbContext>();
            foreach (var item in tipoviOcenaPredmeta)
                mockSetTipOcenePredmeta.Setup(t => t.Add(item));
            mockContext.Setup(x => x.TipoviOcenaPredmeta).Returns(mockSetTipOcenePredmeta.Object);

            var controller = new PredmetiController(mockContext.Object);

            var result = controller.VratiSveTipoveOcenaPredmeta() as JsonResult;

            //Assert result
            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);

            //Test JSON
            string jsonString = JsonConvert.SerializeObject(result.Data);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var jsonArray = JArray.Parse(Helper.checkJsonJArray(jsonString));
            foreach (JObject item in jsonArray)
            {
                var tipOcenePredmetaId = item["TipOcenePredmetaId"].ToString();
                var tip = item["Tip"].ToString();

                TipOcenePredmeta temp = tipoviOcenaPredmeta.FirstOrDefault(t => t.TipOcenePredmetaId == int.Parse(tipOcenePredmetaId));
                Assert.AreEqual<string>(tipOcenePredmetaId, temp.TipOcenePredmetaId.ToString());
                Assert.AreEqual<string>(tip, temp.Tip.ToString());
                Assert.AreEqual<int>(jsonArray.Count(), 2);

            }

        }
    }
}