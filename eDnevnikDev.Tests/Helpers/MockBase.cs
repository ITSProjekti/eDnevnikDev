using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using eDnevnikDev.ViewModel;
using eDnevnikDev.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using eDnevnikDev.Tests.helpers;
using eDnevnikDev.Models;
using System.Data.Entity;
using System.Reflection;

namespace eDnevnikDev.Tests.Helpers
{
    class MockBase
    {
        public MockBase()
        {
            
            var mockContext = new Mock<ApplicationDbContext>();


            string UcenikQualifiedName = typeof(Ucenik).AssemblyQualifiedName;
            Type elementType = Type.GetType(UcenikQualifiedName);
            Type dbSetType = typeof(DbSet<>).MakeGenericType(new[] { elementType });

            Type listType = typeof(Mock<>).MakeGenericType(new[] { dbSetType });
            Mock mock = (Mock)Activator.CreateInstance(listType);

        }

        //public Mock<DbSet<T>> () where T:class
        //{

        //    return mockSet;
        //}

    }
}
