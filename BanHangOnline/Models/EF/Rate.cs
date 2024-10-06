using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BanHangOnline.Models.EF
{
    [Table("tb_Rates")]
    public class Rate : CommonAstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        public string NameUser { get; set; }
        public string IdProduct { get; set; }
        public int StartRate { get; set; }
        public string ContentRate { get; set; }
        public DateTime NgayTao { get; set; }

    }
}