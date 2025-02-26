using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models;

namespace TheCoffeeHouse.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        // slider
        public ActionResult SliderPartial()
        {
            var sliders = db.sliders.ToList();
            return PartialView("SliderPartial", sliders);
        }

        // danh sách sản phẩm review ở trang chủ
        public ActionResult MenuHomePartial()
        {
            var items = db.products.OrderBy(x => Guid.NewGuid()).Take(6).ToList();
            var sale = db.sales.Single(s => s.Image == "sale.jpg").Image;
            ViewBag.sale = sale;
            return PartialView("MenuHomePartial", items);
        }

        public ActionResult cloudteaHomePartial()
        {
            var sales = db.sales.Where(s => s.Name == "_cloudteaHomePartial").ToList();
            return PartialView("cloudteaHomePartial", sales);
        }


        //danh sách cách bài post
        public ActionResult PostPartial()
        {
            var item = db.posts.ToList();
            if (item == null)
            {
                return HttpNotFound();
            }

            var category = db.postCategories.Where(p => p.Parent == 0).ToList();
            ViewBag.Category = category;

            return PartialView("PostPartial", item);
        }
    }
}