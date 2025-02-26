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
    public class PostController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Post
        public ActionResult Index()
        {
            var item = db.posts.ToList();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        public ActionResult Add()
        {
            var categories = db.postCategories.ToList();

            var viewModel = new PostCategoryViewModel
            {
                postCategory = categories.Select(c => new SelectListItem
                {
                    Value = c.CateID.ToString(),
                    Text = c.Name
                }).ToList()
            };

            ViewBag.PostCategoyName = viewModel;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Post model)
        {
            if (ModelState.IsValid)
            {
                db.posts.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.posts.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            var categories = db.postCategories.ToList();

            var viewModel = new PostCategoryViewModel
            {
                postCategory = categories.Select(c => new SelectListItem
                {
                    Value = c.CateID.ToString(),
                    Text = c.Name
                }).ToList()
            };

            ViewBag.PostCategoyName = viewModel;

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post model)
        {
            if (ModelState.IsValid)
            {
                var existingItem = db.posts.Find(model.PostID);
                if (existingItem == null)
                {
                    return HttpNotFound();
                }

                existingItem.Name = model.Name;
                existingItem.SeoTitle = model.SeoTitle;
                existingItem.Image = model.Image;
                existingItem.Description = model.Description;
                existingItem.Created_Date = DateTime.Today;
                existingItem.Updates_Date = DateTime.Today;
                db.Entry(existingItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.posts.Find(id);
            if (item != null)
            {
                db.posts.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}