using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class OrderController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var items = db.Orders.OrderByDescending(x => x.CreatedDate).ToList();

            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber, pageSize));
        }



        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham(int id)
        {
            var items = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                //kiểm tra trạng thái 
                //nếu đơn hàng đã hoàn thành, hủy hoặc giao hàng không thành công thì không được cập nhật nữa
                if (item.OrderStatus == 3 || item.OrderStatus == 4 || item.OrderStatus == 7)
                {
                    return Json(new { message = "Không thể thay đổi trạng thái của đơn hàng", Success = false });
                }

                //nếu đơn hàng là "đang giao" thì không thể "hủy" 
                if (item.OrderStatus == 5 && trangthai == 4)
                {
                    return Json(new { message = "Không thể hủy đơn hàng đang giao", Success = false });
                }
                //nếu đơn hàng là "đang giao" thì không thể thay đổi trạng thái thành đang chuẩn bị hàng
                if (item.OrderStatus == 5 && trangthai == 6)
                {
                    return Json(new { message = "Không thể thay đổi trạng thái khi đơn hàng đang giao", Success = false });
                }

                //nếu đơn hàng được cập nhật trạng thái thành "Hủy" hoặc "Giao không thành công" thì cộng lại số lượng sản phẩm vào bảng Product
                if(trangthai == 4 || trangthai == 7)
                {
                    foreach (var orderDetail in item.OrderDetails)
                    {
                        var product = db.Products.Find(orderDetail.ProductId);
                        if (product != null)
                        {
                            product.Quatity += orderDetail.Quantity;
                            db.Entry(product).Property(x => x.Quatity).IsModified = true;
                        }
                    }
                }

                //nếu đơn hàng đang ở trạng thái chuẩn bị hàng thì chỉ được thay đổi trạng thái thành hủy hoặc đang giao hàng
                if(item.OrderStatus == 6)
                {
                    if(trangthai == 3) //khi đang chuẩn bị thì không được hoàn thành đơn
                    {
                        return Json(new { message = "Đơn hàng đang chuẩn bị không thể hoàn thành", Success = false });
                    }
                    else if(trangthai == 7) //khi đang chuẩn bị thì đơn hàng không được phép giao không thành công
                    {
                        return Json(new { message = "Đơn hàng đang chuẩn bị không thể giao hàng không thành công", Success = false });
                    }
                }

                //khi vừa tiếp nhận đơn hàng thì phải chuẩn bị hàng mới tới bước tiếp theo
                if (item.OrderStatus == 1 || item.OrderStatus == 2)
                {
                    if (trangthai == 3 || trangthai == 4 || trangthai == 5 || trangthai == 7) //khi đang chuẩn bị thì không được hoàn thành đơn
                    {
                        return Json(new { message = "Đơn hàng chưa được chuẩn bị", Success = false });
                    }
                }

                    //Nếu đơn hàng là COD
                    //Khi hoàn thành thì trạng thái thanh toán thành "Đã thanh toán"
                    if (item.Status == 1 && trangthai == 3)
                {
                    db.Orders.Attach(item);
                    item.Status = 2;
                    item.OrderStatus = trangthai;
                    db.Entry(item).Property(x => x.TypePayment).IsModified = true;
                    db.SaveChanges();
                    return Json(new { message = "Success", Success = true });
                }

                
                //---------//
                db.Orders.Attach(item);
                //item.TypePayment = trangthai;
                item.OrderStatus = trangthai;
                db.Entry(item).Property(x => x.TypePayment).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }


        //public void ThongKe(string fromDate, string toDate)
        //{
        //    var query = from o in db.Orders
        //                join od in db.OrderDetails on o.Id equals od.OrderId
        //                join p in db.Products
        //                on od.ProductId equals p.Id
        //                select new
        //                {
        //                    CreatedDate = o.CreatedDate,
        //                    Quantity = od.Quantity,
        //                    Price = od.Price,
        //                    OriginalPrice = p.Price
        //                };
        //    if (!string.IsNullOrEmpty(fromDate))
        //    {
        //        DateTime start = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
        //        query = query.Where(x => x.CreatedDate >= start);
        //    }
        //    if (!string.IsNullOrEmpty(toDate))
        //    {
        //        DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
        //        query = query.Where(x => x.CreatedDate < endDate);
        //    }
        //    var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(r => new
        //    {
        //        Date = r.Key.Value,
        //        TotalBuy = r.Sum(x => x.OriginalPrice * x.Quantity), // tổng giá bán
        //        TotalSell = r.Sum(x => x.Price * x.Quantity) // tổng giá mua
        //    }).Select(x => new RevenueStatisticViewModel
        //    {
        //        Date = x.Date,
        //        Benefit = x.TotalSell - x.TotalBuy,
        //        Revenues = x.TotalSell
        //    });
        //}
    }
}