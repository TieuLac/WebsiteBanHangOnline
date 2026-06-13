using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ApproveOrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ApproveOrder
        public ActionResult Index(string Searchtext, int? page)
        {
            //var items = db.Orders.OrderBy(x => x.CreatedDate).ToList();
            var items = db.Orders
                  .Where(x => x.IsApprove == false) // Chỉ lấy đơn hàng chưa duyệt
                  .OrderBy(x => x.CreatedDate) // Sắp xếp theo ngày tạo
                  .ToList();

            if (page == null)
            {
                page = 1; 
            }

            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Phone.Contains(Searchtext) || x.CustomerName.Contains(Searchtext) || x.Code.Contains(Searchtext)).ToList();
            }

            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public JsonResult ApproveOrder(int id)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.IsApprove = true;
                db.Entry(item).Property(x => x.TypePayment).IsModified = true;
                db.SaveChanges();
                return Json(new { Success = true });
            }
            return Json(new { Success = false, message = "Không tìm thấy đơn hàng" });
        }
    }
}