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


    }

}
