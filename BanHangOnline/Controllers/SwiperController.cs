using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Controllers
{
    public class SwiperController : Controller
    {
        // GET: Swiper
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewSwiperMain1()
        {
            return PartialView("Slider_main_1");
        }

        public ActionResult ViewSwiperMain2()
        {
            return PartialView("Slider_main_2");
        }

        public ActionResult ViewSwiperMain3()
        {
            return PartialView("Slider_main_3");
        }
        public ActionResult ViewSwiperMain4()
        {
            return PartialView("Slider_main_4");
        }
        public ActionResult ViewSwiperMain5()
        {
            return PartialView("Slider_main_5");
        }
    }
}