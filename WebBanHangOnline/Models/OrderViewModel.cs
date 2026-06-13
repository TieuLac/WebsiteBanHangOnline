using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Tên khách hàng không để trống")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không để trống")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ khổng để trống")]
        public string Address { get; set; }
        public string Email { get; set; }
        public string CustomerId { get; set; }
        public int TypePayment { get; set; }
        public int TypePaymentVN { get; set; }
        public string FullAddress { get; set;
        }
        [Required(ErrorMessage = "Vui lòng chọn tỉnh/thành phố.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn quận/huyện.")]
        public string District { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phường/xã.")]
        public string Ward { get; set; }

        //public string FullAddress
        //{
        //    get { return $"{Address}, {Ward}, {District}, {Province}"; }
        //}
    }
}