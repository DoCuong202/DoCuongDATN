using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Products
        public ActionResult Index(string searchText,int? page)

        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Alias.Contains(searchText) || x.Title.Contains(searchText) || x.ProductCode.Contains(searchText));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
            return View(items);
        }

        public ActionResult getProductInAdmin(string searchText, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Alias.Contains(searchText) || x.Title.Contains(searchText));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
            return PartialView("viewAddProductAdmin", items);
        }

        public ActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {
            if(ModelState.IsValid)
            {
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            model.Image = Images[i];
                            model.ProductImages.Add(new ProductImage
                            {
                                ProductID = model.Id,
                                Image = Images[i],
                                IsDefault = true
                            });
                        }
                        else
                        {
                            model.ProductImages.Add(new ProductImage
                            {
                                ProductID = model.Id,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.Products.Add(model);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        }

        public ActionResult getImageEdit(int id)
        {
            var items = db.ProductImages.Where(x=>x.ProductID == id).ToList();
            return PartialView(items);
        }

        public ActionResult Edit(int Id)
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            ViewBag.ProductImages = new SelectList(db.ProductImages.Where(x => x.ProductID == Id).ToList(), "Id", "Image");
            var a = ViewBag.ProductImages;
            var item = db.Products.Find(Id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model, List<string> Images, List<string> rDefault)
        {
            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count > 0)
                {
                    // Lấy danh sách các hình ảnh hiện tại của sản phẩm
                    var lstCurrent = db.ProductImages.Where(x => x.ProductID == model.Id).ToList();

                    // Xóa các hình ảnh cũ
                    foreach (var item in lstCurrent)
                    {
                        db.ProductImages.Remove(item);
                    }
                    db.SaveChanges();

                    // Thêm các hình ảnh mới
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (Images[i] == rDefault[0])
                        {
                            var newImage = new ProductImage
                            {
                                ProductID = model.Id,
                                Image = Images[i],
                                IsDefault = true
                            };
                            db.ProductImages.Add(newImage); // Sử dụng phương thức Add để thông báo cho EF rằng đây là các đối tượng mới

                        }
                        else
                        {
                            var newImage = new ProductImage
                            {
                                ProductID = model.Id,
                                Image = Images[i],
                                IsDefault = false
                            };
                            db.ProductImages.Add(newImage); // Sử dụng phương thức Add để thông báo cho EF rằng đây là các đối tượng mới
                        }

                    }
                }

                // Thiết lập ảnh đại diện (ảnh mặc định) cho sản phẩm
                if(rDefault != null && rDefault.Count > 0)
                {
                    model.Image = rDefault[0];
                }
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);

                // Đính kèm đối tượng sản phẩm và cập nhật trạng thái của nó
                db.Products.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var item = db.Products.Find(Id);
            if (item != null)
            {
                db.Products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult deleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var a = db.Products.Find(Convert.ToInt32(item));
                        db.Products.Remove(a);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int Id)
        {
            var item = db.Products.Find(Id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsHome(int Id)
        {
            var item = db.Products.Find(Id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });
            }

            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult IsSale(int Id)
        {
            var item = db.Products.Find(Id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsSale = item.IsSale });
            }

            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult IsFeature(int Id)
        {
            var item = db.Products.Find(Id);
            if (item != null)
            {
                item.IsFeature = !item.IsFeature;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsFeature = item.IsFeature });
            }

            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult IsHot(int Id)
        {
            var item = db.Products.Find(Id);
            if (item != null)
            {
                item.IsHot = !item.IsHot;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsHot = item.IsHot });
            }

            return Json(new { success = false });
        }

    }
}