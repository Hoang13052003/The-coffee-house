using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using TheCoffeeHouse.Models;
using TheCoffeeHouse.Models.EtityFarmwork;

namespace TheCoffeeHouse.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingCartController()
        {
            _dbContext = new ApplicationDbContext();
        }

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
                var user = _dbContext.Users.Find(userId); // Tìm User theo ID nhanh hơn
                if (user != null)
                {
                    dynamic userInfo = new ExpandoObject();
                    userInfo.FullName = user.Fullname;
                    userInfo.Email = user.Email;
                    userInfo.Phone = user.Phone;

                    ViewBag.UserInfo = userInfo;
                }
            }

            return View();
        }

        public ActionResult OrderHistory()
        {

            string userId = User.Identity.GetUserId(); // Lấy ID người dùng đang đăng nhập
            var user = _dbContext.Users.Find(userId); // Tìm User theo ID nhanh hơn
            if (user != null)
            {
                var listOrder = _dbContext.orders.Where(x => x.Phone == user.Phone).ToList();

                ViewBag.OrderHistory = listOrder;
            }

            return View();
        }
        public ActionResult OrderHistoryDetail(int Id)
        {
            var order = _dbContext.orders.FirstOrDefault(x=> x.OrderID == Id);
            if (order != null)
            {
                ViewBag.order = order;
            }
            var listOrderDetail = _dbContext.orderDetails.Where(x=> x.OrderID == Id).ToList();
            
            if(listOrderDetail.Count > 0)
            {
                ViewBag.listOrder = listOrderDetail;        
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThanhToan(Order model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["SuccessMessage"] = "Vui lòng đăng nhập trước khi đặt hàng!";
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid) return RedirectToAction("Index");

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart == null || !cart.Items.Any()) return RedirectToAction("Index");

            // Lấy phương thức thanh toán
            int paymentMethod = int.TryParse(Request.Form["pay-by"], out int method) ? method : 1;

            // Tạo đơn hàng
            model.OrderDate = DateTime.Now;
            model.Created_Date = DateTime.Now;
            model.StatusID = 1;
            model.DeliveredID = 1;
            //model.PaymentMethod = paymentMethod;
            _dbContext.orders.Add(model);

            // Thêm chi tiết đơn hàng
            foreach (var item in cart.Items)
            {
                _dbContext.orderDetails.Add(new OrderDetail
                {
                    OrderID = model.OrderID,
                    ProductID = item.ProductID,
                    ProductName = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Description = item.Description
                });
            }

            _dbContext.SaveChanges(); // Lưu 1 lần duy nhất

            // Xử lý thanh toán
            return await HandlePayment(paymentMethod, model.OrderID, cart, model);
        }

        // Xử lý thanh toán dựa vào phương thức thanh toán
        private async Task<ActionResult> HandlePayment(int paymentMethod, int orderId, ShoppingCart cart, Order model)
        {
            ActionResult result;
            switch (paymentMethod)
            {
                case 2:
                    var momoService = new MomoService();
                    var payUrl = await momoService.CreatePaymentUrlAsync(orderId, model.TotalMoney); // Sử dụng service

                    if (!string.IsNullOrEmpty(payUrl))
                    {
                        result = Redirect(payUrl);
                    }
                    else
                    {
                        result = Content("Lỗi khi tạo yêu cầu thanh toán qua MoMo.");
                    }
                    break;
                //case 3:
                //    result = RedirectToAction("Payment", "ZaloPay", new { orderId });
                //    break;
                //case 4:
                //    result = RedirectToAction("Payment", "ShopeePay", new { orderId });
                //    break;
                //case 5:
                //    result = RedirectToAction("Payment", "Bank", new { orderId });
                //    break;
                default:
                    result = View(model); // Thanh toán tiền mặt => Trả về View xác nhận đơn hàng
                    break;
            }

            SendOrderEmail(cart, model);
            cart.ClearCart();

            return result;
        }

        // Gửi email xác nhận đơn hàng
        private void SendOrderEmail(ShoppingCart cart, Order model)
        {
            string strProduct = string.Join("", cart.Items.Select(sp =>
                $"<tr><td>{sp.Name}</td><td>{sp.Quantity}</td><td>{sp.Price}</td></tr>"
            ));

            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"))
                .Replace("{{MaDon}}", model.OrderID.ToString())
                .Replace("{{SanPham}}", strProduct)
                .Replace("{{ThanhTien}}", string.Format("{0:N0} đ", model.TotalMoney))
                .Replace("{{TenKhachHang}}", model.CustomerName)
                .Replace("{{DiaChiNhanHang}}", model.Address)
                .Replace("{{Phone}}", model.Phone)
                .Replace("{{Email}}", model.Email)
                .Replace("{{NgayDat}}", DateTime.Now.ToString());

            TheCoffeeHouse.Common.Common.SendMail("The Coffee House", "Đơn hàng #" + model.OrderID, contentCustomer, model.Email);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}