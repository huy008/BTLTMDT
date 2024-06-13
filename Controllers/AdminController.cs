using PTUDTMDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTUDTMDT.Controllers
{
    public class AdminController : Controller
    {
        private DataContext db = new DataContext();
        // GET: Admin
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(customer customer)
        {
            db.customers.Add(customer);
            db.SaveChanges();
            return RedirectToAction("DangNhap");
        }
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(customer customer)
        {
            var firstname = customer.first_name;
            var password = customer.password;
            var check = db.customers.SingleOrDefault(x => x.first_name.Equals(firstname)  && x.password.Equals(password));
            if (check != null)
            {
                Session["customer"] =check;
                return RedirectToAction("DanhSach", "products");
            }
            else
            {
                ViewBag.error = "Tai khoản mật khẩu không chính xác";
                return View("DangNhap");
            }

        }

        public ActionResult DangXuat()
        {
            Session["customer"] =null;
            return RedirectToAction("DangNhap");
        }

        public ActionResult ThongKe()
        {
            var monTH = db.order_
                .Where(o => ((DateTime)o.order_date).Month == DateTime.Now.Month)
               .GroupBy(o => new { ((DateTime)o.order_date).Month })
               .Select(g => new
               {
                   Month = g.Key.Month,
                   OrderCount = g.Count(),
                   TotalRevenue = g.Sum(o => o.order_price)
               })
               .OrderBy(stat => stat.Month)
               .FirstOrDefault();

            var daY = db.order_
               .Where(o => ((DateTime)o.order_date).Day == DateTime.Now.Day)
              .GroupBy(o => new { ((DateTime)o.order_date).Day })
              .Select(g => new
              {
                  Day = g.Key.Day,
                  OrderCount = g.Count(),
                  TotalRevenue = g.Sum(o => o.order_price)
              })
              .OrderBy(stat => stat.Day)
              .FirstOrDefault();

            var yeaR = db.order_
              .Where(o => ((DateTime)o.order_date).Year == DateTime.Now.Year)
             .GroupBy(o => new { ((DateTime)o.order_date).Year })
             .Select(g => new
             {
                 Year = g.Key.Year,
                 OrderCount = g.Count(),
                 TotalRevenue = g.Sum(o => o.order_price)
             })
             .OrderBy(stat => stat.Year)
             .FirstOrDefault();

            ViewBag.monTHTotalRevenue =monTH.TotalRevenue;
            ViewBag.monTHOrderCount =monTH.OrderCount;
            ViewBag.monTH =monTH.Month;

            if (daY!=null)
            {
                ViewBag.daYTotalRevenue =daY.TotalRevenue;
                ViewBag.daYOrderCount =daY.OrderCount;
                ViewBag.daY =daY.Day;
            }
            else
            {
                ViewBag.daYTotalRevenue=0;
                ViewBag.daYOrderCount=0;
                ViewBag.daY = DateTime.Now.Day;
            }

            ViewBag.yeaRTotalRevenue =yeaR.TotalRevenue;
            ViewBag.yeaROrderCount =yeaR.OrderCount;
            ViewBag.yeaR =yeaR.Year;


            var shipment = db.shipments.ToList();



            var topProducts = (from p in db.products
                               join oi in db.order_item on p.product_id equals oi.product_id
                               group new { product = p, quantity = oi.quantity } by p.product_id into productGroup
                               orderby productGroup.Sum(x => x.quantity) descending
                               select new
                               {
                                   SKU = productGroup.FirstOrDefault().product.SKU,
                                   product_image = productGroup.FirstOrDefault().product.product_image,
                                   price = productGroup.FirstOrDefault().product.price,
                                   description = productGroup.FirstOrDefault().product.description,
                               })
                             .ToList();

            ViewBag.topProductsTop3 = topProducts.Select(p => new product
            {
                SKU = p.SKU,
                product_image = p.product_image,
                price = p.price,
                description =p.description,
            }).Take(3).ToList();

            ViewBag.topProductsTop5 = topProducts.Select(p => new product
            {
                SKU = p.SKU,
                product_image = p.product_image,
                price = p.price,
                description =p.description,
            }).Take(5).ToList();

            var notProduct = db.products
                    .Where(p => !db.order_item.Any(oi => oi.product_id == p.product_id))
                .Select(p => new
                {
                    SKU = p.SKU,
                    product_image = p.product_image,
                    price = p.price,
                    description = p.description,
                })
                .ToList();

            ViewBag.NotSellingTop3 = notProduct.Select(p => new product
            {
                SKU = p.SKU,
                product_image = p.product_image,
                price = p.price,
                description =p.description,
            }).Take(3).ToList();

            ViewBag.NotSellingTop5 = notProduct.Select(p => new product
            {
                SKU = p.SKU,
                product_image = p.product_image,
                price = p.price,
                description =p.description,
            }).Take(5).ToList();


            return View(shipment);
        }

        [HttpPost]
        public JsonResult updateStatus(int id, int value)
        {
            var shipment = db.shipments.FirstOrDefault(s => s.shipment_id == id);
            if (shipment != null)
            {
                shipment.status = value.ToString();
                db.SaveChanges();
            }
            return Json(new { success = true, message = "Cập nhật trạng thái thành công." });
        }
    }
}