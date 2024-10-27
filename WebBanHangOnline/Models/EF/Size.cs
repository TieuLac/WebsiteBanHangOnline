using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Size")]
    public class Size : CommonAbstract
    {
        public Size() 
        {
            this.ProductDetail = new HashSet<ProductDetail>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public decimal? Width { get; set; }     // Chiều rộng

        public decimal? Height { get; set; }    // Chiều cao

        public decimal? Length { get; set; }    // Chiều dài

        public decimal? Weight { get; set; }    // Cân nặng

        public string Measurement { get; set; } // Đơn vị đo

        public int DisplayOrder { get; set; } // Thứ tự hiển thị

        public virtual ICollection<ProductDetail> ProductDetail { get; set; }
    }
}