using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models;
using TheCoffeeHouse.Models.EtityFarmwork;
using TheCoffeeHouse.Models.ViewModel;

namespace TheCoffeeHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SaleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Sale
        public ActionResult Index()
        {
            var item = db.sales.ToList();
            return View(item);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Sale model)
        {
            if (ModelState.IsValid)
            {
                db.sales.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.sales.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sale model)
        {
            if (ModelState.IsValid)
            {
                var existingItem = db.sales.Find(model.SaleID);
                if (existingItem == null)
                {
                    return HttpNotFound();
                }

                existingItem.Name = model.Name;
                existingItem.SeoTitle = model.SeoTitle;
                existingItem.Image = model.Image;
                existingItem.Description= model.Description;

                db.Entry(existingItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.sales.Find(id);
            if (item != null)
            {
                db.sales.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}