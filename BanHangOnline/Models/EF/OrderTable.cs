using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BanHangOnline.Models.EF
{
    [Table("tb_OrderTable")]
    public class OrderTable : CommonAstract
    {
        public OrderTable()
        {
            this.OrderTableDetails = new HashSet<OrderTableDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

       public string IDTable { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống số điện thoại")]
        public string Phone { get; set; }

        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }

        public string CountCustomer { get; set; }

        public DateTime Day { get; set;}

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string TypePayment { get; set; }

        public int statusPayment { get; set; }
        [Required]
        public int statusOrder { get; set; }

        public int status { get; set; }

        public string note { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<OrderTableDetail> OrderTableDetails { get; set; }
    }
}