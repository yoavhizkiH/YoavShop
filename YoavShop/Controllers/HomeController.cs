using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoavShop.DAL;
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}