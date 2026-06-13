using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    public class ProductColor
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string ColorName { get; set; }

        [StringLength(10)]
        public string ColorCode { get; set; }

        //[ForeignKey("ProductId")]
        //public virtual Product Product { get; set; }

        //public virtual ICollection<ProductInventory> Inventories { get; set; }
    }
}