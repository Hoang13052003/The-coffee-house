using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models;
using TheCoffeeHouse.Models.EtityFarmwork;
using PagedList;
using Microsoft.Win32;
using Antlr.Runtime.Tree;

namespace TheCoffeeHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int ? page)
        {
            var item  =  db.orders.OrderByDescending(s=>s.Created_Date).ToList();
            if(page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            return View(item.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult View(int id)
        {
            var item = db.orders.Find(id);
            return View(item);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.orders.Find(id);
            if (item != null)
            {
                db.orders.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Update(int id, int trangthai)
        {
            var item = db.orders.Find(id);
            if (item != null)
            {
                db.orders.Attach(item);
                item.StatusID = trangthai;
                db.Entry(item).Property(s=>s.StatusID).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", success = true });
            }
            return Json(new { message = "Unsuccess", success = false });
        }
    }
}