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
        [TestMethod]
        public void OdeljenjeController_OdeljenjeUcenici()
        {
            byte razred = 1;
            int odeljenje = 1;

            var listaUce = new List<Ucenik>()
            {
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
                OdeljenjeId = odeljenje,
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
                OdeljenjeId = odeljenje,
                Razred = razred,
                Smer = new Smer(),
                SmerID = 1,
                UcenikID = 2,
                Vanredan = false }
            }.AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();
            var mockSetUcenik = new Mock<DbSet<Ucenik>>();
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.Provider).Returns(listaUce.Provider);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.Expression).Returns(listaUce.Expression);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.ElementType).Returns(listaUce.ElementType);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.GetEnumerator()).Returns(listaUce.GetEnumerator());

            foreach (var item in listaUce)
            {
                mockSetUcenik.Setup(p => p.Add(item));
            }

            mockContext.Setup(p => p.Ucenici).Returns(mockSetUcenik.Object);
            var odeljenjeController = new OdeljenjeController(mockContext.Object);
            var result = odeljenjeController.OdeljenjeUcenici(razred, odeljenje) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);

            string jsonString = JsonConvert.SerializeObject(result.Data);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var jsonArray = JArray.Parse(jsonString);

                foreach (JObject item in jsonArray)
                {
                    
                    string id = item["ID"].ToString();
                    string Ime = item["Ime"].ToString();
                    string Prezime = item["Prezime"].ToString();

                    Ucenik temp = listaUce.FirstOrDefault(x => x.UcenikID == int.Parse(id));
                    Assert.AreEqual<string>(id, temp.UcenikID.ToString());
                    Assert.AreEqual<string>(Ime, temp.Ime.ToString());
                    Assert.AreEqual<string>(Prezime, temp.Prezime.ToString());
                }
            Assert.AreEqual<int>(jsonArray.Count, 2);

        }
        
        [TestMethod]
        public void OdeljenjeController_OdeljenjeTrajanje()
        {
           
        }
    }

}
