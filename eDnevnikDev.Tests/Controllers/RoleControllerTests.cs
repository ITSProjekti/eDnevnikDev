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
using System.Data.Entity.Core.Objects;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;

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
        public async Task PromeniPravoPristupaProfesora()
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

            //Assert
            var resultNullId = await controler.PromeniPravoPristupaProfesora(null);
            Assert.IsInstanceOfType(resultNullId, typeof(HttpStatusCodeResult));

            var result = await controler.PromeniPravoPristupaProfesora(id) as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.Model, typeof(ProfesorRoleViewModel));

            var dummyProfesor = profesori.FirstOrDefault(x => x.UserProfesorId== id);
            Assert.AreEqual((result.Model as ProfesorRoleViewModel).Ime,dummyProfesor.Ime);
            Assert.AreEqual((result.Model as ProfesorRoleViewModel).Prezime, dummyProfesor.Prezime);
            Assert.AreEqual((result.Model as ProfesorRoleViewModel).UserProfesorId, dummyProfesor.UserProfesorId);
            Assert.AreEqual((result.Model as ProfesorRoleViewModel).RolaEditor, false);
            Assert.AreEqual((result.Model as ProfesorRoleViewModel).RolaProfesor, true);
        }
        [TestMethod]
        public async Task PromeniPravoPristupaUcenika()
        {
            string id = "22222222";
            var ucenici = new List<Ucenik>
            {
                new Ucenik {UcenikID = 1,UserUcenikId="11111111", Ime = "Petar", Prezime = "Gajic", Adresa = "Kneza Milosa 12", BrojTelefonaRoditelja = "+381-11/1443467", ImeMajke = "Jovana", ImeOca = "Jovana", JMBG = "1234567891234", MestoPrebivalista = "Blace", MestoRodjenjaId = 1, OdeljenjeId = 1, PrezimeMajke = "Petrovic", PrezimeOca = "Petrovic", Razred = 1, SmerID = 2, Vanredan = false },
                new Ucenik {UcenikID = 2,UserUcenikId="22222222", Ime = "Marko", Prezime = "Milic", Adresa = "Jurija gagarina 2", BrojTelefonaRoditelja = "+381-11/1223337", ImeMajke = "Jovana", ImeOca = "Jovana", JMBG = "1234567891234", MestoPrebivalista = "Blace", MestoRodjenjaId = 1, OdeljenjeId = 1, PrezimeMajke = "Petrovic", PrezimeOca = "Petrovic", Razred = 1, SmerID = 2, Vanredan = false },
                new Ucenik {UcenikID = 3,UserUcenikId="33333333", Ime = "Milos", Prezime = "Djokic", Adresa = "Petroviceva 9", BrojTelefonaRoditelja = "+381-11/12555557", ImeMajke = "Jovana", ImeOca = "Jovana", JMBG = "1234567891234", MestoPrebivalista = "Blace", MestoRodjenjaId = 1, OdeljenjeId = 1, PrezimeMajke = "Petrovic", PrezimeOca = "Petrovic", Razred = 1, SmerID = 2, Vanredan = false }

            }.AsQueryable();
            var role = new List<string>()
            {
                "Ucenik",
                "Editor",
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

            //Assert
            var resultNullId = await controler.PromeniPravoPristupaUcenika(null);
            Assert.IsInstanceOfType(resultNullId, typeof(HttpStatusCodeResult));

            var result = await controler.PromeniPravoPristupaUcenika(id) as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.Model, typeof(UcenikRoleViewModel));

            var dummyUcenik = ucenici.FirstOrDefault(x => x.UserUcenikId == id);
            Assert.AreEqual((result.Model as UcenikRoleViewModel).Ime, dummyUcenik.Ime);
            Assert.AreEqual((result.Model as UcenikRoleViewModel).Prezime, dummyUcenik.Prezime);
            Assert.AreEqual((result.Model as UcenikRoleViewModel).UserUcenikId, dummyUcenik.UserUcenikId);
            Assert.AreEqual((result.Model as UcenikRoleViewModel).JMBG, dummyUcenik.JMBG);
            Assert.AreEqual((result.Model as UcenikRoleViewModel).RolaEditor, true);
            Assert.AreEqual((result.Model as UcenikRoleViewModel).RolaUcenik, true);
        }

        [TestMethod]
        public void DodajRolu()
        {
            //MOCKUJE USERE IIDENTITY
            var dto = new DTORola() { KorisnikID = "222222", Rola = "Editor" };
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var appUser = new Mock<UserManager<ApplicationUser>>(userStore.Object);
            var roles = new Mock<MockableIdentity<IdentityRole>>();
            //roles.Setup(x=>
            mockContext.Setup(x => x.Roles).Returns(roles.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();
            var controllerContext = new Mock<ControllerContext>();

            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            var username = "profesorHashID";

            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);
            controllerContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            var mockIdentity = new Mock<IIdentity>();
            fakeHttpContext.SetupGet(x => x.User.Identity).Returns(mockIdentity.Object);
            fakeIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
            var _requestController = new RoleController(mockContext.Object);

            _requestController.ControllerContext = controllerContext.Object;

            //mockContext.Setup(x => x.Users).Returns(appUserManager.Object);
            _requestController.DodajRolu(dto);
            roles.Verify(x => x.AddToRole(It.IsAny<string>(), It.IsAny<string>()),Times.AtLeastOnce());
            //appUser.Verify(x => x.AddToRole(dto.KorisnikID, dto.Rola), Times.AtLeastOnce());
        }
    }
}
