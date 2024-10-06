using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class OrderTableController : Controller
    {
        // GET: Admin/OrderTable
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<OrderTable> items = db.OrderTables.OrderByDescending(x => x.CreatedDate).ToList();
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Code.Contains(searchText) || x.Phone.Contains(searchText) || x.CustomerName.Contains(searchText));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
            return View(items);
        }


        public ActionResult Add(int? Id, int? type)
        {
            if (Id != null)
            {
                var item = db.OrderTables.FirstOrDefault(x => x.Id == Id);
                //ADĐ GIỎ HÀNG
                var checkProduct = db.OrderTableDetails.Where(x => x.OrderTableID == Id).ToList();
                if (checkProduct != null)
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
        public ActionResult Add(string code, string customername, string phone, string day, string countCustomer, string TimeStart, string TimeEnd, string note, string id)
        {
            DateTime parsedDay = DateTime.Parse(day);
            ShoppingCart cart = (ShoppingCart)Session["Cart"];


            if (cart != null)
            {
                OrderTable order;
                bool isNewOrder = string.IsNullOrEmpty(id);

                if (!isNewOrder)
                {
                    var ID = Convert.ToInt32(id);
                    order = db.OrderTables.FirstOrDefault(x => x.Id == ID);
                    var orders = db.OrderTableDetails.Where(x => x.OrderTableID == ID).ToList();
                    foreach (var item in orders)
                    {
                        db.OrderTableDetails.Remove(item);
                    }
                    db.SaveChanges();
                    if (order == null)
                    {
                        return Json(new { success = false, message = "Order not found" });
                    }
                }
                else
                {
                    order = new OrderTable();
                    db.OrderTables.Add(order);
                }

                order.Code = code;
                order.CustomerName = customername;
                order.Phone = phone;
                order.Day = parsedDay;
                order.CountCustomer = countCustomer;
                order.TimeStart = TimeStart;
                order.TimeEnd = TimeEnd;
                cart.Items.ForEach(x => order.OrderTableDetails.Add(new OrderTableDetail
                {
                    ProductID = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }));
                order.Quantity = cart.Items.Sum(x => x.Quantity);
                order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                order.TypePayment = "COD";
                order.statusPayment = 0;
                order.note = note;
                order.statusOrder = -1;
                order.ModifiedDate = DateTime.Now;

                if (isNewOrder)
                {
                    order.CreatedDate = DateTime.Now;
                    order.CreatedBy = "Employee";
                }

                db.SaveChanges();
                cart.ClearCart();
                return Json(new { success = true });
            }

            return Json(new { success = false });

        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
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
            var item = db.OrderTables.Find(Id);
            if (item != null)
            {
                var itemDetail = db.OrderTableDetails.Where(x => x.OrderTableID == Id).ToList();
                foreach (var a in itemDetail)
                {
                    db.OrderTableDetails.Remove(a);
                }
                db.OrderTables.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public ActionResult Detail(int id)
        {
            var item = db.OrderTables.Where(x => x.Id == id).FirstOrDefault();
            return View(item);
        }

        public ActionResult Detail2(string id)
        {
            var ID = Convert.ToInt32(id);
            var item = db.OrderTables.Where(x => x.Id == ID).FirstOrDefault();
            if (item != null)
            {
                var items = db.OrderTableDetails.Where(a => a.OrderTableID == ID).ToList();
                if (items.Count > 0)
                {
                    ViewBag.Check = 1;
                }
                else
                {
                    ViewBag.Check = 0;
                }
                return PartialView(item);
            }
            return View();
        }

        public ActionResult ProductInDetailAdmin(int id)
        {
            var item = db.OrderTableDetails.Where(x => x.OrderTableID == id).ToList();
            if (item != null)
            {
                return PartialView(item);

            }
            return View();
        }

        public ActionResult Edit(string Id)
        {
            var id = Convert.ToInt32(Id);
            var item = db.OrderTables.Find(id);
            return PartialView(item);
        }

        public ActionResult Update(string id, string statusOrder)
        {
            var ID = Convert.ToInt32(id);
            var status = Convert.ToInt32(statusOrder);
            var item = db.OrderTables.Find(ID);
            if (item != null)
            {
                db.OrderTables.Attach(item);
                item.statusOrder = status;
                db.Entry(item).Property(x => x.statusOrder).IsModified = true;
                db.SaveChanges();
                return Json(new { success = true, message = "Cập nhật thành công" });
            }

            return Json(new { success = false, message = "Lỗi truy vấn!" });
        }
    }
}