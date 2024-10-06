using BanHangOnline.Areas.Admin.Data;
using BanHangOnline.Models;
using BanHangOnline.Models.EF;
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

            var itemProduct = db.Products.ToList();
            ViewBag.countProduct = itemProduct.Count;

            var itemPost = db.Posts.ToList();
            ViewBag.countPost = itemPost.Count;

            return View();
        }

        public ActionResult datgiao()
        {
            var recentOrders = db.Orders.OrderByDescending(order => order.CreatedDate).Take(10).ToList();
            var dangchuanbi = db.Orders.Count(x => x.statusOrder == -1);
            var danggiao = db.Orders.Count(x => x.statusOrder == 0);
            var giaothanhcong = db.Orders.Count(x => x.statusOrder == 1);
            var giaothatbai = db.Orders.Count(x => x.statusOrder == 2);

            ViewBag.dangchuanbi = dangchuanbi;
            ViewBag.danggiao = danggiao;
            ViewBag.giaothanhcong = giaothanhcong;
            ViewBag.giaothatbai = giaothatbai;


            var dangchuanbiban = db.OrderTables.Count(x => x.statusOrder == -1);
            ViewBag.dangchuanbiban = dangchuanbiban;

            return PartialView("datgiao", recentOrders);
        }

        public ActionResult GetOrdersByStatus(int status, int pageSize, int pageNumber)
        {
            var ordersQuery = db.Orders.Where(o => o.statusOrder == status).OrderByDescending(order => order.CreatedDate);

            if (pageSize > 0)
            {
                ordersQuery = (IOrderedQueryable<Models.EF.Order>)ordersQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            var orders = ordersQuery.ToList();
            return PartialView("_OrderTablePartial", orders);
        }

        public ActionResult GetRecentOrders(int pageSize, int pageNumber)
        {
            var ordersQuery = db.Orders.OrderByDescending(order => order.CreatedDate);

            if (pageSize > 0)
            {
                ordersQuery = (IOrderedQueryable<Models.EF.Order>)ordersQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            var orders = ordersQuery.ToList();
            return PartialView("_OrderTablePartial", orders);
        }


        //-----------------------------------------------------------------------ĐẶT BÀN
        public ActionResult datban()
        {
            var recentOrders = db.OrderTables.OrderByDescending(order => order.CreatedDate).Take(10).ToList();
            var dangchuanbi = db.OrderTables.Count(x => x.statusOrder == -1);
            var danggiao = db.OrderTables.Count(x => x.statusOrder == 0);
            var giaothanhcong = db.OrderTables.Count(x => x.statusOrder == 1);
            var giaothatbai = db.OrderTables.Count(x => x.statusOrder == 2);

            ViewBag.dangchuanbi = dangchuanbi;
            ViewBag.danggiao = danggiao;
            ViewBag.giaothanhcong = giaothanhcong;
            ViewBag.giaothatbai = giaothatbai;

            return PartialView("datban", recentOrders);
        }

        public ActionResult GetOrdersByStatusDatban(int status, int pageSize, int pageNumber)
        {
            var ordersQuery = db.OrderTables.Where(o => o.statusOrder == status).OrderByDescending(order => order.CreatedDate);

            if (pageSize > 0)
            {
                ordersQuery = (IOrderedQueryable<Models.EF.OrderTable>)ordersQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            var orders = ordersQuery.ToList();
            return PartialView("_OrderTableDatBanPartial", orders);
        }

        public ActionResult GetRecentOrdersDatBan(int pageSize, int pageNumber)
        {
            var ordersQuery = db.OrderTables.OrderByDescending(order => order.CreatedDate);

            if (pageSize > 0)
            {
                ordersQuery = (IOrderedQueryable<Models.EF.OrderTable>)ordersQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            var orders = ordersQuery.ToList();
            return PartialView("_OrderTableDatBanPartial", orders);
        }
















        public ActionResult GetSoLuongTheoNamThang()
        {
            string donhangtheotuan = string.Empty;
            string donhangtheothang = string.Empty;
            int tongDonHangTrongTuan = 0;
            int tongDonHangTrongNam = 0;
            int tongDonHangCODTrongTuan = 0;
            int tongDonHangCODTrongNam = 0;
            int tongDonHangVNPAYTrongTuan = 0;
            int tongDonHangVNPAYTrongNam = 0;

            var itemOrders = db.Orders.ToList();

            // Lấy ngày hiện tại
            DateTime today = DateTime.Today;

            // Tính ngày đầu tuần (thứ Hai) và ngày cuối tuần (Chủ nhật)
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // Dictionary để lưu trữ số lượng đơn hàng theo ngày trong tuần hiện tại
            Dictionary<DayOfWeek, int> ordersByDayOfWeek = new Dictionary<DayOfWeek, int>
    {
        { DayOfWeek.Monday, 0 },
        { DayOfWeek.Tuesday, 0 },
        { DayOfWeek.Wednesday, 0 },
        { DayOfWeek.Thursday, 0 },
        { DayOfWeek.Friday, 0 },
        { DayOfWeek.Saturday, 0 },
        { DayOfWeek.Sunday, 0 }
    };

            // Lọc đơn hàng theo ngày trong tuần hiện tại
            foreach (var order in itemOrders)
            {
                var orderDate = order.CreatedDate; // Giả sử bạn có một trường OrderDate trong model Order

                if (orderDate >= startOfWeek && orderDate <= endOfWeek)
                {
                    var dayOfWeek = orderDate.DayOfWeek;
                    if (ordersByDayOfWeek.ContainsKey(dayOfWeek))
                    {
                        ordersByDayOfWeek[dayOfWeek]++;
                        tongDonHangTrongTuan++;

                        if (order.TypePayment == "COD")
                        {
                            tongDonHangCODTrongTuan++;
                        }
                        else if (order.TypePayment == "VNPAY")
                        {
                            tongDonHangVNPAYTrongTuan++;
                        }
                    }
                }

                if (orderDate.Year == today.Year)
                {
                    tongDonHangTrongNam++;

                    if (order.TypePayment == "COD")
                    {
                        tongDonHangCODTrongNam++;
                    }
                    else if (order.TypePayment == "VNPAY")
                    {
                        tongDonHangVNPAYTrongNam++;
                    }
                }
            }

            // Chuyển đổi kết quả thành chuỗi
            donhangtheotuan = string.Join(", ", ordersByDayOfWeek.OrderBy(kv => kv.Key).Select(kv => kv.Value));

            // Lấy số lượng đơn hàng theo tháng
            Dictionary<int, int> ordersByMonth = new Dictionary<int, int>();
            for (int month = 1; month <= 12; month++)
            {
                ordersByMonth[month] = 0;
            }

            foreach (var order in itemOrders)
            {
                var orderDate = order.CreatedDate; // Giả sử bạn có một trường OrderDate trong model Order
                var month = orderDate.Month;
                if (ordersByMonth.ContainsKey(month))
                {
                    ordersByMonth[month]++;
                }
            }

            // Chuyển đổi kết quả thành chuỗi
            donhangtheothang = string.Join(", ", ordersByMonth.OrderBy(kv => kv.Key).Select(kv => kv.Value));

            // Đưa dữ liệu vào ViewBag để sử dụng trong View
            //ViewBag.DonHangTheoTuan = donhangtheotuan;
            //ViewBag.DonHangTheoThang = donhangtheothang;

            string donbantheotuan = string.Empty;
            string donbantheothang = string.Empty;
            int tongDonBanTrongTuan = 0;
            int tongDonBanTrongNam = 0;
            int tongDonBanCODTrongTuan = 0;
            int tongDonBanCODTrongNam = 0;
            int tongDonBanVNPAYTrongTuan = 0;
            int tongDonBanVNPAYTrongNam = 0;

            var itemOrderTable = db.OrderTables.ToList();

            // Dictionary để lưu trữ số lượng đơn hàng theo ngày trong tuần hiện tại
            Dictionary<DayOfWeek, int> orderTableByDayOfWeek = new Dictionary<DayOfWeek, int>
    {
        { DayOfWeek.Monday, 0 },
        { DayOfWeek.Tuesday, 0 },
        { DayOfWeek.Wednesday, 0 },
        { DayOfWeek.Thursday, 0 },
        { DayOfWeek.Friday, 0 },
        { DayOfWeek.Saturday, 0 },
        { DayOfWeek.Sunday, 0 }
    };

            // Lọc đơn hàng theo ngày trong tuần hiện tại
            foreach (var order in itemOrderTable)
            {
                var orderDate = order.CreatedDate; // Giả sử bạn có một trường OrderDate trong model Order

                if (orderDate >= startOfWeek && orderDate <= endOfWeek)
                {
                    var dayOfWeek = orderDate.DayOfWeek;
                    if (orderTableByDayOfWeek.ContainsKey(dayOfWeek))
                    {
                        orderTableByDayOfWeek[dayOfWeek]++;
                        tongDonBanTrongTuan++;

                        if (order.TypePayment == "COD")
                        {
                            tongDonBanCODTrongTuan++;
                        }
                        else if (order.TypePayment == "VNPAY")
                        {
                            tongDonBanVNPAYTrongTuan++;
                        }
                    }
                }

                if (orderDate.Year == today.Year)
                {
                    tongDonBanTrongNam++;

                    if (order.TypePayment == "COD")
                    {
                        tongDonBanCODTrongNam++;
                    }
                    else if (order.TypePayment == "VNPAY")
                    {
                        tongDonBanVNPAYTrongNam++;
                    }
                }
            }

            // Chuyển đổi kết quả thành chuỗi
            donbantheotuan = string.Join(", ", orderTableByDayOfWeek.OrderBy(kv => kv.Key).Select(kv => kv.Value));

            // Lấy số lượng đơn hàng theo tháng
            Dictionary<int, int> orderTableByMonth = new Dictionary<int, int>();
            for (int month = 1; month <= 12; month++)
            {
                orderTableByMonth[month] = 0;
            }

            foreach (var order in itemOrderTable)
            {
                var orderDate = order.CreatedDate; // Giả sử bạn có một trường OrderDate trong model Order
                var month = orderDate.Month;
                if (orderTableByMonth.ContainsKey(month))
                {
                    orderTableByMonth[month]++;
                }
            }

            // Chuyển đổi kết quả thành chuỗi
            donbantheothang = string.Join(", ", orderTableByMonth.OrderBy(kv => kv.Key).Select(kv => kv.Value));

            // Đưa dữ liệu vào ViewBag để sử dụng trong View
            ViewBag.DonHangTheoTuan = donhangtheotuan;
            ViewBag.DonHangTheoThang = donhangtheothang;
            ViewBag.TongDonHangTrongTuan = tongDonHangTrongTuan;
            ViewBag.TongDonHangTrongNam = tongDonHangTrongNam;
            ViewBag.TongDonHangCODTrongTuan = tongDonHangCODTrongTuan;
            ViewBag.TongDonHangCODTrongNam = tongDonHangCODTrongNam;
            ViewBag.TongDonHangVNPAYTrongTuan = tongDonHangVNPAYTrongTuan;
            ViewBag.TongDonHangVNPAYTrongNam = tongDonHangVNPAYTrongNam;

            ViewBag.DonBanTheoTuan = donbantheotuan;
            ViewBag.DonBanTheoThang = donbantheothang;
            ViewBag.TongDonBanTrongTuan = tongDonBanTrongTuan;
            ViewBag.TongDonBanTrongNam = tongDonBanTrongNam;
            ViewBag.TongDonBanCODTrongTuan = tongDonBanCODTrongTuan + tongDonHangCODTrongTuan;
            ViewBag.TongDonBanCODTrongNam = tongDonBanCODTrongNam + tongDonHangCODTrongNam;
            ViewBag.TongDonBanVNPAYTrongTuan = tongDonBanVNPAYTrongTuan + tongDonHangVNPAYTrongTuan;
            ViewBag.TongDonBanVNPAYTrongNam = tongDonBanVNPAYTrongNam + tongDonHangVNPAYTrongNam;

            var itemOrder = db.Orders.ToList();
            int COD = itemOrder.Where(x => x.TypePayment == "COD").ToList().Count();
            ViewBag.countOrder = itemOrder.Count;

            var itemOrdertable = db.OrderTables.ToList();
            int VNPAY = itemOrdertable.Where(x => x.TypePayment == "VNPAY").ToList().Count();
            ViewBag.countOrderTable = itemOrdertable.Count;

            ViewBag.COD = COD;
            ViewBag.VNPAY = VNPAY;

            return PartialView("ThongKeThangNam");
        }


        public ActionResult MonAn()
        {
            // Lấy tất cả chi tiết đơn hàng từ cơ sở dữ liệu
            var items = db.OrdersDetails.ToList();

            // Nhóm các mục theo ProductID, sắp xếp theo số lượng mục trong mỗi nhóm theo thứ tự giảm dần và lấy 10 nhóm đầu tiên
            var topProductGroups = items
                .GroupBy(item => item.ProductID)
                .OrderByDescending(group => group.Count())
                .Take(10)
                .ToList();

            // Tính tổng số lần xuất hiện của 10 sản phẩm nhiều nhất
            int totalCount = topProductGroups.Sum(g => g.Count());

            // Lấy danh sách ProductID từ 10 nhóm này
            var topProductIds = topProductGroups.Select(g => g.Key).ToList();

            // Truy vấn thông tin chi tiết từ bảng Product dựa trên danh sách ProductID
            var productInfos = db.Products
                .Where(product => topProductIds.Contains(product.Id))
                .ToList();

            var topItems = topProductGroups
                .Join(productInfos, g => g.Key, p => p.Id, (g, p) => new TopProductViewModel
                {
                    ProductName = p.Title,
                    ProductUrl = p.ProductCategoryID + "-" + p.Id,  // Giả sử cột đường dẫn là Alias
                    Percentage = Math.Floor((double)g.Count() / totalCount * 100),  // Lấy phần nguyên của phần trăm
                    Count = g.Count()
                })
                .ToArray();

            // Truyền mảng thông tin đến view
            return PartialView("ThongKeMonAn", topItems);

        }

        public ActionResult AboutOrder()
        {

            return View();
        }
    }
}