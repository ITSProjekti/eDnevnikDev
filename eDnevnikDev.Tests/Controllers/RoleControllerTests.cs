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
using System.Web;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using System.Net;

namespace eDnevnikDev.Tests.Controllers
{
    /// <summary>
    /// Summary description for RoleControllerTests
    /// </summary>
    [TestClass]
    public class RoleControllerTests
    {

        Mock<ApplicationDbContext> mockContext;
        RoleController controler;
        public RoleControllerTests()
        {
            mockContext = new Mock<ApplicationDbContext>();
            controler = new RoleController(mockContext.Object);
        }
        
        [TestMethod]
        public void Index()
        {
            var result = controler.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void VratiProfesore()
        {

            var profesori = new List<Profesor>
            {
                new Profesor() { ProfesorID = 1, Ime="Profesor",Prezime="Profesoric",Adresa="Adresa 1", Telefon="0654182374",Vanredan = true,UserProfesorId="1111111"},
                new Profesor() { ProfesorID = 2, Ime="Profesor2",Prezime="Profesoric2",Adresa="Adresa 12", Telefon="767894272",Vanredan = true,UserProfesorId="22222222" },
                new Profesor() { ProfesorID = 3, Ime="Profesor3",Prezime="Profesoric3",Adresa="Adresa 123", Telefon="0239042322",Vanredan = true,UserProfesorId="33333333" }

            }.AsQueryable();
            var role = new List<string>()
            {
                "Profesor",
                "Admin"
            } as IList<string>;
            var mockSetProfesor = new Mock<DbSet<Profesor>>();
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Provider).Returns(profesori.Provider);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Expression).Returns(profesori.Expression);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.ElementType).Returns(profesori.ElementType);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.GetEnumerator()).Returns(profesori.GetEnumerator());
            mockContext.Setup(x => x.Profesori).Returns(mockSetProfesor.Object);

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var appUserManager = new Mock<ApplicationUserManager>(userStore.Object);
            appUserManager.Setup(x => x.GetRolesAsync(It.IsAny<string>())).ReturnsAsync(role);
            controler._userManager = appUserManager.Object;
            var result = controler.VratiProfesore() as Task<JsonResult>;

            //Assert result
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);

            string jsonString = JsonConvert.SerializeObject(result.Result.Data);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var jsonArray = JArray.Parse(Helper.checkJsonJArray(jsonString));
            var i = 0;
            foreach (JObject item in jsonArray)
            {
               
                var id = item["Id"].ToString();
                var ime = item["Ime"].ToString();
                var prezime = item["Prezime"].ToString();
                var roleTemp = item.GetValue("Role");
                List<string> listaRola = new List<string>();
                foreach (JValue itemInner in roleTemp)
                {
                    listaRola.Add(itemInner.ToString());
                }
                Profesor temp = profesori.ElementAtOrDefault(i++);
                Assert.AreEqual(id, temp.UserProfesorId);
                Assert.AreEqual(ime, temp.Ime);
                Assert.AreEqual(prezime, temp.Prezime);
                Assert.AreEqual(listaRola[0],role[0].ToString());
                Assert.AreEqual(listaRola[1],role[1].ToString());

            }
            Assert.AreEqual(jsonArray.Count(), 3);


        }

        [TestMethod]
        public void VratiUcenike()
        {
            var ucenici = new List<Ucenik>
            {
                new Ucenik() { UcenikID = 1, UserUcenikId="13214332", Ime="Ucenik",Prezime="Ucenicic",JMBG="1231243214"},
                new Ucenik() { UcenikID = 2,UserUcenikId="43535312", Ime="Ucenik2",Prezime="Ucenicic",JMBG="4523322413"},
                new Ucenik() { UcenikID = 3,UserUcenikId="63242234", Ime="Ucenik3",Prezime="Ucenicic",JMBG="6575612134" }

            }.AsQueryable();
            var role = new List<string>()
            {
                "Ucenik",
                "Admin"
            } as IList<string>;
            var mockSetUcenik = new Mock<DbSet<Ucenik>>();
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.Provider).Returns(ucenici.Provider);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.Expression).Returns(ucenici.Expression);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.ElementType).Returns(ucenici.ElementType);
            mockSetUcenik.As<IQueryable<Ucenik>>().Setup(m => m.GetEnumerator()).Returns(ucenici.GetEnumerator());
            mockContext.Setup(x => x.Ucenici).Returns(mockSetUcenik.Object);

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var appUserManager = new Mock<ApplicationUserManager>(userStore.Object);
            appUserManager.Setup(x => x.GetRolesAsync(It.IsAny<string>())).ReturnsAsync(role);
            controler._userManager = appUserManager.Object;
            var result = controler.VratiUcenike() as Task<JsonResult>;

            //Assert result
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.JsonRequestBehavior == JsonRequestBehavior.AllowGet);

            string jsonString = JsonConvert.SerializeObject(result.Result.Data);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var jsonArray = JArray.Parse(Helper.checkJsonJArray(jsonString));
            var i = 0;
            foreach (JObject item in jsonArray)
            {

                var id = item["Id"].ToString();
                var jmbg = item["JMBG"].ToString();
                var ime = item["Ime"].ToString();
                var prezime = item["Prezime"].ToString();
                var roleTemp = item.GetValue("Role");
                List<string> listaRola = new List<string>();
                foreach (JValue itemInner in roleTemp)
                {
                    listaRola.Add(itemInner.ToString());
                }
                Ucenik temp = ucenici.ElementAtOrDefault(i++);
                Assert.AreEqual(id, temp.UserUcenikId);
                Assert.AreEqual(jmbg, temp.JMBG);
                Assert.AreEqual(ime, temp.Ime);
                Assert.AreEqual(prezime, temp.Prezime);
                Assert.AreEqual(listaRola[0], role[0].ToString());
                Assert.AreEqual(listaRola[1], role[1].ToString());

            }
            Assert.AreEqual(jsonArray.Count(), 3);
        }
        [TestMethod]
        public void PromeniPravoPristupaProfesora()
        {
            string id = "22222222";
            var profesori = new List<Profesor>
            {
                new Profesor() { ProfesorID = 1, Ime="Profesor",Prezime="Profesoric",Adresa="Adresa 1", Telefon="0654182374",Vanredan = true,UserProfesorId="1111111"},
                new Profesor() { ProfesorID = 2, Ime="Profesor2",Prezime="Profesoric2",Adresa="Adresa 12", Telefon="767894272",Vanredan = true,UserProfesorId="22222222" },
                new Profesor() { ProfesorID = 3, Ime="Profesor3",Prezime="Profesoric3",Adresa="Adresa 123", Telefon="0239042322",Vanredan = true,UserProfesorId="33333333" }

            }.AsQueryable();
            var role = new List<string>()
            {
                "Profesor",
                "Admin",
            } as IList<string>;

            var mockSetProfesor = new Mock<DbSet<Profesor>>();
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Provider).Returns(profesori.Provider);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Expression).Returns(profesori.Expression);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.ElementType).Returns(profesori.ElementType);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.GetEnumerator()).Returns(profesori.GetEnumerator());
            mockContext.Setup(x => x.Profesori).Returns(mockSetProfesor.Object);

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var appUserManager = new Mock<ApplicationUserManager>(userStore.Object);
            appUserManager.Setup(x => x.GetRolesAsync(It.IsAny<string>())).ReturnsAsync(role);
            controler._userManager = appUserManager.Object;
            var result = controler.VratiUcenike() as Task<JsonResult>;

            //Assert
            var resultNullId = controler.PromeniPravoPristupaProfesora(null);
            Assert.AreEqual((HttpStatusCodeResult)resultNullId, new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            //var result = controler.PromeniPravoPristupaProfesora(id);

        }
    }
}
