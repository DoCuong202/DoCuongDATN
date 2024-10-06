using BanHangOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();   

        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuTop() {
            var items = db.Categories.OrderBy(x=>x.Position).ToList();  

            return PartialView("_MenuTop", items);
        }

        public ActionResult MenuProductCategory()
        {
            var items = db.ProductCategories.ToList();
            return PartialView("_MenuProductCategory", items);
        }

        public ActionResult MenuArrivals()
        {
            var items = db.ProductCategories.ToList();
            return PartialView("_MenuArrivals", items);
        }

        public ActionResult MenuCategoryInProducts(string title)
        {
            var items = db.ProductCategories.ToList();
            ViewBag.title = title;
            return PartialView("_MenuCategoryInProducts", items);
        }

        public ActionResult MenuSet()
        {
            var items = db.ProductCategories.Where(x=>x.isSet == true).ToList();
            return PartialView("_MenuSet", items);
        }
    }
}