using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductImages
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = db.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            db.ProductImages.Add(new ProductImage()
            {
                ProductId = productId,
                Image = url,
                IsDefault = false,
            });
            db.SaveChanges();
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductImages.Find(id);
            db.ProductImages.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult DeleteAll(int productId)
        {
            try
            {
                var images = db.ProductImages.Where(x => x.ProductId == productId).ToList();
                db.ProductImages.RemoveRange(images);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult IsDefault(int id)
        {
            var item = db.ProductImages.Find(id);
            if (item != null)
            {
                // Đặt tất cả ảnh khác về false
                db.ProductImages.Where(x => x.ProductId == item.ProductId).ToList().ForEach(x => x.IsDefault = false);
                // Đặt ảnh hiện tại là mặc định
                item.IsDefault = true;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}