using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PTUDTMDT.Models;

namespace PTUDTMDT.Controllers
{
    public class productsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: products
        public ActionResult Index(string name,string gia,int ?page,int? pageSize)
        {
            var products = db.products.Select(s=>s);
            if (!string.IsNullOrEmpty(name))
            {
                products=products.Where(p => p.SKU.Contains(name)); 
            }
            if (!string.IsNullOrEmpty(gia))
            {
                decimal price = 0;
                if(decimal.TryParse(gia,out price))
                    products= products.Where(p => p.price > price);
            }
            products= products.OrderBy(p=>p.product_id);
            if (page==null) page=1;
            if (pageSize==null) pageSize=8;
            return View(products.ToPagedList((int)page,(int)pageSize));
        }

        public ActionResult DanhSach(string name, string gia, int? page, int? pageSize)
        {
            var products = db.products.Select(s => s);
            if (!string.IsNullOrEmpty(name))
            {
                products=products.Where(p => p.SKU.Contains(name));
            }
            if (!string.IsNullOrEmpty(gia))
            {
                decimal price = 0;
                if (decimal.TryParse(gia, out price))
                    products= products.Where(p => p.price > price);
            }
            products= products.OrderBy(p => p.product_id);
            if (page==null) page=1;
            if (pageSize==null) pageSize=8;
            return View(products.ToPagedList((int)page, (int)pageSize));
        }

        public ActionResult getBestSellingProducts()
        {
            //var topProducts = (from p in db.products
            //                   join oi in db.order_item on p.product_id equals oi.product_id
            //                   group new { product = p, quantity = oi.quantity } by p.product_id into productGroup
            //                   orderby productGroup.Sum(x => x.quantity) descending
            //                   select new
            //                   {
            //                       SKU = productGroup.FirstOrDefault().product.SKU,
            //                       product_image = productGroup.FirstOrDefault().product.product_image,
            //                       price = productGroup.FirstOrDefault().product.price,
            //                   })
            //                   .Take(4).ToList();

            var topProducts = db.products
    .Join(db.order_item, p => p.product_id, oi => oi.product_id, (p, oi) => new { product = p, quantity = oi.quantity })
    .GroupBy(x => x.product.product_id)
    .OrderByDescending(g => g.Sum(x => x.quantity))
    .Take(4)
    .Select(g => new
    {
        SKU = g.FirstOrDefault().product.SKU,
        product_image = g.FirstOrDefault().product.product_image,
        price = g.FirstOrDefault().product.price
    })
    .ToList();
            
            var product = topProducts.Select(p => new product
            {
                SKU = p.SKU,
                product_image = p.product_image,
                price = p.price,
            }).ToList();

            ViewBag.newProducts =db.products.OrderByDescending(p=>p.created_at).Take(4).ToList(); 
            return View(product);
        }

        public ActionResult getProductsByCat(int id)
        {
            var products = db.products.Where(s => s.category_id == id).OrderBy(p => p.product_id);
            return View(products.ToList());
        }

      


        // GET: products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.products.Find(id);
            var products = db.products.Where(p=> p.category_id == product.category_id).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.products =products;
            return View(product);
        }

        // GET: products/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name");
            return View();
        }

        // POST: products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,SKU,description,price,stock,category_id,product_image")] product product)
        {
            product.created_at = DateTime.Now;
            if (ModelState.IsValid)
            {
                var f = Request.Files["FileName"];

                if (f!=null&&f.ContentLength>0)
                {
                    string tenfile = System.IO.Path.GetFileName(f.FileName);
                    string path = Server.MapPath("~/Content/frontend/img/"+tenfile);
                    f.SaveAs(path);
                    product.product_image = tenfile;    
                }
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("DanhSach");
            }

            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", product.category_id);
            return View(product);
        }

        // GET: products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", product.category_id);
            return View(product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,SKU,description,price,stock,category_id,product_image")] product product)
        {
            if (ModelState.IsValid)
            {

                var f = Request.Files["FileName"];

                if (f!=null&&f.ContentLength>0)
                {
                    string tenfile = System.IO.Path.GetFileName(f.FileName);
                    string duongdan = Server.MapPath("~/Content/frontend/img/"+tenfile);
                    f.SaveAs(duongdan);
                    product.product_image = tenfile;
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSach");
            }
            ViewBag.category_id = new SelectList(db.categories, "category_id", "name", product.category_id);
            return View(product);
        }

        // GET: products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("DanhSach");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
