using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class OrderCompletedController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/OrderCompleted
        public ActionResult Index(string Searchtext, int? page)
        {
            //var items = db.Orders.OrderBy(x => x.CreatedDate).ToList();

            var items = db.Orders
                  .Where(o => o.OrderStatus == 4 || o.OrderStatus == 3 || o.OrderStatus == 7)
                  .OrderByDescending(x => x.CreatedDate)
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
    }
}