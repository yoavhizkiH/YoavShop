using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoavShop.DAL;
using YoavShop.Models;
using YoavShop.ViewModels;

namespace YoavShop.Controllers
{
    public class HomeController : Controller
    {
        private YoavShopContext db = new YoavShopContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            IQueryable<TransactionsDateGroup> data = from transaction in db.Transactions
                group transaction by transaction.TimeStamp into dateGroup
                select new TransactionsDateGroup()
                {
                    TransactionDate = dateGroup.Key,
                    TransactionsCount = dateGroup.Count()
                };
            return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserInfo LoginVar)
        {
            if (LoginVar.UserName == "admin" && LoginVar.Password == "admin")
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            Customer CustomerResult = null;
            Supplier SupplierResult = null;

            CustomerResult =
                db.Customers.SingleOrDefault(c => c.UserName == LoginVar.UserName && c.Password == LoginVar.Password);
            SupplierResult =
                db.Suppliers.SingleOrDefault(c => c.UserName == LoginVar.UserName && c.Password == LoginVar.Password);

            if (CustomerResult != null)
            {
                return RedirectToAction("Index", "Product", new { area = "Customer" });
            }
            if (SupplierResult != null)
            {
                return RedirectToAction("Index", "Supplier", new { area = "Supplier" });
            }

            ViewBag.Message = string.Format("UserName and Password is incorrect");
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}