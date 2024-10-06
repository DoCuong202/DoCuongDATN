using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using VNPAY_CS_ASPX;

namespace BanHangOnline.Controllers
{
    public class OrderTableController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: OrderTable
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getTable(string day, string TimeStart, string TimeEnd)
        {

            if (string.IsNullOrWhiteSpace(day) ||
                string.IsNullOrWhiteSpace(TimeStart) ||
                string.IsNullOrWhiteSpace(TimeEnd))
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
            }
            // Parse strings to DateTime outside the LINQ query
            DateTime parsedDay = DateTime.Parse(day);
            TimeSpan start1 = DateTime.Parse(TimeStart).TimeOfDay;
            TimeSpan end1 = DateTime.Parse(TimeEnd).TimeOfDay;

            // Xác định các khoảng thời gian hợp lệ
            TimeSpan startMorning = new TimeSpan(10, 0, 0); // 10:00 sáng
            TimeSpan endMorning = new TimeSpan(14, 0, 0); // 14:00 chiều
            TimeSpan startEvening = new TimeSpan(17, 0, 0); // 17:00 chiều
            TimeSpan endEvening = new TimeSpan(23, 0, 0); // 23:00 tối

            // Kiểm tra xem startTime có nằm trong khoảng thời gian hợp lệ không
            bool isStartTimeValid = (start1 >= startMorning && start1 <= endMorning) || (start1 >= startEvening && start1 <= endEvening);

            // Kiểm tra xem endTime có nằm trong khoảng thời gian hợp lệ không
            bool isEndTimeValid = (end1 >= startMorning && end1 <= endMorning) || (end1 >= startEvening && end1 <= endEvening);

            // Nếu startTime hoặc endTime không hợp lệ, trả về thông báo lỗi
            if (!isStartTimeValid || !isEndTimeValid)
            {
                return Json(new { success = false, message = "Thời gian bắt đầu hoặc kết thúc không nằm trong khoảng thời gian hợp lệ." });
            }

            // Kiểm tra xem startTime có bé hơn endTime không
            if (start1 >= end1)
            {
                return Json(new { success = false, message = "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc." });
            }


            var strRs = "";
            var matchingOrders = db.OrderTables.Where(order => order.Day == parsedDay).ToList();
            foreach (var item in matchingOrders)
            {
                TimeSpan start2 = DateTime.Parse(item.TimeStart).TimeOfDay;
                TimeSpan end2 = DateTime.Parse(item.TimeEnd).TimeOfDay;
                if (DoTimeRangesOverlap(start1, end1, start2, end2) || DoesTimeRangeContain(start1, end1, start2, end2))
                {
                    strRs += item.IDTable + ",";
                }
            }

            return Json(new { success = true, message = "Danh sach cac ma ban", data = strRs });
        }

        bool DoTimeRangesOverlap(TimeSpan start1, TimeSpan end1, TimeSpan start2, TimeSpan end2)
        {
            return (start1 <= end2 && end1 >= start2);
        }

        bool DoesTimeRangeContain(TimeSpan start1, TimeSpan end1, TimeSpan start2, TimeSpan end2)
        {
            return (start2 >= start1 && end2 <= end1);
        }


        [HttpPost]
        public ActionResult Add(string key, string day, string startTime, string endTime, string tableChoice, string nameCustomer, string phoneCustomer, string countCustomer, string note)
        {
            var code = new { Success = false, message = "", code = "", Url = "", type = 0 };
            if (string.IsNullOrWhiteSpace(day) ||
                string.IsNullOrWhiteSpace(startTime) ||
                string.IsNullOrWhiteSpace(endTime) ||
                string.IsNullOrWhiteSpace(tableChoice) ||
                string.IsNullOrWhiteSpace(nameCustomer) ||
                string.IsNullOrWhiteSpace(phoneCustomer) ||
                string.IsNullOrWhiteSpace(countCustomer) ||
                string.IsNullOrWhiteSpace(note))
            {
                return Json(new { Success = false, message = "Vui lòng điền đầy đủ thông tin." });
            }

            // Chuyển đổi startTime và endTime từ chuỗi thành TimeSpan
            TimeSpan start, end;
            bool isValidStartTime = TimeSpan.TryParse(startTime, out start);
            bool isValidEndTime = TimeSpan.TryParse(endTime, out end);

            // Kiểm tra xem startTime và endTime có được chuyển đổi thành công không
            if (!isValidStartTime || !isValidEndTime)
            {
                return Json(new { success = false, message = "Thời gian bắt đầu hoặc kết thúc không hợp lệ." });
            }

            // Xác định các khoảng thời gian hợp lệ
            TimeSpan startMorning = new TimeSpan(10, 0, 0); // 10:00 sáng
            TimeSpan endMorning = new TimeSpan(14, 0, 0); // 14:00 chiều
            TimeSpan startEvening = new TimeSpan(17, 0, 0); // 17:00 chiều
            TimeSpan endEvening = new TimeSpan(23, 0, 0); // 23:00 tối

            // Kiểm tra xem startTime có nằm trong khoảng thời gian hợp lệ không
            bool isStartTimeValid = (start >= startMorning && start <= endMorning) || (start >= startEvening && start <= endEvening);

            // Kiểm tra xem endTime có nằm trong khoảng thời gian hợp lệ không
            bool isEndTimeValid = (end >= startMorning && end <= endMorning) || (end >= startEvening && end <= endEvening);

            // Nếu startTime hoặc endTime không hợp lệ, trả về thông báo lỗi
            if (!isStartTimeValid || !isEndTimeValid)
            {
                return Json(new { success = false, message = "Thời gian bắt đầu hoặc kết thúc không nằm trong khoảng thời gian hợp lệ." });
            }

            // Kiểm tra xem startTime có bé hơn endTime không
            if (start >= end)
            {
                return Json(new { success = false, message = "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc." });
            }


            DateTime parsedDay = DateTime.Parse(day);
            Random rd = new Random();
            var id = "HD" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            // Tạo một đối tượng OrderTable mới
            OrderTable order = new OrderTable
            {
                IDTable = tableChoice,
                CustomerName = nameCustomer,
                Phone = phoneCustomer,
                Day = parsedDay,
                CountCustomer = countCustomer,
                TimeStart = startTime,
                TimeEnd = endTime,
                note = note,
                statusOrder = -1,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
               
                Code = id
            };
        

            if (User.Identity.IsAuthenticated)
            {
                order.CreatedBy = User.Identity.GetUserId();
            }
            else
            {
                order.CreatedBy = "Customer";
            }
            db.OrderTables.Add(order);
            db.SaveChanges();
            code = new { Success = true, message = "", code = order.Code, Url = "", type = 1 };
            return Json(code);
        }

        public ActionResult ToOrderDetail(string code)
        {
            var orderDetail = db.OrderTables.Where(x => x.Code == code).FirstOrDefault();
            return View(orderDetail);
        }

        public ActionResult ProductInOrderDetail(int Id)
        {
            var ProductOrderDetail = db.OrderTableDetails.Where(x => x.OrderTableID == Id).ToList();
            //return View(ProductOrderDetail);
            return PartialView(ProductOrderDetail);
        }

        public ActionResult ProductInTable()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return View(cart.Items);
            }
            return View();
        }

        public ActionResult view1()
        {
            return View();
        }

        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                OverViewTable table = (OverViewTable)Session["ViewTable"];
                ViewBag.day = table.day;
                ViewBag.starttime = table.starttime;
                ViewBag.endtime = table.endtime;
                ViewBag.code = table.code;
                ViewBag.name = table.name;
                ViewBag.phone = table.phone;
                ViewBag.count =     table.count;
                ViewBag.note = table.note;
                return View(cart.Items);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddWithProduct( string TyPaymentVN)
        {
            var code = new { Success = false, code = "", Url = "", type = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            OverViewTable table = (OverViewTable)Session["ViewTable"];
            if (cart != null)
            {
                OrderTable order = new OrderTable();
                order.IDTable = table.code;
                order.CustomerName = table.name;
                order.Phone = table.phone;
                order.Day = DateTime.Parse(table.day);
                order.CountCustomer = table.count;
                order.TimeStart = table.starttime;
                order.TimeEnd = table.endtime;
                order.note = table.note;
                order.statusOrder = -1;
                order.CreatedDate = DateTime.Now;
                order.ModifiedDate = DateTime.Now;
                cart.Items.ForEach(x => order.OrderTableDetails.Add(new OrderTableDetail
                {
                    ProductID = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }));
                order.Quantity = cart.Items.Sum(x => x.Quantity);
                order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                order.TypePayment = "VNPAY";
                order.statusPayment = 0;
                order.statusOrder = -1;
                if (User.Identity.IsAuthenticated)
                {
                    order.CreatedBy = User.Identity.GetUserId();
                }
                else
                {
                    order.CreatedBy = "Customer";
                }
                Random rd = new Random();
                order.Code = "HD" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                db.OrderTables.Add(order);
                db.SaveChanges();
                cart.ClearCart();
                
                var url = UrlPayment(Convert.ToInt32(TyPaymentVN), order.Code);
                code = new { Success = true, code = order.Code, Url = url, type = 2 };
            }
            return Json(code);

        }

        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = db.OrderTables.FirstOrDefault(x => x.Code == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_ReturnurlDatBan"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.TotalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }

        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {

                        var itemOrder = db.OrderTables.FirstOrDefault(x => x.Code == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.statusPayment = 1;
                            db.OrderTables.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.code = 0;
                        ViewBag.orderCode = orderCode;
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        var itemOrder = db.OrderTables.FirstOrDefault(x => x.Code == orderCode);
                        if (itemOrder != null)
                        {
                            db.OrderTables.Remove(itemOrder);
                            db.SaveChanges();
                        }
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.code = 1;
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }

        [HttpPost]
        public ActionResult AddOverViewTable(string day, string startTime, string endTime, string code, string name, string phone, string count, string note)
        {
            // Kiểm tra các tham số truyền vào để đảm bảo rằng chúng không bị rỗng hoặc không hợp lệ
            if (string.IsNullOrWhiteSpace(day) ||
                string.IsNullOrWhiteSpace(startTime) ||
                string.IsNullOrWhiteSpace(endTime) ||
                string.IsNullOrWhiteSpace(code) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(count) ||
                string.IsNullOrWhiteSpace(note))
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
            }

            // Tạo một đối tượng OverViewTable mới
            OverViewTable viewTable = new OverViewTable
            {
                day = day,
                starttime = startTime,
                endtime = endTime,
                code = code,
                name = name,
                phone = phone,
                count = count,
                note = note
            };

            // Kiểm tra xem trong session có chứa ViewTable không
            if (Session["ViewTable"] != null)
            {
                // Nếu đã có, ghi đè lên đối tượng cũ
                Session["ViewTable"] = viewTable;
            }
            else
            {
                // Nếu chưa có, thêm mới vào session
                Session["ViewTable"] = viewTable;
            }

            return Json(new { success = true, message = "Thêm thông tin thành công." });
        }

    }
}