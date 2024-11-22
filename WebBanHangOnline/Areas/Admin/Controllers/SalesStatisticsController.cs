using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class SalesStatisticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/SalesStatistics
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };

            //lọc theo khoảng thời gian: theo ngày
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate < endDate);
            }


            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice), //nhập
                TotalSell = x.Sum(y => y.Quantity * y.Price), //bán
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetStatistical(string fromDate = "", string toDate = "")
        //{
        //    DateTime? from = string.IsNullOrEmpty(fromDate)
        //        ? (DateTime?)null
        //        : DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //    DateTime? to = string.IsNullOrEmpty(toDate)
        //        ? (DateTime?)null
        //        : DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //    var data = db.OrderDetails
        //        .Include(od => od.Order)
        //        .Include(od => od.Product)
        //        .Where(od => od.Order.IsApprove == true)
        //        .Where(od => from == null || od.Order.CreatedDate >= from)
        //        .Where(od => to == null || od.Order.CreatedDate <= to)
        //        .GroupBy(od => od.Order.CreatedDate.Date)
        //        .Select(g => new
        //        {
        //            Date = g.Key,
        //            // Calculate total sales based on selling price or promotional price
        //            DoanhThu = g.Sum(od =>
        //                od.Quantity * (od.Product.PriceSale > 0
        //                    ? od.Product.PriceSale
        //                    : od.Product.Price)),

        //            // Calculate profit by subtracting import price from selling/promotional price
        //            LoiNhuan = g.Sum(od =>
        //                od.Quantity * (
        //                    (od.Product.PriceSale > 0
        //                        ? od.Product.PriceSale
        //                        : od.Product.Price)
        //                    - od.Product.OriginalPrice))
        //        })
        //        .OrderBy(x => x.Date)
        //        .ToList();

        //    return Json(new { Success = true, Data = data });
        //}
    }
}