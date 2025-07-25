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

            //HOME

            // Route tùy chỉnh cho Coffee
            routes.MapRoute(
                name: "Coffee",
                url: "coffee",
                defaults: new { controller = "AtHome", action = "Coffee" }
            );

            // Route tùy chỉnh cho Tea
            routes.MapRoute(
                name: "Tea",
                url: "tea",
                defaults: new { controller = "AtHome", action = "Tea" }
            );
           
            routes.MapRoute(
                name: "Lịch sử đơn hàng",
                url: "lich-su-don-hang",
                defaults: new { controller = "ShoppingCart", action = "OrderHistory" },
                namespaces: new[] { "TheCoffeeHouse.Controllers" }
            );

            routes.MapRoute(
               name: "Danh sách sản phẩm",
               url: "san-pham",
               defaults: new { controller = "Menu", action = "Index" },
               namespaces: new[] { "TheCoffeeHouse.Controllers" }
            );

            routes.MapRoute(
               name: "Danh sách sản phẩm theo danh mục",
               url: "san-pham/danh-muc/{id}",
               defaults: new { controller = "Menu", action = "ProductByCategory", id = UrlParameter.Optional },
               namespaces: new[] { "TheCoffeeHouse.Controllers" }
            );

            // Route cho chi tiết sản phẩm
            routes.MapRoute(
                name: "Chi tiết sản phẩm",
                url: "chi-tiet-san-pham/{cateid}/{id}",
                defaults: new { controller = "Menu", action = "ProductDetail", cateid = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Giỏ hàng",
               url: "gio-Hang",
               defaults: new { controller = "ShoppingCart", action = "Index" },
               namespaces: new[] { "TheCoffeeHouse.Controllers" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TheCoffeeHouse.Controllers" }
            );




        }
    }
}
