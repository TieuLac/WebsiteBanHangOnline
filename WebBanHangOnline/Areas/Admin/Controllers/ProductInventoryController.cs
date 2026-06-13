using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductInventoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductInventory
        public ActionResult Index(string Searchtext, int? page)
        {
            IEnumerable<ProductInventory> items = db.ProductInventories.OrderByDescending(x => x.Id);
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
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
            ViewBag.ProductSize = new SelectList(db.ProductSizes.ToList(), "Id", "SizeName");
            ViewBag.ProductColor = new SelectList(db.ProductColors.ToList(), "Id", "ColorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductInventory model, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;

                // Lấy thông tin ProductInventory
                var productInventory = db.ProductInventories.FirstOrDefault(x => x.Id == model.Id);
                if (productInventory != null)
                {
                    // Gán Alias theo định dạng: productid-sizeid-colorid
                    model.Alias = $"{productInventory.ProductId}-{productInventory.SizeId}-{productInventory.ColorId}";
                }

                db.ProductInventories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product = new SelectList(db.Products.ToList(), "Id", "Title");
            ViewBag.ProductSize = new SelectList(db.ProductSizes.ToList(), "Id", "SizeName");
            ViewBag.ProductColor = new SelectList(db.ProductColors.ToList(), "Id", "ColorName");
            return View(model);
        }

        //public ActionResult Edit(int id)
        //{
        //    var item = db.ProductInventories.Find(id);

        //    ViewBag.ProductName = new SelectList(db.Products.ToList(), "Id", "Title", item.ProductId);
        //    ViewBag.ProductSize = new SelectList(db.ProductSizes.ToList(), "Id", "SizeName", item.SizeId);
        //    ViewBag.ProductColor = new SelectList(db.ProductColors.ToList(), "Id", "ColorName", item.ColorId);


        //    return View(item);
        //}


        public ActionResult Edit(int id)
        {
            var item = db.ProductInventories.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            var products = db.Products.ToList();
            var sizes = db.ProductSizes.ToList();
            var colors = db.ProductColors.ToList();

            ViewBag.ProductName = new SelectList(products, "Id", "Title", item.ProductId);
            ViewBag.ProductSize = new SelectList(sizes, "Id", "SizeName", item.SizeId);
            ViewBag.ProductColor = new SelectList(colors, "Id", "ColorName", item.ColorId);

            return View(item);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(ProductInventory model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        model.ModifiedDate = DateTime.Now;

        //        var product = db.Products.FirstOrDefault(p => p.Id == model.Id);
        //        string productName = product != null ? product.Title : "Unknown";

        //        var size = db.ProductSizes.FirstOrDefault(p => p.Id == model.Id);
        //        string sizeName = size != null ? size.SizeName : "Unknown";

        //        var color = db.ProductColors.FirstOrDefault(p => p.Id == model.Id);
        //        string colorName = product != null ? color.ColorName : "Unknown";

        //        // Lấy thông tin ProductInventory
        //        var productInventory = db.ProductInventories.FirstOrDefault(x => x.Id == model.Id);
        //        if (productInventory != null)
        //        {
        //            // Gán Alias theo định dạng: productid-sizeid-colorid
        //            model.Alias = $"{WebBanHangOnline.Models.Commons.Filter.FilterChar(productName)}-{WebBanHangOnline.Models.Commons.Filter.FilterChar(colorName)}-{WebBanHangOnline.Models.Commons.Filter.FilterChar(sizeName)}";
        //        }

        //        db.ProductInventories.Attach(model);
        //        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductInventory model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;

                // Tìm lại thông tin sản phẩm, kích thước, màu sắc từ ProductInventory
                var productInventory = db.ProductInventories.Find(model.Id);
                if (productInventory == null)
                {
                    return HttpNotFound();
                }

                var product = db.Products.FirstOrDefault(p => p.Id == model.ProductId);
                string productName = product != null ? product.Title : "Unknown";

                var size = db.ProductSizes.FirstOrDefault(p => p.Id == model.SizeId);
                string sizeName = size != null ? size.SizeName : "Unknown";

                var color = db.ProductColors.FirstOrDefault(p => p.Id == model.ColorId);
                string colorName = color != null ? color.ColorName : "Unknown";

                //Cập nhật thông tin từ model vào productInventory
                productInventory.Quantity = model.Quantity;
                productInventory.Price = model.Price;
                productInventory.PriceSale = model.PriceSale;
                productInventory.OriginalPrice = model.OriginalPrice;
                productInventory.SizeId = model.SizeId;
                productInventory.ColorId = model.ColorId;
                productInventory.ProductId = model.ProductId;
                productInventory.ModifiedDate = model.ModifiedDate;

                // Gán Alias theo định dạng: productname-colorname-sizename
                productInventory.Alias = $"{WebBanHangOnline.Models.Commons.Filter.FilterChar(productName)}-{WebBanHangOnline.Models.Commons.Filter.FilterChar(colorName)}-{WebBanHangOnline.Models.Commons.Filter.FilterChar(sizeName)}";

                
                db.Entry(productInventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductName = new SelectList(db.Products.ToList(), "Id", "Title");
            ViewBag.ProductSize = new SelectList(db.ProductSizes.ToList(), "Id", "SizeName");
            ViewBag.ProductColor = new SelectList(db.ProductColors.ToList(), "Id", "ColorName");
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductInventories.Find(id);
            if (item != null)
            {
                db.ProductInventories.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
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
                        var obj = db.ProductInventories.Find(Convert.ToInt32(item));
                        db.ProductInventories.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.ProductInventories.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }


    }
}