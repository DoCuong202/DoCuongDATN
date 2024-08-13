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
        public ActionResult Index(string searchText, int? star, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Rate> items = db.Rates.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.IdProduct.Contains(searchText) || x.NameUser.Contains(searchText));
            }
            if (star > 0)
            {
                items = items.Where(x=>x.StartRate ==  star).ToList();
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
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
    }
}