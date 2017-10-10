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
    public class EvidencijaControllerTests
    {
        [TestMethod]
        public void VratiBrojcaneOcene()
        {
            int? PredmetId = 1;
            int? OdeljenjeId= 2;
            int? ProfesorId= 3;
            int? UcenikId = 1;

            var casovi = new List<Cas> {
                new Cas { CasId = 1, Naziv = "cas 1", PredmetId = 1, OdeljenjeId = 2, ProfesorId = 3 },
                new Cas { CasId = 2, Naziv = "cas 2", PredmetId = 3, OdeljenjeId = 1, ProfesorId = 5 }
            }.AsQueryable();
            var tipOcena = new List<TipOcene> {
                new TipOcene { TipOceneId=1,Tip="kontrolni"},
                new TipOcene { TipOceneId=2,Tip="pismeni" }}.AsQueryable();

            var ocene = new List<Ocena> {
                new Ocena { CasId = 1, Oznaka = 5, Napomena = "napomena 1", TipOceneId = 2,UcenikId=1 },
                new Ocena { CasId=1,Oznaka=4,Napomena="napomena 2",TipOceneId=1,UcenikId=1 } }.AsQueryable();
            

            var mockContext = new Mock<ApplicationDbContext>();
            //Mock cas
            var mockSetCas = new Mock<DbSet<Cas>>();
            mockSetCas.As<IQueryable<Cas>>().Setup(x => x.Provider).Returns(casovi.Provider);
            mockSetCas.As<IQueryable<Cas>>().Setup(x => x.Expression).Returns(casovi.Expression);
            mockSetCas.As<IQueryable<Cas>>().Setup(x => x.ElementType).Returns(casovi.ElementType);
            mockSetCas.As<IQueryable<Cas>>().Setup(x => x.GetEnumerator()).Returns(casovi.GetEnumerator());
            mockContext.Setup(x => x.Casovi).Returns(mockSetCas.Object);
            //Mock ocene
            var mockSetOcene = new Mock<DbSet<Ocena>>();
            mockSetCas.As<IQueryable<Ocena>>().Setup(x => x.Provider).Returns(ocene.Provider);
            mockSetCas.As<IQueryable<Ocena>>().Setup(x => x.Expression).Returns(ocene.Expression);
            mockSetCas.As<IQueryable<Ocena>>().Setup(x => x.ElementType).Returns(ocene.ElementType);
            mockSetCas.As<IQueryable<Ocena>>().Setup(x => x.GetEnumerator()).Returns(ocene.GetEnumerator());
            mockContext.Setup(x => x.Ocene).Returns(mockSetOcene.Object);
            //Mock tip ocene
            var mockSetTipOcene = new Mock<DbSet<TipOcene>>();
            mockSetOcene.As<IQueryable<TipOcene>>().Setup(x => x.Provider).Returns(tipOcena.Provider);
            mockSetOcene.As<IQueryable<TipOcene>>().Setup(x => x.Expression).Returns(tipOcena.Expression);
            mockSetOcene.As<IQueryable<TipOcene>>().Setup(x => x.ElementType).Returns(tipOcena.ElementType);
            mockSetOcene.As<IQueryable<TipOcene>>().Setup(x => x.GetEnumerator()).Returns(tipOcena.GetEnumerator());
            mockContext.Setup(x => x.TipoviOcena).Returns(mockSetTipOcene.Object);

            var controller = new EvidencijaController(mockContext.Object);
            var result = controller.VratiBrojcaneOcene(OdeljenjeId, ProfesorId, PredmetId, UcenikId) as JsonResult;
            //Assert result
            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);
            //Test json output

            string jsonString = JsonConvert.SerializeObject(result.Data);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var jsonArray = JArray.Parse(Helper.checkJsonJArray(jsonString));
            foreach (JObject item in jsonArray)
            {
                foreach(JObject item2 in item)
                {

                }
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
