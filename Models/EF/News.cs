using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Models.EF
{
    [Table("tb_New")]
    public class News : CommonAstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Alias { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống tiêu đề")]
        [StringLength(250)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Image {  get; set; }

        //public int CategoryID { get; set; }
        [AllowHtml]
        public string Detail { get; set; }

        public string SeoTitle { get; set; }

        public string SeoDescription { get; set; }

        public string SeoKeywords { get; set; }

        public bool IsActive { get; set; }

        public virtual Category Category { get; set; }
    }
}