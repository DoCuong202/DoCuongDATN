using BanHangOnline.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanHangOnline.Models.EF;
using PagedList;
using System.Web.UI;

namespace BanHangOnline.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Profile
        public ActionResult Index(int? page)
        {
            var userManager = new UserManager<BanHangOnline.Models.ApplicationUser>(new UserStore<BanHangOnline.Models.ApplicationUser>(new BanHangOnline.Models.ApplicationDbContext()));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            if (currentUser != null)
            {
                var items = db.Orders.Where(x => x.IdCustomer == currentUser.Id).ToList();
                ViewBag.CountOrder = items.Count;
            }

            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Order> item = db.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            if(currentUser != null)
            {
                item = item.Where(x => x.IdCustomer == currentUser.Id).ToList();
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            item = item.ToPagedList(pageIndex, pageSize);
            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
            return View(item);
        }

        public ActionResult update(string IdCustomer)
        {
            var userManager = new UserManager<BanHangOnline.Models.ApplicationUser>(new UserStore<BanHangOnline.Models.ApplicationUser>(new BanHangOnline.Models.ApplicationDbContext()));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            if (currentUser != null)
            {
                ViewBag.UserId = currentUser.Id;
                ViewBag.Address = currentUser.Address;
                ViewBag.Phone = currentUser.Phone;
                ViewBag.Email = currentUser.Email;
                ViewBag.FullName = currentUser.Fullname;
            }

            return PartialView();
        }

        [HttpPost]
        public ActionResult ToUpdate(string IdCustomer, string fullName, string Address, string Phone)
        {
            var userManager = new UserManager<BanHangOnline.Models.ApplicationUser>(new UserStore<BanHangOnline.Models.ApplicationUser>(new BanHangOnline.Models.ApplicationDbContext()));
            var currentUser = userManager.FindById(IdCustomer);
            if (currentUser != null)
            {
                currentUser.Fullname = fullName; 
                currentUser.Address = Address; 
                currentUser.Phone = Phone;
                userManager.Update(currentUser);
                return Json(new {success = true});
            }
            return Json(new { success = false });
        }
    }
}