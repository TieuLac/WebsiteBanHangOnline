using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    public class ProductInventory : CommonAbstract
    {
        public ProductInventory()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public int? ColorId { get; set; }
        
        public int? SizeId { get; set; }
        
        public decimal OriginalPrice { get; set; }
        
        public decimal Price { get; set; }
        
        public decimal? PriceSale { get; set; }
        
        public int Quantity { get; set; }
        
        public bool IsActive { get; set; }

        [StringLength(250)]
        public string Alias { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [ForeignKey("ColorId")]
        public virtual ProductColor Color { get; set; }

        [ForeignKey("SizeId")]
        public virtual ProductSize Size { get; set; }

    }
}