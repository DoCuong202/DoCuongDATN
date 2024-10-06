using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class RatesController : Controller
    {
        // GET: Admin/Rates
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string searchText, int? starRateFilter, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Rate> items = db.Rates.OrderByDescending(x => x.Id);

            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.IdProduct.Contains(searchText) || x.ContentRate.Contains(searchText) || x.NameUser.Contains(searchText));
            }

            if (starRateFilter.HasValue && starRateFilter.Value > 0)
            {
                items = items.Where(x => x.StartRate == starRateFilter.Value);
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);

            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
            ViewBag.searchText = searchText; // Pass the search text back to the view
            ViewBag.starRateFilter = starRateFilter; // Pass the star rate filter back to the view

            return View(items);
        }


        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Rate model)
        {
            var userManager = new UserManager<BanHangOnline.Models.ApplicationUser>(new UserStore<BanHangOnline.Models.ApplicationUser>(new BanHangOnline.Models.ApplicationDbContext()));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                model.NgayTao = DateTime.Now;
                model.NameUser = currentUser.Fullname;
                db.Rates.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var item = db.Rates.Find(Id);
            if (item != null)
            {
                db.Rates.Remove(item);
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
                        var a = db.Rates.Find(Convert.ToInt32(item));
                        db.Rates.Remove(a);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Open(string Id)
        {
            var item = db.Products.FirstOrDefault(x => x.ProductCode == Id);
            if (item != null)
            {
                return Json(new { success = true, message = $@"/chi-tiet-san-pham/{item.ProductCategoryID}-{item.Id}" });
            }

            return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
        }

        [HttpPost]
        public ActionResult detail(int id)
        {
            var item = db.Rates.Find(id);
            if (item != null)
            {
                return Json(new { success = true, username = item.NameUser, product = item.IdProduct, time = item.NgayTao, sao = item.StartRate, nd = item.ContentRate });
            }
            return Json(new { success = false });
        }
    }
}