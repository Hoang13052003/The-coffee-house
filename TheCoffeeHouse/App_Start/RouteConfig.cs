using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TheCoffeeHouse
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            routes.MapRoute(
                name: "GioHang",
                url: "GioHang",
                defaults: new { controller = "ShoppingCart", action = "Index" },
                namespaces: new[] { "TheCoffeeHouse.Controllers" }
            );
            routes.MapRoute(
                name: "LichSu",
                url: "LichSuDonHang",
                defaults: new { controller = "ShoppingCart", action = "OrderHistory" },
                namespaces: new[] { "TheCoffeeHouse.Controllers" }
            );

            routes.MapRoute(
               name: "ListProduct",
               url: "San-pham",
               defaults: new { controller = "Menu", action = "Index" },
               namespaces: new[] { "TheCoffeeHouse.Controllers" }
           );

            //routes.MapRoute(
            //    name: "ProductByCategory",
            //    url: "San-pham/{namecategory}",
            //    defaults: new { controller = "Menu", action = "ProductByCategory", namecategory = UrlParameter.Optional },
            //    namespaces: new[] { "TheCoffeeHouse.Controllers" }
            //);

            //routes.MapRoute(
            //    name: "ProductDetail",
            //    url: "San-pham/{nameproduct}",
            //    defaults: new { controller = "Menu", action = "ProductDetail", nameproduct = UrlParameter.Optional },
            //    namespaces: new[] { "TheCoffeeHouse.Controllers" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TheCoffeeHouse.Controllers" }
            );




        }
    }
}
