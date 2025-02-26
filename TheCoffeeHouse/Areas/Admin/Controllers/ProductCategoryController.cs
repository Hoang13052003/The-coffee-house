using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models;
using TheCoffeeHouse.Models.EtityFarmwork;

namespace TheCoffeeHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var item = db.productCategories.ToList();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.Created_Date = DateTime.Now;
                model.Updates_Date = DateTime.Now;
                db.productCategories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.productCategories.Find(id);
            if (item == null)
            {
                return HttpNotFound(); // or return a different result if the item is not found
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                var existingItem = db.productCategories.Find(model.CateID);
                if (existingItem == null)
                {
                    return HttpNotFound(); // or return a different result if the item is not found
                }

                // Update the properties of the existing item
                existingItem.Name = model.Name;
                existingItem.Link = model.Link;
                existingItem.SeoTitle = model.SeoTitle;
                existingItem.Parent = model.Parent;
                existingItem.Updates_Date = DateTime.Now;

                db.Entry(existingItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.productCategories.Find(id);
            if (item != null)
            {
                db.productCategories.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}