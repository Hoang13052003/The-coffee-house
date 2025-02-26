using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using System;
using TheCoffeeHouse.Models;

[assembly: OwinStartupAttribute(typeof(TheCoffeeHouse.Startup))]
namespace TheCoffeeHouse
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            this.CreateRolesAndUser();
        }
        public void CreateRolesAndUser()
        {
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            //var ApplicationDbContext = new ApplicationDbContext();
            //var appUserStore = new ApplicationUser(ApplicationDbContext);
            //var userManager = new ApplicationDbContext(appUserStore);
            //if (!roleManager.RoleExists("Admin"))
            //{
            //    var role = new IdentityRole();
            //    role.Name = "Admin";
            //    roleManager.Create(role);
            //}
            //if (userManager.FindByName("Admin") == null)
            //{
            //    var user = new AppUser();
            //    user.UserName = "Admin";
            //    user.Email = "admin@gmail.com";
            //    string userPassword = "admin123";

            //    var checkUser = userManager.Create(user, userPassword);
            //    if (checkUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "Admin");
            //    }
            //}
            ////manager
            //if (!roleManager.RoleExists("Manager"))
            //{
            //    var role = new IdentityRole();
            //    role.Name = "Manager";
            //    roleManager.Create(role);
            //}
            //if (userManager.FindByName("Manager") == null)
            //{
            //    var user = new AppUser();
            //    user.UserName = "Manager";
            //    user.Email = "nanager@gmail.com";
            //    string userPassword = "manager123";

            //    var checkUser = userManager.Create(user, userPassword);
            //    if (checkUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "Manager");
            //    }

            //}
            //if (!roleManager.RoleExists("Customer"))
            //{
            //    var role = new IdentityRole();
            //    role.Name = "Customer";
            //    roleManager.Create(role);
            //}
        }
    }
}
