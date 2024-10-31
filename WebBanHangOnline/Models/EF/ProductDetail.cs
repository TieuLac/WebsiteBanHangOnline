using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_ProductDetail")]
    public class ProductDetail : CommonAbstract
    {
        public ProductDetail()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SizeId { get; set; }

        public int ColorId { get; set; }

        //public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string SKU { get; set; }

        public bool IsActive { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }

        [ForeignKey("ColorId")]
        public virtual Color Color { get; set; }


        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}