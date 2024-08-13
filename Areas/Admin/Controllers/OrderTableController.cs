﻿using BanHangOnline.Models.EF;
using BanHangOnline.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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