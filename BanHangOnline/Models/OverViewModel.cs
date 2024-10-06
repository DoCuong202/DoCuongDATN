using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanHangOnline.Models
{
    public class OverViewModel
    {
        [Required(ErrorMessage = "Vui lòng không để trống tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống điện chỉ")]
        public string Address { get; set; }

        public string TyPayment {  get; set; }

        public int TyPaymentVN { get; set; }
    }
}