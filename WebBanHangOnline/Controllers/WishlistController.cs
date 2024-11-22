using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class WishlistController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Wishlist
        public ActionResult Index(int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Wishlist> items = db.Wishlists.Where(x => x.UserName == User.Identity.Name).OrderByDescending(x => x.CreatedDate);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            
            return View(items);
        }

        [HttpPost]
        public ActionResult PostWishlist(int ProductId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Vui lòng đăng nhập để thêm sản phẩm vào danh sách yêu thích.",
                    NeedLogin = true
                });
            }
            var checkItem = db.Wishlists.FirstOrDefault(x => x.ProductId == ProductId && x.UserName == User.Identity.Name);
            if (checkItem != null)
            {
                return Json(new { Success = false, Message = "Sản phẩm đã có trong danh sách yêu thích.", IsActive = true });
            }
            var item = new Wishlist();
            item.ProductId = ProductId;
            item.UserName = User.Identity.Name;
            item.CreatedDate = DateTime.Now;
            db.Wishlists.Add(item);
            db.SaveChanges();
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult PostDeleteWishlist(int ProductId)
        {
            var checkItem = db.Wishlists.FirstOrDefault(x => x.ProductId == ProductId && x.UserName == User.Identity.Name);
            if (checkItem != null)
            {
                var item = db.Wishlists.Find(checkItem.Id);
                db.Set<Wishlist>().Remove(item);
                var i = db.SaveChanges();
                return Json(new { Success = true, Message = "Xóa sản phẩm ra khỏi danh sách yêu thích thành công." });
            }
            return Json(new { Success = false, Message = "Xóa thất bại." });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}