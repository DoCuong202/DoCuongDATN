using BanHangOnline.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{


    [Authorize(Roles = "Admin, Employee")]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Admin/Home
        public ActionResult Index()
        {
            var itemOrders = db.Orders.ToList();
            ViewBag.countOrder = itemOrders.Count;

            var itemOrdertables = db.OrderTables.ToList();
            ViewBag.countOrderTable = itemOrdertables.Count;
            return View();
        }

        public ActionResult AboutOrder()
        {
            return View();
        }
    }
}