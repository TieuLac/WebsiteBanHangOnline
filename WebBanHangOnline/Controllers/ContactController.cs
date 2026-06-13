using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Contact
        public ActionResult Index(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult TrackOrder(string contactInfo)
        {
            //var order = db.Orders
            //    .Where(o => o.Code == orderCode && (o.Phone == contactInfo || o.Email == contactInfo))
            //    .FirstOrDefault();

            var order = db.Orders
                .Where(o => (o.Phone == contactInfo || o.Email == contactInfo))
                .ToList();

            

            if (order == null || !order.Any())
            {
                return Content("<p class='text-danger text-center'>Không tìm thấy đơn hàng.</p>");
            }

            return PartialView("_PartialOrderDetails", order);
        }
    }
}