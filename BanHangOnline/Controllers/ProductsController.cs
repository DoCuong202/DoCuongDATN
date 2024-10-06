using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;

namespace BanHangOnline.Controllers
{
   
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();   
        // GET: Products
        public ActionResult Index(string searchText, int? page, int? Id)
        {
            var pageSize = 10;
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            if (page == null)
            {
                page = 1;
            }
            if (Id != null)
            {
                items = items.Where(x => x.ProductCategoryID == Id).ToList();
                var title = db.ProductCategories.Where(x => x.Id == Id).FirstOrDefault().Title;
                ViewBag.active = title;
            }
            else if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Alias.Contains(searchText) || x.Title.Contains(searchText));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.size = pageSize;
            ViewBag.page = pageIndex;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x=>x.IsHome && x.IsActive).Take(20).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ItemsSale()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(10).ToList();
            return PartialView(items);
        }

        public ActionResult SetOrderInHome()
        {
            var items = db.Products.Where(x => x.IsSet && x.IsActive).ToList();
            return PartialView("_SetOrderInHome", items);
        }

        public ActionResult Detail(int? ProductCategoryID, int Id)
        {
            var item = db.Products.Find(Id);

            if(item != null)
            {
                int tongsao = 0;
                var rates = db.Rates.Where(x=>x.IdProduct == item.ProductCode).ToList();
                foreach(var a in rates)
                {
                    tongsao += a.StartRate;
                }
                int tbSao = 0;
                if(rates.Count > 0)
                {
                    tbSao = tongsao / rates.Count;

                }
                ViewBag.tbSao = tbSao;
            }
            if (ProductCategoryID > 0)
            {
                var ProductCategoryTitle = db.ProductCategories.Find(ProductCategoryID).Title;
                ViewBag.productCategoryTitle = ProductCategoryTitle;
                ViewBag.productCategoryId = ProductCategoryID;
            }
            else
            {
                ViewBag.productcategorytitle = null;
                ViewBag.productcategoryid = null;
            }

            return View(item);


        }

        public ActionResult ProductLQ(int idCategory, int idProduct)
        {
            var items = db.Products.Where(x => x.ProductCategory.Id == idCategory).Take(4).ToList();
            ViewBag.IdCurrent = idProduct;
            return PartialView("ProductLQ",items);
        }

        public ActionResult Rates(string Id, int? star)
        {
            var items = db.Rates.Where(x=>x.IdProduct == Id).OrderByDescending(x=>x.NgayTao).ToList();

            if (star != null)
            {
                if(star == 0)
                {
                    items = db.Rates.Where(x => x.IdProduct == Id).OrderByDescending(x => x.NgayTao).ToList();
                }
                if(star > 0)
                {
                    for(int i = 1; i <= 5; i++)
                    {
                        if(i == star)
                        {
                            items = db.Rates.Where(x => x.IdProduct == Id && x.StartRate == star).OrderByDescending(x => x.NgayTao).ToList();
                        }
                    }
                }
            }

            ViewBag.Count = items.Count;
            ViewBag.Id = Id;
            return PartialView("_RatesPartialView", items);
        }

        public ActionResult AddRate(string Id)
        {
            ViewBag.IdProduct = Id;
            return PartialView("AddRate");
        }

        [HttpPost]
        public ActionResult SubmitAddRate(string IdProduct, string NameUser, string content, int startRate)
        {
            if (!string.IsNullOrEmpty(IdProduct))
            {
                var rate = new Rate();
                rate.CreatedDate = DateTime.Now;
                rate.ModifiedDate = DateTime.Now;
                rate.IdProduct = IdProduct;
                rate.NameUser = NameUser;
                rate.ContentRate = content;
                rate.StartRate = startRate;
                rate.NgayTao = DateTime.Now;
                db.Rates.Add(rate);
                db.SaveChanges();
                return Json(new {success = true});
            }
            return Json(new { success = false });

        }
    }
}