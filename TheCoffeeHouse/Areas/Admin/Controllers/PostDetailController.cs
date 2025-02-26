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
    public class PostDetailController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/PostDetail
        public ActionResult Index()
        {
            var item = db.postDetails.ToList();
            if(item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        public ActionResult Add()
        {
            var post = db.posts.ToList();

            var viewModel = new PostViewModel
            {
                post = post.Select(c => new SelectListItem
                {
                    Value = c.PostID.ToString(),
                    Text = c.Name
                }).ToList()
            };

            ViewBag.PostName = viewModel;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PostDetail model)
        {
            if (ModelState.IsValid)
            {
                db.postDetails.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.postDetails.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            var post = db.posts.ToList();

            var viewModel = new PostViewModel
            {
                post = post.Select(c => new SelectListItem
                {
                    Value = c.PostID.ToString(),
                    Text = c.Name
                }).ToList()
            };

            ViewBag.PostName = viewModel;

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PostDetail model)
        {
            if (ModelState.IsValid)
            {
                var existingItem = db.postDetails.Find(model.PostDetailID);
                if (existingItem == null)
                {
                    return HttpNotFound();
                }

                existingItem.PostID = model.PostID;
                existingItem.Name = model.Name;
                existingItem.SeoTitle = model.SeoTitle;
                existingItem.Image = model.Image;
                existingItem.Description = model.Description;

                db.Entry(existingItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.postDetails.Find(id);
            if (item != null)
            {
                db.postDetails.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }   
    }
}