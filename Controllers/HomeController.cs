using PTUDTMDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTUDTMDT.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(customer customer)
        {
            if (ModelState.IsValid)
            {
                db.customers.Add(customer);
            db.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            return View();
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
                return RedirectToAction("Index", "products");
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
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}