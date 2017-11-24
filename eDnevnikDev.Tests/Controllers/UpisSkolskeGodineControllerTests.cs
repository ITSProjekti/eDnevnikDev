using Microsoft.VisualStudio.TestTools.UnitTesting;
using eDnevnikDev.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eDnevnikDev.ViewModel;
using eDnevnikDev.Models;
using Moq;
using System.Data.Entity;

namespace eDnevnikDev.Controllers.Tests
{
    [TestClass()]
    public class UpisSkolskeGodineControllerTests
    {
        [TestMethod()]
        public void CreateTestSaDobrimPodacima()
        {
            // Arange
            var mockContext = new Mock<ApplicationDbContext>();

            var skolskeGodine = new List<SkolskaGodina>()
            {

            }.AsQueryable();

            var mockSetSkolskaGodina = new Mock<DbSet<SkolskaGodina>>();
            mockSetSkolskaGodina.As<IQueryable<SkolskaGodina>>().Setup(m => m.Provider).Returns(skolskeGodine.Provider);
            mockSetSkolskaGodina.As<IQueryable<SkolskaGodina>>().Setup(m => m.Expression).Returns(skolskeGodine.Expression);
            mockSetSkolskaGodina.As<IQueryable<SkolskaGodina>>().Setup(m => m.ElementType).Returns(skolskeGodine.ElementType);
            mockSetSkolskaGodina.As<IQueryable<SkolskaGodina>>().Setup(m => m.GetEnumerator()).Returns(()=>skolskeGodine.GetEnumerator());

            foreach (var item in skolskeGodine)
                mockSetSkolskaGodina.Setup(p => p.Add(item));
            mockContext.Setup(p => p.SkolskaGodine).Returns(mockSetSkolskaGodina.Object);

            var polugodista = new List<Polugodiste>()
            {

            }.AsQueryable();

            var mockSetPolugodiste = new Mock<DbSet<Polugodiste>>();
            mockSetPolugodiste.As<IQueryable<Polugodiste>>().Setup(m => m.Provider).Returns(polugodista.Provider);
            mockSetPolugodiste.As<IQueryable<Polugodiste>>().Setup(m => m.Expression).Returns(polugodista.Expression);
            mockSetPolugodiste.As<IQueryable<Polugodiste>>().Setup(m => m.ElementType).Returns(polugodista.ElementType);
            mockSetPolugodiste.As<IQueryable<Polugodiste>>().Setup(m => m.GetEnumerator()).Returns(()=>polugodista.GetEnumerator());

            foreach (var item in polugodista)
                mockSetPolugodiste.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Polugodista).Returns(mockSetPolugodiste.Object);


            var tromesecja = new List<Tromesecje>()
            {

            }.AsQueryable();

            var mockSetTromesecje = new Mock<DbSet<Tromesecje>>();
            mockSetTromesecje.As<IQueryable<Tromesecje>>().Setup(m => m.Provider).Returns(tromesecja.Provider);
            mockSetTromesecje.As<IQueryable<Tromesecje>>().Setup(m => m.Expression).Returns(tromesecja.Expression);
            mockSetTromesecje.As<IQueryable<Tromesecje>>().Setup(m => m.ElementType).Returns(tromesecja.ElementType);
            mockSetTromesecje.As<IQueryable<Tromesecje>>().Setup(m => m.GetEnumerator()).Returns(()=>tromesecja.GetEnumerator());

            foreach (var item in tromesecja)
                mockSetTromesecje.Setup(p => p.Add(item));
            mockContext.Setup(p => p.Tromesecja).Returns(mockSetTromesecje.Object);

            int sledecaGodina = Int32.Parse(DateTime.Now.Year.ToString()) + 1;

            UpisTromesecjaStringViewModel srpskiDatumi = new UpisTromesecjaStringViewModel()
            {
                PrvoPocetak = "01.09." + DateTime.Now.Year,
                PrvoKraj = "15.11." + DateTime.Now.Year,
                DrugoPocetak = "16.11." + DateTime.Now.Year,
                DrugoKraj = "15.01." + sledecaGodina,
                TrecePocetak = "16.01." + sledecaGodina,
                TreceKraj = "20.03." + sledecaGodina,
                CetvrtoPocetak = "21.03." + sledecaGodina,
                CetvrtoKraj = "25.06." + sledecaGodina,
                Poruka = null
            };

            // Act
            var services = new UpisSkolskeGodineController(mockContext.Object);
            services.Create(srpskiDatumi);

            // Assert

            mockSetSkolskaGodina.Verify(p => p.Add(It.IsAny<SkolskaGodina>()), Times.Once());
            mockContext.Verify(p => p.SaveChanges(), Times.Once());

            mockSetPolugodiste.Verify(p => p.Add(It.IsAny<Polugodiste>()), Times.Once());
            mockContext.Verify(p => p.SaveChanges(), Times.Once());

            mockSetTromesecje.Verify(p => p.Add(It.IsAny<Tromesecje>()), Times.Once());
            mockContext.Verify(p => p.SaveChanges(), Times.Once());
        }
        
    }
}