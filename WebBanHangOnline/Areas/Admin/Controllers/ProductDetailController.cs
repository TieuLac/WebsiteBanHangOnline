using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
using System.Data.Entity;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductDetailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductDetail
        public ActionResult Index(string Searchtext, int? page)
        {
            IEnumerable<ProductDetail> items = db.ProductDetails.OrderByDescending(x => x.Id);
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                //items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
                items = items.Where(x => x.Product.Alias.Contains(Searchtext) || x.Product.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Add()
        {
            ViewBag.Product = new SelectList(db.Products.ToList(), "Id", "Title");
            ViewBag.Color = new SelectList(db.Colors.ToList(), "Id", "Name");
            ViewBag.Size = new SelectList(db.Sizes.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductDetail model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin Product và Category
                    var productCategory = db.Products.Include(x => x.ProductCategory).FirstOrDefault(x => x.Id == model.ProductId);
                    var product = db.Products.FirstOrDefault(x => x.Id == model.ProductId);
                    var color = db.Colors.FirstOrDefault(x => x.Id == model.ColorId); // Sử dụng model.ColorId
                    var size = db.Sizes.FirstOrDefault(x => x.Id == model.SizeId);

                    if (product != null && color != null && size != null && productCategory != null)
                    {
                        // Tạo SKU từ categoryName-color-size
                        string categoryName = productCategory.ProductCategory.Title;
                        string productName = product.Title;
                        string colorName = color.Name;
                        string sizeName = size.Name;

                        // Loại bỏ dấu và ký tự đặc biệt từ categoryName
                        categoryName = WebBanHangOnline.Models.Commons.Filter.FilterChar(categoryName);
                        productName = WebBanHangOnline.Models.Commons.Filter.FilterChar(productName);
                        colorName = WebBanHangOnline.Models.Commons.Filter.FilterChar(colorName);
                        sizeName = WebBanHangOnline.Models.Commons.Filter.FilterChar(sizeName);

                        model.SKU = $"{categoryName}-{productName}-{colorName}-{sizeName}";

                        // Kiểm tra SKU trùng
                        if (IsSKUExists(model.SKU))
                        {
                            ModelState.AddModelError("", "SKU đã tồn tại!");
                            return View(model);
                        }

                        model.CreatedDate = DateTime.Now;
                        model.ModifiedDate = DateTime.Now;
                        db.ProductDetails.Add(model);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.Message);
                }
            }
            ViewBag.Product = new SelectList(db.Products.ToList(), "Id", "Title");
            ViewBag.Color = new SelectList(db.Colors.ToList(), "Id", "Name");
            ViewBag.Size = new SelectList(db.Sizes.ToList(), "Id", "Name");
            return View(model);
        }
        private bool IsSKUExists(string sku)
        {
            return db.ProductDetails.Any(x => x.SKU == sku);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Product = new SelectList(db.Products.ToList(), "Id", "Title");
            ViewBag.Color = new SelectList(db.Colors.ToList(), "Id", "Name");
            ViewBag.Size = new SelectList(db.Sizes.ToList(), "Id", "Name");
            var item = db.ProductDetails.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDetail model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    // Lấy thông tin Product và Category
                    var productCategory = db.Products.Include(x => x.ProductCategory).FirstOrDefault(x => x.Id == model.ProductId);
                    var product = db.Products.FirstOrDefault(x => x.Id == model.ProductId);
                    var color = db.Colors.FirstOrDefault(x => x.Id == model.ColorId); // Sử dụng model.ColorId
                    var size = db.Sizes.FirstOrDefault(x => x.Id == model.SizeId);

                    if (product != null && color != null && size != null && productCategory != null)
                    {
                        // Tạo SKU từ categoryName-color-size
                        string categoryName = productCategory.ProductCategory.Title;
                        string productName = product.Title;
                        string colorName = color.Name;
                        string sizeName = size.Name;

                        // Loại bỏ dấu và ký tự đặc biệt từ categoryName
                        categoryName = WebBanHangOnline.Models.Commons.Filter.FilterChar(categoryName);
                        productName = WebBanHangOnline.Models.Commons.Filter.FilterChar(productName);
                        colorName = WebBanHangOnline.Models.Commons.Filter.FilterChar(colorName);
                        sizeName = WebBanHangOnline.Models.Commons.Filter.FilterChar(sizeName);

                        model.SKU = $"{categoryName}-{productName}-{colorName}-{sizeName}";

                        // Kiểm tra SKU trùng
                        if (IsSKUExists(model.SKU))
                        {
                            ModelState.AddModelError("", "SKU đã tồn tại!");
                            return View(model);
                        }
                    }
                }
                db.ProductDetails.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductDetails.Find(id);
            if (item != null)
            {
                db.ProductDetails.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.ProductDetails.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = db.ProductDetails.Find(Convert.ToInt32(item));
                        db.ProductDetails.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}