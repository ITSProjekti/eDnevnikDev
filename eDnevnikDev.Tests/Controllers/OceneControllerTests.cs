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
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace eDnevnikDev.Controllers.Tests
{
    [TestClass()]
    public class OceneControllerTests
    {
        //TO BE DONE
        
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
                UserProfesorId = "profesorHashID",
                Predmeti = predmeti.ToList()
                }
            }.AsQueryable();

            var mockContext = new Mock<ApplicationDbContext>();

            var username = "profesorHashID";

            var fakeHttpContext = new Mock<HttpContextBase>();
            var controllerContext = new Mock<ControllerContext>();

            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal);
            controllerContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            var mockIdentity = new Mock<IIdentity>();
            fakeHttpContext.SetupGet(x => x.User.Identity).Returns(mockIdentity.Object);
            fakeIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
            var _requestController = new OceneController(mockContext.Object);

            _requestController.ControllerContext = controllerContext.Object;

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
            //Set your controller ControllerContext with fake context

            _requestController.Predmeti();

            var result = _requestController.Predmeti() as ViewResult;
            var model = result.Model as List<Predmet>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model.Count);
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual(predmeti.First(), model.First());
        }
    }
        
    }
