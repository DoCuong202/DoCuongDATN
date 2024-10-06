using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BanHangOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Cửa hàng",
                url: "cua-hang",
                defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Chi tiet san pham",
                url: "chi-tiet-san-pham/{ProductCategoryID}-{Id}",
                defaults: new { controller = "Products", action = "Detail", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Chi tiet san pham single",
                url: "chi-tiet-san-pham/{Id}",
                defaults: new { controller = "Products", action = "Detail", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Liên hệ",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Thanh toán VNPAY",
                url: "VnpayReturn",
                defaults: new { controller = "ShoppingCart", action = "VnpayReturn", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Thanh toán VNPAY ĐẶT BÀN",
                url: "VnpayReturnDatBan",
                defaults: new { controller = "OrderTable", action = "VnpayReturn", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Chung toi",
                url: "chung-toi",
                defaults: new { controller = "About", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Giỏ hàng",
                url: "gio-hang",
                defaults: new { controller = "ShoppingCart", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Dat ban",
                url: "dat-ban",
                defaults: new { controller = "Ordertable", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Bài viết",
                url: "bai-viet",
                defaults: new { controller = "Blogs", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Chi tiết đơn hàng",
                url: "checkout-datship/order-detail/{code}",
                defaults: new { controller = "ShoppingCart", action = "ToOrderDetail", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Chi tiết đơn đặt bàn",
                url: "checkout-datban/order-detail/{code}",
                defaults: new { controller = "OrderTable", action = "ToOrderDetail", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Chi tiết đơn bat ban",
                url: "checkout-datban/order-detail/{code}",
                defaults: new { controller = "OrderTable", action = "ToOrderDetail", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Câu chuyện ẩm thực",
                url: "cauchuyenamthuc/{alias}-{Id}",
                defaults: new { controller = "Blogs", action = "Detail", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Dat ban vnpay",
                url: "checkout-datban",
                defaults: new { controller = "OrderTable", action = "CheckOut", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Danh mục sản phẩm",
                url: "danh-muc-san-pham/{alias}-{Id}",
                defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
            routes.MapRoute(
                name: "Chi tiet don dat ban",
                url: "dat-ban/order-table-detail/{Code}",
                defaults: new { controller = "OrderTable", action = "ToOrderDetail", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers" }
            );
        }
    }
}
