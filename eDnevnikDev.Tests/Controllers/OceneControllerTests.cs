﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
//using System.Security.Claims;

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

            //// create mock principal
            //var mocks = new MockRepository(MockBehavior.Default);
            //Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            //mockPrincipal.SetupGet(p => p.Identity.Name).Returns("profesorHashID");
            //mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);
            //mockPrincipal.Setup(p => p.IsInRole("Profesor")).Returns(true);
            //mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

            //var context = new Mock<HttpContextBase>();
            //var mockIdentity = new Mock<IIdentity>();
            //context.SetupGet(x => x.User.Identity).Returns(mockIdentity.Object);
            //mockIdentity.Setup(x => x.Name).Returns("test_name");

            //var mock = new Mock<ControllerContext>();
            //var _controller = new OceneController
            //{
            //    ControllerContext = A.Fake<ControllerContext>()
            //};



            //var mockIdentity2 = new Mock<IIdentity>()

            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            var _requestController = new OceneController();

            //Set your controller ControllerContext with fake context
            _requestController.ControllerContext = controllerContext.Object;
            _requestController.Predmeti();



        //    var mockContext = new Mock<ApplicationDbContext>();

       


        //    var mockSetProfesor = new Mock<DbSet<Profesor>>();
        //    mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Provider).Returns(profesori.Provider);
        //    mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Expression).Returns(profesori.Expression);
        //    mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.ElementType).Returns(profesori.ElementType);
        //    mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.GetEnumerator()).Returns(profesori.GetEnumerator());
        //    mockContext.Setup(p => p.Profesori).Returns(mockSetProfesor.Object);
            
            


        //    var mockSetPredmeti = new Mock<DbSet<Predmet>>();
        //    mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.Provider).Returns(predmeti.Provider);
        //    mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.Expression).Returns(predmeti.Expression);
        //    mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.ElementType).Returns(predmeti.ElementType);
        //    mockSetPredmeti.As<IQueryable<Predmet>>().Setup(m => m.GetEnumerator()).Returns(predmeti.GetEnumerator());

        //    mockContext.Setup(p => p.Predmeti).Returns(mockSetPredmeti.Object);



        //    var service = new OceneController(mockContext.Object);
            
        //    var result = service.Predmeti() as ViewResult;
        //    var model = result.Model as List<Predmet>;

        //    //Assert.IsNotNull(result);
        //    //Assert.IsNotNull(model.Count);
        //    Assert.AreEqual(2, model.Count);
        //    Assert.AreEqual(predmeti.First(), model.First());
        //}

        ////TO BE DONE
        //[Ignore]
        //[TestMethod]
        //public void PredmetiTest_ProfesorNemaPredmet()
        //{
        //    var profesori = new List<Profesor>(){
        //        new Profesor
        //        {
        //            ProfesorID = 1,
        //        Ime = "Marko",
        //        Prezime = "Markovic",
        //        Telefon = "064333333",
        //        Adresa = "Neka Adresa",
        //        Vanredan = true,
        //        RazredniStaresina = true,
        //        UserProfesorId = "profesor"
        //        }
        //    }.AsQueryable();

        //    var mockContext = new Mock<ApplicationDbContext>();

        //    var mockSetProfesor = new Mock<DbSet<Profesor>>();
        //    mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Provider).Returns(profesori.Provider);
        //    mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.Expression).Returns(profesori.Expression);
        //    mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.ElementType).Returns(profesori.ElementType);
        //    mockSetProfesor.As<IQueryable<Profesor>>().Setup(m => m.GetEnumerator()).Returns(profesori.GetEnumerator());
        //    mockContext.Setup(p => p.Profesori).Returns(mockSetProfesor.Object);

        //    var service = new OceneController(mockContext.Object);

        //    var result = service.Predmeti() as ViewResult;
        //    var model = result.Model as List<Predmet>;

        //    Assert.AreEqual(0, model.Count);
        }
        
    }
}