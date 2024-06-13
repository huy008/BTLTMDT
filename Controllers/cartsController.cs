using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTUDTMDT.Models;

namespace PTUDTMDT.Controllers
{
    public class cartsController : Controller
    {
        private DataContext db = new DataContext();
        // GET: carts
        public ActionResult Index()
        {
            var customer = Session["customer"] as  PTUDTMDT.Models.customer; 
            var carts = db.carts.Where(c=>c.customer_id == customer.customer_id).ToList();

            ViewBag.total = db.carts.Where(c => c.customer_id == customer.customer_id).Sum(c => c.product.price*c.quantity);
            return View(carts);
        }
        

        //public ActionResult AddOrder()
        //{
        //    var customer = Session["customer"] as  PTUDTMDT.Models.customer;
        //    decimal total = (decimal)db.carts.Where(c => c.customer_id == customer.customer_id).Sum(c => c.product.price*c.quantity);

        //    var carts = db.carts
        //            .Where(c => c.customer_id == customer.customer_id)
        //            .Select(c => new // Create new order_item objects
        //            {
        //                quantity = c.quantity,
        //                price = (decimal)(c.product.price * c.quantity),
        //                product_id = c.product.product_id
        //            }).ToList();

        //    db.order_.Add(new order_
        //    {
        //        order_date = DateTime.Now,
        //        order_price = total,
        //        customer_id = customer.customer_id,
        //        order_item = carts.Select(c => new order_item
        //        {
        //            quantity = c.quantity,
        //            price=c.price,
        //            product_id=c.product_id
        //        }).ToList()
        //    }) ; 
        //    db.SaveChanges();
        //    return RedirectToAction("AddShipment");
        //}

        public ActionResult ShowOrder()
        {
            var customer = Session["customer"] as  PTUDTMDT.Models.customer;
            var order = db.order_.Where(c => c.customer_id == customer.customer_id).ToList();
            ViewBag.order=order;
            return View();
        }


        public ActionResult AddShipment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddShipment([Bind(Include = "shipment_date,address,city,status,country,zip_code,customer_id")] shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.shipments.Add(shipment);
                db.SaveChanges();
            }
            var customer = Session["customer"] as  PTUDTMDT.Models.customer;
            decimal total = (decimal)db.carts.Where(c => c.customer_id == customer.customer_id).Sum(c => c.product.price*c.quantity);

            var carts = db.carts
                    .Where(c => c.customer_id == customer.customer_id)
                    .Select(c => new // Create new order_item objects
                    {
                        quantity = c.quantity,
                        price = (decimal)(c.product.price * c.quantity),
                        product_id = c.product.product_id
                    }).ToList();

            db.order_.Add(new order_
            {
                order_date = DateTime.Now,
                order_price = total,
                customer_id = customer.customer_id,
                shipment = shipment,
                order_item = carts.Select(c => new order_item
                {
                    quantity = c.quantity,
                    price=c.price,
                    product_id=c.product_id
                }).ToList()
            });
            db.SaveChanges();
            TempData["toast"] = "Đặt hàng thành công";
            return RedirectToAction("Index","products");
        }
        // GET: carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }
        public ActionResult Add(int id,int customer_id)
        {
            product product = db.products.Find(id);
            cart cartItem = db.carts.Where(c => c.product_id == id && c.customer_id == customer_id).FirstOrDefault();
            if(cartItem == null)
            {
                db.carts.Add(new cart(1, id, customer_id));
                db.SaveChanges();
            }
            else
            {
              cartItem.quantity += 1;
                db.SaveChanges();
            }
            var carts = db.carts.Where(c => c.customer_id == customer_id).ToList();

            ViewBag.total = db.carts.Where(c => c.customer_id == customer_id).Sum(c => c.product.price*c.quantity);
            return View("Index", carts);
        }
        // GET: carts/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name");
            ViewBag.product_id = new SelectList(db.products, "product_id", "SKU");
            return View();
        }

        // POST: carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cart_id,quantity,customer_id,product_id")] cart cart)
        {
            if (ModelState.IsValid)
            {
                db.carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name", cart.customer_id);
            ViewBag.product_id = new SelectList(db.products, "product_id", "SKU", cart.product_id);
            return View(cart);
        }

        // GET: carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name", cart.customer_id);
            ViewBag.product_id = new SelectList(db.products, "product_id", "SKU", cart.product_id);
            return View(cart);
        }

        // POST: carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cart_id,quantity,customer_id,product_id")] cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name", cart.customer_id);
            ViewBag.product_id = new SelectList(db.products, "product_id", "SKU", cart.product_id);
            return View(cart);
        }

        // GET: carts/Delete/5
        public ActionResult Delete(int? id,int customer_id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            db.carts.Remove(cart);
            db.SaveChanges();
            var carts = db.carts.Where(c => c.customer_id == customer_id).ToList();

            ViewBag.total = db.carts.Where(c => c.customer_id == customer_id).Sum(c => c.product.price*c.quantity);
            return View("Index", carts);
        }

        // POST: carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cart cart = db.carts.Find(id);
            db.carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
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
