using BanHangOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Controllers
{
    public class ProductImageInDetailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductImage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListImageInDetail(int Id)
        {
            var items = db.ProductImages.Where(x => x.ProductID == Id).ToList();
            return PartialView(items);
        }
    }
}