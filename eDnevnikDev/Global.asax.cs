using eDnevnikDev.Helpers;
using eDnevnikDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace eDnevnikDev
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ApplicationDbContext context = new ApplicationDbContext();
            IdentityHelper.SeedIdentities(context);
            SeedHelper.Seed(context);
            SeedHelper.SeedTipOcene(context);
            SeedHelper.SeedTipPredmeta(context);
            JobScheduler.Start();
        }
    }
}
