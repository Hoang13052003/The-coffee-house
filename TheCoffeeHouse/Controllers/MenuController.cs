using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models;
using TheCoffeeHouse.Models.EtityFarmwork;

namespace TheCoffeeHouse.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Menu
        public ActionResult Index()
        {
            var item = db.products.ToList();
            if (item == null)
            {
                return HttpNotFound();
            }
            var menu = db.productCategories.Where(p => p.Parent == 0).ToList();
            ViewBag.Menu = menu;

            var menuProduct = db.productCategories.Where(p => p.Parent != 0).ToList();
            ViewBag.MenuProduct = menuProduct;
            return View(item);
        }

        public ActionResult ProductByCategory(int id)
        {
            if (id == 1)
            {
                var item = db.products.ToList();
                if (item == null)
                {
                    return HttpNotFound();
                }
                var menu = db.productCategories.Where(p => p.Parent == 0).ToList();
                ViewBag.Menu = menu;

                var menuProduct = db.productCategories.Where(p => p.Parent != 0).ToList();
                ViewBag.MenuProduct = menuProduct;
                return View(item);
            }
            else
            {
                var item = db.products.ToList();
                if (item == null)
                {
                    return HttpNotFound();
                }
                var menu = db.productCategories.Where(p => p.Parent == 0).ToList();
                ViewBag.Menu = menu;

                var menuProduct = db.productCategories.Where(p => p.Parent == id - 1).ToList();
                ViewBag.MenuProduct = menuProduct;
                return View(item);
            }
        }
        public ActionResult ProductDetail(int id, int cateid)
        {
            var item = db.products.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            var relatedProducts = db.products.Where(p => p.CateID == cateid).OrderBy(x => Guid.NewGuid()).Take(6).ToList();
            ViewBag.RelatedProducts = relatedProducts;
            var itemCategory = db.productCategories.Find(cateid);
            ViewBag.category = itemCategory.Name;
            return View(item);
        }
    }
}