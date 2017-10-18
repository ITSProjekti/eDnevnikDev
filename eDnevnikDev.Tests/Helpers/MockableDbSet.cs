using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;

namespace eDnevnikDev.Tests.Helpers
{
    public abstract class MockableDbSet<T>:DbSet<T> where T :class
    {
        public abstract void AddOrUpdate(params T[] entities);
        public abstract void AddOrUpdate(Expression<Func<T, object>> identifierExpression, params T[] entities);
    }
    public abstract class MockableIdentity<T> : DbSet<T> where T : class
    {
        public abstract void AddToRole(string T, string U);
       
    }
}
