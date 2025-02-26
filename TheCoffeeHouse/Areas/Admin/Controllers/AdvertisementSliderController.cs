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
    public class AdvertisementSliderController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/AdvertisementSlider
        public ActionResult Index()
        {
            var item = db.sliders.ToList();
            //if (item == null)
            //{
            //    return HttpNotFound();
            //}
            return View(item);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Slider model)
        {
            if (ModelState.IsValid)
            {
                db.sliders.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.sliders.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }        
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slider model)
        {
            if (ModelState.IsValid)
            {
                var existingItem = db.sliders.Find(model.ID);
                if (existingItem == null)
                {
                    return HttpNotFound();
                }

                existingItem.Name = model.Name;
                existingItem.Image = model.Image;
                //existingItem.ListImage = model.ListImage;
                existingItem.Link = model.Link;

                db.Entry(existingItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.sliders.Find(id);
            if (item != null)
            {
                db.sliders.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
