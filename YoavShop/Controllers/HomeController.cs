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
                GlobalVariables.Role = "Admin";
                return RedirectToAction("Index", "Home");
            }

            Customer CustomerResult =
                db.Customers.SingleOrDefault(c => c.UserName == LoginVar.UserName && c.Password == LoginVar.Password);

            if (CustomerResult != null)
            {
                GlobalVariables.Role = "Customer";
                return RedirectToAction("Index", "Home");
            }

            Supplier SupplierResult =
                db.Suppliers.SingleOrDefault(c => c.UserName == LoginVar.UserName && c.Password == LoginVar.Password);
            if (SupplierResult != null)
            {
                GlobalVariables.Role = "Supplier";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = string.Format("UserName and Password are incorrect");
            return View();
        }

        public ActionResult LogOff()
        {
            GlobalVariables.Role = "";
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}