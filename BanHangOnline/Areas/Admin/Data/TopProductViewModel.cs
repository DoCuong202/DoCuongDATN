using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanHangOnline.Areas.Admin.Data
{
    public class TopProductViewModel
    {
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
        public double Percentage { get; set; }
        public int Count { get; set; }
    }
}