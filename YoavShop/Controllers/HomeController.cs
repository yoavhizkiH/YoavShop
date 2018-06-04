using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using YoavShop.BL;
using YoavShop.DAL;
using YoavShop.Models;
using YoavShop.ViewModels;

namespace YoavShop.Controllers
{
    public class HomeController : Controller
    {
        private YoavShopContext db = new YoavShopContext();
        private StatisticsManager StatisticsManager  = new StatisticsManager();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.SupplierUserNameSortParm = sortOrder == "SupplierUserName" ? "SupplierUserName_desc" : "SupplierUserName";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.CategorieNameSortParm = sortOrder == "CategorieName" ? "CategorieName_desc" : "CategorieName";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var products = from product in db.Products select product;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(product => product.Name.Contains(searchString)
                || product.Supplier.UserName.Contains(searchString) ||
                                                     product.ProductCategorie.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "SupplierUserName":
                    products = products.OrderBy(s => s.Supplier.UserName);
                    break;
                case "SupplierName_desc":
                    products = products.OrderByDescending(s => s.Supplier.UserName);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "Price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                case "CategorieName":
                    products = products.OrderBy(s => s.ProductCategorie.Name);
                    break;
                case "CategorieName_desc":
                    products = products.OrderByDescending(s => s.ProductCategorie.Name);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult SuppliersByTransactions()
        {
            return Json(
                StatisticsManager.GetSuppliersByMoney(db.Suppliers.ToList(), db.Transactions.Include(t => t.Product).ToList())
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransactionsByTime()
        {
            return Json(
                StatisticsManager.TransactionsByTime(db.Transactions.ToList())
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Buy(int productId)
        {
            if (GlobalVariables.Role != "Customer")
            {
                ViewBag.Message = "Please log in with a customer account";
                return RedirectToAction("Login", "Home");
            }

            var product = db.Products.Single(p => p.Id == productId);

            if (product.Amount < 1)
            {
                ViewBag.Message = "Item is not available";
                return RedirectToAction("Index", "Home");
            }

            var newTransaction = new Transaction
            {
                ProductId = productId,
                CustomerId = GlobalVariables.StoreUser.Id,
                MoneyPaid = product.Price,
                TimeStamp = DateTime.Now
            };
            product.Amount--;
            db.Transactions.Add(newTransaction);
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
                GlobalVariables.StoreUser = new UserInfo{UserName = "admin", Password = "admin"};
                return RedirectToAction("Index", "Home");
            }

            Customer CustomerResult =
                db.Customers.SingleOrDefault(c => c.UserName == LoginVar.UserName && c.Password == LoginVar.Password);

            if (CustomerResult != null)
            {
                GlobalVariables.Role = "Customer";
                GlobalVariables.StoreUser = CustomerResult;
                return RedirectToAction("Index", "Home");
            }

            Supplier SupplierResult =
                db.Suppliers.SingleOrDefault(c => c.UserName == LoginVar.UserName && c.Password == LoginVar.Password);
            if (SupplierResult != null)
            {
                GlobalVariables.Role = "Supplier";
                GlobalVariables.StoreUser = SupplierResult;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "UserName and Password are incorrect";
            return View();
        }

        public ActionResult LogOff()
        {
            GlobalVariables.Initialize();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}