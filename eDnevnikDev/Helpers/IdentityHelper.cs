using eDnevnikDev.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eDnevnikDev.Helpers
{
    public class IdentityHelper
    {

        internal static void SeedIdentities(DbContext context)
        {
            string userNameAdmin = "rootadmin";
            string passwordAdmin = "P@ssw0rd";
            string emailAdmin = "rootadmin@admin.admin";

            string userNameAdmin2 = "rootadmin2";
            string passwordAdmin2 = "P@ssw0rd";
            string emailAdmin2 = "rootadmin2@admin.admin";


            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(RoleNames.ROLE_ADMINISTRATOR))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNames.ROLE_ADMINISTRATOR));
            }
            if (!roleManager.RoleExists(RoleNames.ROLE_PROFESOR))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNames.ROLE_PROFESOR));
            }
            if (!roleManager.RoleExists(RoleNames.ROLE_EDITOR))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNames.ROLE_EDITOR));
            }
            if (!roleManager.RoleExists(RoleNames.ROLE_UCENIK))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNames.ROLE_UCENIK));
            }

            ApplicationUser user = userManager.FindByName(userNameAdmin);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userNameAdmin,
                    Email = emailAdmin
                };
                IdentityResult userResult = userManager.Create(user, passwordAdmin);
                if (userResult.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, RoleNames.ROLE_ADMINISTRATOR);
                }
            }

            user = userManager.FindByName(userNameAdmin2);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userNameAdmin2,
                    Email=emailAdmin2
                };
                IdentityResult userResult = userManager.Create(user, passwordAdmin2);
                if (userResult.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, RoleNames.ROLE_ADMINISTRATOR);
                }
            }


        }
        
    }
}