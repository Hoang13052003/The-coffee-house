using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models;
using TheCoffeeHouse.Models.EtityFarmwork;
using TheCoffeeHouse.Models.ViewModel;

namespace TheCoffeeHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Product
        public ActionResult Index()
        {
            var item = db.products.ToList();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
       
        public ActionResult Add()
        {
            var categories = db.productCategories.Where(pc=>pc.Parent != 0).ToList();

            var viewModel = new ProductViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CateID.ToString(),
                    Text = c.Name
                }).ToList()
            };

            ViewBag.ProductCategoyName = viewModel;

            return View();
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model)
        {
            if (ModelState.IsValid)
            {
                model.Created_Date = DateTime.Now;
                model.Updates_Date = DateTime.Now;
                db.products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.products.Find(id);

            var categories = db.productCategories.ToList();

            var viewModel = new ProductViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CateID.ToString(),
                    Text = c.Name
                }).ToList()
            };

            ViewBag.ProductCategoyName = viewModel;

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            var categories = db.productCategories.ToList();

            var viewModel = new ProductViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CateID.ToString(),
                    Text = c.Name
                }).ToList()
            };

            ViewBag.ProductCategoyName = viewModel;

            if (ModelState.IsValid)
            {
                var existingItem = db.products.Find(model.ProductID);
                if (existingItem == null)
                {
                    return HttpNotFound();
                }

                existingItem.Name = model.Name;
                existingItem.Image = model.Image;
                //existingItem.ListImage = model.ListImage;
                existingItem.Price = model.Price;
                existingItem.Description = model.Description;
                existingItem.CateID = model.CateID;
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
            var item = db.products.Find(id);
            if (item != null)
            {
                db.products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult Search(string searchString)
        {
            var products = db.products.Where(p => p.Name.Contains(searchString)).ToList();
            return View(products);
        }
    }
}