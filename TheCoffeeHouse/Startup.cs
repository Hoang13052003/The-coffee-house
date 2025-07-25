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
        }
    }
}
