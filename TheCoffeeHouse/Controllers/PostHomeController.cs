using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models;
using TheCoffeeHouse.Models.EtityFarmwork;

namespace TheCoffeeHouse.Controllers
{
    public class PostHomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       //chưa làm trang post

        //public ActionResult Index()
        //{
        //    var item = db.posts.ToList();
        //    if (item == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(item);
        //}

        public ActionResult PostDetail(int id)
        {
            var item = db.posts.Find(id);
            var detail= db.postDetails.Where(p=>p.PostID == item.PostID).ToList();
            if(item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Detail = detail;
            return View(item);
        }
        
    }
}