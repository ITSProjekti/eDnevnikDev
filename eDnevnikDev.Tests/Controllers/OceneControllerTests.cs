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
//using System.Security.Principal;
using System.Web;
//using System.Security.Claims;

namespace eDnevnikDev.Controllers.Tests
{
    [TestClass()]
    public class OceneControllerTests
    {
        //TO BE DONE
        [Ignore]
        [TestMethod()]
        public void PredmetiTest_VracaPredmete()
        {
            var predmeti = new List<Predmet>()
            {
                new Predmet {
                    PredmetID = 1,
                    NazivPredmeta = "Mata"
                },

                new Predmet {
                    PredmetID = 2,
                    NazivPredmeta = "IT"
                }
            }.AsQueryable();

            var profesori = new List<Profesor>(){
                new Profesor
                {
                    ProfesorID = 1,
                Ime = "Marko",
                Prezime = "Markovic",
                Telefon = "064333333",
                Adresa = "Neka Adresa",
                Vanredan = true,
                RazredniStaresina = true,
                UserProfesorId = "profesor",
                Predmeti = predmeti.ToList()
                }
            }.AsQueryable();

            //var context = new Mock<HttpContextBase>();
            //var mockIdentity = new Mock<IIdentity>();
            //context.SetupGet(x => x.User.Identity).Returns(mockIdentity.Object);
            //mockIdentity.Setup(u => u.Name).Returns("profesor");

            //var controllerContext = new Mock<ControllerContext>();
            //var principal = new Moq.Mock<IPrincipal>();
            ////principal.Setup(p => p.IsInRole("Administrator")).Returns(true);
            //principal.SetupGet(x => x.Identity.Name).Returns("profesor");
            //controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);



            //var fakeHttpContext = new Mock<HttpContextBase>();
            //var fakeIdentity = new GenericIdentity("User");
            //var principal = new GenericPrincipal(fakeIdentity, null);

            //fakeHttpContext.Setup(t => t.User).Returns(principal);
            //var controllerContext = new Mock<ControllerContext>();
            //controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            //_requestController = new RequestController();

            ////Set your controller ControllerContext with fake context
            //_requestController.ControllerContext = controllerContext.Object;


            //var context = new Mock<HttpContextBase>();
            //var mockIdentity = new Mock<IIdentity>();
            //context.SetupGet(x => x.User.Identity).Returns(mockIdentity.Object);
            //mockIdentity.Setup(x => x.Name).Returns("profesor");


            //var claim = new Claim("test", "profesor");
            //var mockIdentity =
            //    Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            //var mockContext = Mock.Of<ControllerContext>(cc => cc.HttpContext.User == mockIdentity);
            //var controller = new MyController()
            //{
            //    ControllerContext = mockContext
            //};



            //controller.User.Identity.GetUserId(); //returns "IdOfYourChoosing"
            


        var mockContext = new Mock<ApplicationDbContext>();
            
            var mockSetProfesor = new Mock<DbSet<Profesor>>();
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Provider).Returns(profesori.Provider);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Expression).Returns(profesori.Expression);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.ElementType).Returns(profesori.ElementType);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.GetEnumerator()).Returns(profesori.GetEnumerator());
            mockContext.Setup(p => p.Profesori).Returns(mockSetProfesor.Object);

            var mockSetPredmeti = new Mock<DbSet<Predmet>>();
            mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(predmeti.Provider);
            mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(predmeti.Expression);
            mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(predmeti.ElementType);
            mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(predmeti.GetEnumerator());

            mockContext.Setup(p => p.Predmeti).Returns(mockSetPredmeti.Object);



            var service = new OceneController(mockContext.Object);
            
            var result = service.Predmeti() as ViewResult;
            var model = result.Model as List<Predmet>;

            //Assert.IsNotNull(result);
            //Assert.IsNotNull(model.Count);
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual(predmeti.First(), model.First());
        }

        //TO BE DONE
        [Ignore]
        [TestMethod]
        public void PredmetiTest_ProfesorNemaPredmet()
        {
            var profesori = new List<Profesor>(){
                new Profesor
                {
                    ProfesorID = 1,
                Ime = "Marko",
                Prezime = "Markovic",
                Telefon = "064333333",
                Adresa = "Neka Adresa",
                Vanredan = true,
                RazredniStaresina = true,
                UserProfesorId = "profesor"
                }
            }.AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();

            var mockSetProfesor = new Mock<DbSet<Profesor>>();
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Provider).Returns(profesori.Provider);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Expression).Returns(profesori.Expression);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.ElementType).Returns(profesori.ElementType);
            mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.GetEnumerator()).Returns(profesori.GetEnumerator());
            mockContext.Setup(p => p.Profesori).Returns(mockSetProfesor.Object);

            var service = new OceneController(mockContext.Object);

            var result = service.Predmeti() as ViewResult;
            var model = result.Model as List<Predmet>;

            Assert.AreEqual(0, model.Count);
        }
        
    }
}