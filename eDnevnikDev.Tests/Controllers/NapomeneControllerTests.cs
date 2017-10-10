using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Data.Entity;
using System.Web.Mvc;
using eDnevnikDev.ViewModel;
using eDnevnikDev.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using eDnevnikDev.Tests.helpers;
using eDnevnikDev.Models;
using eDnevnikDev.Controllers;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using eDnevnikDev.Tests.Helpers;

namespace eDnevnikDev.Tests.Controllers
{
    [TestClass]
    public class NapomeneControllerTests
    {
        [TestMethod]
        public void NapomeneController_DodajIliIzmeniNapomenu()
        {
            var dtoNapomena = new DTONapomena() { NapomenaId = 1, Opis = "Napomena 1", CasId = 1, ProfesorId = 1, UcenikId = 2 };

            var mockContext = new Mock<ApplicationDbContext>();
            var mockSetNapomene = new Mock<MockableDbSet<Napomena>>();
            DbSet<Napomena> dbSetNapomene = mockSetNapomene.Object;
            mockContext.Setup(x => x.Napomene).Returns(dbSetNapomene);
            var controller = new NapomeneController(mockContext.Object);
           

            controller.DodajIliIzmeniNapomenu(dtoNapomena);

            mockSetNapomene.Verify(x => x.AddOrUpdate(It.IsAny<Napomena>()), Times.AtLeastOnce());
            mockContext.Verify(x => x.SaveChanges(), Times.AtLeastOnce());
            

        }

        private void X_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void NapomeneController_VratiNapomenu()
        {
            //Assign and mock
            var ucenikId = 1;
            var casId = 2;

            var listNapomena = new List<Napomena>()
            {
                new Napomena() { NapomenaId = 1, Opis = "Napomena 1", CasId = 2, ProfesorId = 1, UcenikId = 1 },
                new Napomena() { NapomenaId = 2, Opis = "Napomena 2", CasId = 1, ProfesorId = 3, UcenikId = 3 },
                new Napomena() { NapomenaId = 3, Opis = "Napomena 3", CasId = 3, ProfesorId = 3, UcenikId = 1 },
                new Napomena() { NapomenaId = 4, Opis = "Napomena 4", CasId = 2, ProfesorId = 2, UcenikId = 4 },

            }.AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();
            var mockSetNapomene = new Mock<DbSet<Napomena>>();
            mockSetNapomene.As<IQueryable<Napomena>>().Setup(x => x.Expression).Returns(listNapomena.Expression);
            mockSetNapomene.As<IQueryable<Napomena>>().Setup(x => x.ElementType).Returns(listNapomena.ElementType);
            mockSetNapomene.As<IQueryable<Napomena>>().Setup(x => x.Provider).Returns(listNapomena.Provider);
            mockSetNapomene.As<IQueryable<Napomena>>().Setup(x => x.GetEnumerator()).Returns(listNapomena.GetEnumerator());
            mockContext.Setup(x => x.Napomene).Returns(mockSetNapomene.Object);

            var controler = new NapomeneController(mockContext.Object);
            var result = controler.VratiNapomenu(ucenikId, casId) as JsonResult;

            //Assert result
            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);

            //Test JSON
            string jsonString = JsonConvert.SerializeObject(result.Data);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var jsonArray = JArray.Parse(Helper.checkJsonJArray(jsonString));
            foreach (JObject item in jsonArray)
            {
                //var ocene=item.GetValue("")
                //foreach (var item in collection)
                //{

                //}
                var napomenaId = item["NapomenaId"].ToString();
                var opis = item["Opis"].ToString();
                var _ucenikId = item["UcenikId"].ToString();
                var profesorId = item["ProfesorId"].ToString();
                var _casId = item["CasId"].ToString();



                Napomena temp = listNapomena.FirstOrDefault(x => x.NapomenaId == int.Parse(napomenaId));
                Assert.AreEqual<string>(napomenaId, temp.NapomenaId.ToString());
                Assert.AreEqual<string>(opis, temp.Opis.ToString());
                Assert.AreEqual<string>(_ucenikId, temp.UcenikId.ToString());
                Assert.AreEqual<string>(profesorId, temp.ProfesorId.ToString());
                Assert.AreEqual<string>(_casId, temp.CasId.ToString());
                Assert.AreEqual<int>(jsonArray.Count(), 1);


            }
        }
    }
}
