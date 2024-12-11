using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    //[Authorize]
    public class ReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Review(int productId)
        {
            ViewBag.ProductId = productId;
            var item = new ReviewProduct();
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                if (user != null)
                {
                    item.Email = user.Email;
                    item.FullName = user.FullName;
                    item.UserName = user.UserName;
                }
                return PartialView(item);
            }
            return PartialView();
        }

        public ActionResult _Load_Review(int productId)
        {
            var item = db.Reviews.Where(x => x.ProductId == productId).OrderByDescending(x => x.Id).ToList();
            ViewBag.Count = item.Count;
            return PartialView(item);
        }

        //[HttpPost]
        //public ActionResult PostReview(ReviewProduct req)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        req.CreatedDate = DateTime.Now;
        //        db.Reviews.Add(req);
        //        db.SaveChanges();
        //        return Json(new { Success = true });
        //    }
        //    return Json(new { Success = false });
        //}

        [HttpPost]
        public ActionResult PostReview(ReviewProduct req)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Vui lòng đăng nhập để gửi đánh giá.",
                    NeedLogin = true
                });
            }

            if (ModelState.IsValid)
            {
                req.CreatedDate = DateTime.Now;
                req.UserName = User.Identity.Name; // Đảm bảo gán username của người đăng nhập
                db.Reviews.Add(req);
                db.SaveChanges();
                return Json(new { Success = true });
            }
            return Json(new { Success = false, Message = "Đánh giá không hợp lệ" });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}