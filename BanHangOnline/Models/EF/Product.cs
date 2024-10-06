using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Models.EF
{
    [Table("tb_Product")]
    public class Product : CommonAstract
    {
        public Product() { 
            this.ProductImages = new HashSet<ProductImage>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.OrderTableDetails = new HashSet<OrderTableDetail>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Alias { get; set; }

        public string Title { get; set; }

        public string ProductCode { get; set; }

        public int ProductCategoryID { get; set; }

        public string Description { get; set; }
        [AllowHtml]
        public string Detail { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public decimal PriceSale { get; set; }

        public bool IsHome { get; set; }
        public bool IsSale { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHot { get; set; }
        public bool IsActive { get; set; }
        public bool IsSet { get; set; }

        public int Quantity { get; set; }

        public string SeoTitle { get; set; }

        public string SeoDescription { get; set; }

        public string SeoKeywords { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ICollection<ProductImage> ProductImages {  get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<OrderTableDetail> OrderTableDetails { get; set; }
    }
}