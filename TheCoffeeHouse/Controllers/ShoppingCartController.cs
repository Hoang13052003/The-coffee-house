using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models;
using TheCoffeeHouse.Models.EtityFarmwork;

namespace TheCoffeeHouse.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                ViewBag.CartItem = cart.Items;
            }

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId(); // Lấy ID người dùng đang đăng nhập
                using (var db = new ApplicationDbContext())
                {
                    var user = db.Users.Find(userId); // Tìm User theo ID nhanh hơn
                    if (user != null)
                    {
                        dynamic userInfo = new ExpandoObject();
                        userInfo.FullName = user.Fullname;
                        userInfo.Email = user.Email;
                        userInfo.Phone = user.Phone;

                        ViewBag.UserInfo = userInfo;
                    }
                }
            }

            return View();
        }

        public ActionResult OrderHistory()
        {

            string userId = User.Identity.GetUserId(); // Lấy ID người dùng đang đăng nhập
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(userId); // Tìm User theo ID nhanh hơn
                if (user != null)
                {
                    var listOrder = db.orders.Where(x => x.Phone == user.Phone).ToList();

                    ViewBag.OrderHistory = listOrder;
                }
            }

            return View();
        }
        public ActionResult OrderHistoryDetail(int Id)
        {
            var order = db.orders.FirstOrDefault(x=> x.OrderID == Id);
            if (order != null)
            {
                ViewBag.order = order;
            }
            var listOrderDetail = db.orderDetails.Where(x=> x.OrderID == Id).ToList();
            
            if(listOrderDetail.Count > 0)
            {
                ViewBag.listOrder = listOrderDetail;        
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThanhToan(Order model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    ShoppingCart cart = (ShoppingCart)Session["Cart"];
                    if (cart != null)
                    {
                        //cập nhật đơn hàng vào db
                        model.OrderDate = DateTime.Now;
                        model.Created_Date = DateTime.Now;
                        model.StatusID = 1;
                        model.DeliveredID = 1;
                        db.orders.Add(model);

                        //cập nhật chi tiết sản phẩm vào db
                        List<OrderDetail> lstOD = new List<OrderDetail>();
                        foreach (var item in cart.Items)
                        {
                            OrderDetail detail = new OrderDetail();
                            detail.OrderID = model.OrderID;
                            detail.ProductID = item.ProductID;
                            detail.ProductName = item.Name;
                            detail.Price = item.Price;
                            detail.Quantity = item.Quantity;
                            detail.Description = item.Description;
                            db.orderDetails.Add(detail);
                            lstOD.Add(detail);
                        }
                        db.SaveChanges();


                        // gửi mail cho khách hàng
                        var StrProduct = "";
                        foreach (var sp in cart.Items)
                        {
                            StrProduct += "<tr>";
                            StrProduct += "<td>" + sp.Name + "</td>";
                            StrProduct += "<td>" + sp.Quantity + "</td>";
                            StrProduct += "<td>" + sp.Price + "</td>";
                            StrProduct += "</tr>";
                        }

                        string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                        contentCustomer = contentCustomer.Replace("{{MaDon}}", model.OrderID.ToString());
                        contentCustomer = contentCustomer.Replace("{{SanPham}}", StrProduct);
                        contentCustomer = contentCustomer.Replace("{{ThanhTien}}", string.Format("{0:N0} đ", model.TotalMoney));
                        contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", model.CustomerName);
                        contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", model.Address);
                        contentCustomer = contentCustomer.Replace("{{Phone}}", model.Phone);
                        contentCustomer = contentCustomer.Replace("{{Email}}", model.Email);
                        contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString());
                        TheCoffeeHouse.Common.Common.SendMail("The Coffee House", "Đơn hàng #" + model.OrderID.ToString(), contentCustomer.ToString(), model.Email);

                        cart.ClearCart();
                        return View(model);
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                TempData["SuccessMessage"] = "Vui lòng đăng nhập trước khi đặt hàng! vui lòng đăng nhập!";
                return RedirectToAction("Login", "Account");
            }
        }



        [HttpGet]
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity, string size, string toppings, decimal extraPrice)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.products.FirstOrDefault(s => s.ProductID == id);

            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];

                if (cart == null)
                {
                    cart = new ShoppingCart();
                    Session["Cart"] = cart; // Lưu giỏ hàng vào Session
                }

                // Lưu thông tin size và topping vào ghi chú
                string note = $"Size: {size}";
                if (!string.IsNullOrEmpty(toppings))
                {
                    note += $", Topping: {toppings}";
                }

                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductID = checkProduct.ProductID,
                    Name = checkProduct.Name,
                    Image = checkProduct.Image,
                    CateName = checkProduct.ProductCategory.Name,
                    Quantity = quantity,
                    Description = note
                };

                if (!string.IsNullOrEmpty(checkProduct.Image))
                {
                    item.Image = checkProduct.Image;
                }

                item.Price = checkProduct.Price;
                item.Total = item.Quantity * item.Price + extraPrice;

                cart.AddToCart(item, quantity);
                code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công", code = 1, Count = cart.Items.Count };
            }

            return Json(code);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {

            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];

            if (cart != null)
            {
                var checkProduct =  cart.Items.FirstOrDefault(x => x.ProductID == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {

            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];

            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true});

            }
            return Json(new { Success = false });
        }
        //[HttpGet]
        //public ActionResult _ProductDetailPartial(int id)
        //{
        //    ShoppingCart cart = (ShoppingCart)Session["Cart"];
        //    var checkProduct = cart.Items.FirstOrDefault(x => x.ProductID == id);
        //    return PartialView("_ProductDetailPartial", checkProduct);
        //}
        //[HttpPost]
        //public ActionResult Update(int id, int quantity)
        //{
        //    var code = new { Success = false, msg = "", code = -1, Count = 0 };
        //    ShoppingCart cart = (ShoppingCart)Session["Cart"];

        //    if (cart != null)
        //    {
        //        var checkProduct = cart.Items.FirstOrDefault(x => x.ProductID == id);
        //        if (checkProduct != null)
        //        {
        //            cart.UpdateQuantity(id, quantity);
        //            code = new { Success = true, msg = "Đã thay đổi số lượng sản phẩm thành công", code = 1, Count = cart.Items.Count };
        //        }
        //    }
        //    return Json(code);
        //}
    }
}