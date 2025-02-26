using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models;

namespace TheCoffeeHouse.Controllers
{
    public class AtHomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: AtHome
        public ActionResult Coffee()
        {
            var item = db.products.Where(p=>p.CateID==27).ToList();
            var productCategory = db.productCategories.Find(27);

            if (productCategory != null)
            {
                ViewBag.CoffeePartial = productCategory.Name;
            }
            return View(item);
        }
        public ActionResult Tea()
        {
            var item = db.products.Where(p => p.CateID == 28).ToList();
            var productCategory = db.productCategories.Find(28);

            if (productCategory != null)
            {
                ViewBag.CoffeePartial = productCategory.Name;
            }
            return View(item);
        }

    }
}