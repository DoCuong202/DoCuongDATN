using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Order> items = db.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Code.Contains(searchText) || x.Phone.Contains(searchText) || x.Address.Contains(searchText));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
            return View(items);
        }

        public ActionResult Add(int? Id, int? type)
        {
            if(Id != null)
            {
                var item = db.Orders.FirstOrDefault(x=>x.Id == Id);
                //ADĐ GIỎ HÀNG
                var checkProduct = db.OrdersDetails.Where(x => x.OrderID == Id).ToList();
                if (checkProduct != null)
                {
                    ShoppingCart cart = (ShoppingCart)Session["Cart"];

                    if(type == 1)
                    {
                        if (cart == null)
                        {
                            cart = new ShoppingCart();
                        }
                    }
                    else
                    {
                        if (cart == null)
                        {
                            cart = new ShoppingCart();
                        }
                            cart.ClearCart();

                        foreach (var sp in checkProduct)
                        {
                            var product = db.Products.FirstOrDefault(x => x.Id == sp.ProductID);
                            ShoppingCartItem a = new ShoppingCartItem
                            {
                                ProductId = product.Id,
                                ProductName = product.Title,
                                Alias = product.Alias,
                                Categoryname = product.ProductCategory.Title,
                                ProductCategoryID = Convert.ToString(product.ProductCategoryID),
                                Quantity = sp.Quantity,
                            };
                            if (product.ProductImages.FirstOrDefault(x => x.IsDefault) != null)
                            {
                                a.ProductImage = product.Image;
                            }
                            if (product.PriceSale > 0 && product.IsSale == true)
                            {
                                a.Price = (decimal)product.PriceSale;
                            }
                            else
                            {
                                a.Price = product.Price;
                            }
                            a.TotalPrice = a.Quantity * a.Price;
                            cart.AddToCart(a, sp.Quantity);

                        }
                    }
                    Session["Cart"] = cart;
                }
                return View(item);
            }
            else
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];

                if (type == 1)
                {
                    if (cart == null)
                    {
                        cart = new ShoppingCart();
                    }
                }
                else
                {
                    if (cart == null)
                    {
                        cart = new ShoppingCart();
                    }
                    cart.ClearCart();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add(string customername, string phone, string address, string note, string id)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                Order order;
                bool isNewOrder = string.IsNullOrEmpty(id);

                if (!isNewOrder)
                {
                    var ID = Convert.ToInt32(id);
                    order = db.Orders.FirstOrDefault(x => x.Id == ID);
                    var orders = db.OrdersDetails.Where(x => x.OrderID == ID).ToList();
                    foreach(var item in orders)
                    {
                        db.OrdersDetails.Remove(item);
                    }
                    db.SaveChanges();
                    if (order == null)
                    {
                        return Json(new { success = false, message = "Order not found" });
                    }
                }
                else
                {
                    order = new Order();
                    db.Orders.Add(order);
                }

                order.CustomerName = customername;
                order.Phone = phone;
                order.Address = address;
                
                cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                {
                    ProductID = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }));
                order.Quantity = cart.Items.Sum(x => x.Quantity);
                order.TotalAmount = cart.Items.Sum(x => x.Price * x.Quantity);
                order.TypePayment = "COD";
                order.statusPayment = 0;
                order.note = note;
                order.statusOrder = -1;
                order.ModifiedDate = DateTime.Now;

                if (isNewOrder)
                {
                    order.CreatedDate = DateTime.Now;
                    order.CreatedBy = "Employee";
                    order.IdCustomer = "Anonymous";
                    order.Code = "HD" + new Random().Next(100000, 999999); // Generate 6 digit random code
                }

                db.SaveChanges();
                cart.ClearCart();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public ActionResult EditItem(int Id)
        {
            var item = db.Orders.Find(Id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(Order model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                //model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.Orders.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity, int type)
        {
            var code = new { Success = false, msg = "", code = -1, count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    Alias = checkProduct.Alias,
                    Categoryname = checkProduct.ProductCategory.Title,
                    ProductCategoryID = Convert.ToString(checkProduct.ProductCategoryID),
                    Quantity = quantity,
                };
                if (checkProduct.ProductImages.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImage = checkProduct.ProductImages.FirstOrDefault().Image;
                }
                if (checkProduct.PriceSale > 0 && checkProduct.IsSale == true)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                else
                {
                    item.Price = checkProduct.Price;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Them thanh cong", code = 1, count = cart.Items.Count };
            }
            return Json(code);
        }

        public ActionResult CartInOrder()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }


        [HttpPost]
        public ActionResult DeleteItem(int Id)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == Id);
            if (checkProduct != null)
            {
                cart.Remove(Id);
                return Json(new { success = true, count = cart.Items.Count });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var item = db.Orders.Find(Id);
            if (item != null)
            {
                db.Orders.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public ActionResult Detail(int id)
        {
            var item = db.Orders.Where(x => x.Id == id).FirstOrDefault();
            return View(item);
        }

        public ActionResult Detail2(string id)
        {
            var ID = Convert.ToInt32(id);
            var item = db.Orders.Where(x => x.Id == ID).FirstOrDefault();
            if (item != null)
            {
                return PartialView(item);
            }
            return View();
        }

        public ActionResult ProductInDetailAdmin(int id)
        {
            var item = db.OrdersDetails.Where(x => x.OrderID == id).ToList();
            return PartialView(item);
        }

        public ActionResult Edit(string Id)
        {
            var id = Convert.ToInt32(Id);
            var item = db.Orders.Find(id);
            return PartialView(item);
        }

        public ActionResult Update(string id, string statusOrder)
        {
            var ID = Convert.ToInt32(id);
            var status = Convert.ToInt32(statusOrder);
            var item = db.Orders.Find(ID);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.statusOrder = status;
                db.Entry(item).Property(x => x.statusOrder).IsModified = true;
                db.SaveChanges();
                return Json(new { success = true, message = "Cập nhật thành công" });
            }

            return Json(new { success = false, message = "Lỗi truy vấn!" });
        }
    }
}