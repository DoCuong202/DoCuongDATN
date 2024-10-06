using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace BanHangOnline.Controllers
{
    public class BlogsController : Controller
    {
        // GET: Blogs
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Posts> items = db.Posts.Where(x=>x.IsActive == true).OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Alias.Contains(searchText) || x.Title.Contains(searchText));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
            return View(items);
        }

        public ActionResult GetData()
        {
            var items = db.Posts.Where(x => x.IsActive == true).OrderBy(x => x.CreatedDate).Take(3).ToList();
            
            return PartialView(items);
        }

        public ActionResult BaiVietMoiNhat()
        {
            var items = db.Posts.Where(x => x.IsActive == true).OrderBy(x => x.CreatedDate).Take(3).ToList();

            return PartialView(items);
        }
        public ActionResult Detail(string alias, int Id)
        {
            var item = db.Posts.Where(x=>x.Id == Id && x.IsActive == true).FirstOrDefault();
            return View(item);
        }
    }
}