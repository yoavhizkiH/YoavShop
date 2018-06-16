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

        public ActionResult Index(string sortOrder, [Bind(Include = "currentNameFilter")]string currentNameFilter, string currentSupplierFilter,
            string searchName, string searchSupplierName, string searchCategorie, string currentCategorieFilter, int? page)
        {
            if (searchName != null || searchSupplierName != null || searchCategorie != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentNameFilter;
                searchSupplierName = currentSupplierFilter;
                searchCategorie = currentCategorieFilter;
            }

            ViewBag.CurrentNameFilter = searchName;
            ViewBag.CurrentSupplierFilter = searchSupplierName;
            ViewBag.CurrentCategorieFilter = searchCategorie;
            var products = db.Products.Where(p => p.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(searchName) || !string.IsNullOrEmpty(searchSupplierName) || !string.IsNullOrEmpty(searchCategorie))
            {
                products = string.IsNullOrEmpty(searchName) ? products :  products.Where(product => product.Name.Contains(searchName));
                products = string.IsNullOrEmpty(searchSupplierName) ? products : products.Where(product => product.Supplier.UserName.Contains(searchSupplierName));
                products = string.IsNullOrEmpty(searchCategorie) ? products : products.Where(product => product.ProductCategorie.Name.Contains(searchCategorie));
            }

            products = SortProducts(sortOrder, products);

            return View(products.ToPagedList(page ?? 1, 3));
        }

        public IQueryable<Product> SortProducts(string sortOrder, IQueryable<Product> products)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.SupplierUserNameSortParm = sortOrder == "SupplierUserName" ? "SupplierUserName_desc" : "SupplierUserName";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.CategorieNameSortParm = sortOrder == "CategorieName" ? "CategorieName_desc" : "CategorieName";

            switch (sortOrder)
            {
                case "Name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "SupplierUserName":
                    products = products.OrderBy(s => s.Supplier.UserName);
                    break;
                case "SupplierUserName_desc":
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

            return products;
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult MapLocations()
        {
            return Json(db.MapLocations.ToList() , JsonRequestBehavior.AllowGet);
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
                return RedirectToAction("Details", "Customer");
            }

            Supplier SupplierResult =
                db.Suppliers.SingleOrDefault(c => c.UserName == LoginVar.UserName && c.Password == LoginVar.Password);
            if (SupplierResult != null)
            {
                GlobalVariables.Role = "Supplier";
                GlobalVariables.StoreUser = SupplierResult;
                return RedirectToAction("Details", "Supplier");
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