using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using YoavShop.DAL;
using YoavShop.Models;

namespace YoavShop.Controllers
{
    public class TransactionController : Controller
    {
        private YoavShopContext db = new YoavShopContext();

        // GET: Transaction
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ProductNameSortParm = string.IsNullOrEmpty(sortOrder) ? "ProductName_desc" : "";
            ViewBag.SupplierUserNameSortParm = sortOrder == "SupplierUserName" ? "SupplierUserName_desc" : "SupplierUserName";
            ViewBag.CustomerUserNameSortParm = sortOrder == "CustomerUserName" ? "CustomerUserName_desc" : "CustomerUserName";
            ViewBag.MoneyPaidSortParm = sortOrder == "MoneyPaid" ? "MoneyPaid_desc" : "MoneyPaid";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Datedesc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var transactions = from transaction in db.Transactions select transaction;

            if (!string.IsNullOrEmpty(searchString))
            {
                transactions = transactions.Where(transaction => transaction.Product.Name.Contains(searchString)
                || transaction.Product.Supplier.UserName.Contains(searchString) 
                || transaction.Customer.UserName.Contains(searchString) 
                || transaction.TimeStamp.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ProductName_desc":
                    transactions = transactions.OrderByDescending(s => s.Product.Name);
                    break;
                case "SupplierUserName":
                    transactions = transactions.OrderBy(s => s.Product.Supplier.UserName);
                    break;
                case "SupplierUserName_desc":
                    transactions = transactions.OrderByDescending(s => s.Product.Supplier.UserName);
                    break;
                case "CustomerUserName":
                    transactions = transactions.OrderBy(s => s.Customer.UserName);
                    break;
                case "CustomerUserName_desc":
                    transactions = transactions.OrderByDescending(s => s.Customer.UserName);
                    break;
                case "MoneyPaid":
                    transactions = transactions.OrderBy(s => s.MoneyPaid);
                    break;
                case "MoneyPaid_desc":
                    transactions = transactions.OrderByDescending(s => s.MoneyPaid);
                    break;
                case "Date":
                    transactions = transactions.OrderBy(s => s.TimeStamp);
                    break;
                case "Date_desc":
                    transactions = transactions.OrderByDescending(s => s.TimeStamp);
                    break;
                default:
                    transactions = transactions.OrderBy(s => s.Product.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(transactions.ToPagedList(pageNumber, pageSize));
        }

        // GET: Transaction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "UserName");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,CustomerId,MoneyPaid,TimeStamp")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "UserName", transaction.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", transaction.ProductId);
            return View(transaction);
        }

        // GET: Transaction/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "UserName", transaction.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", transaction.ProductId);
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,CustomerId,MoneyPaid,TimeStamp")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "UserName", transaction.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", transaction.ProductId);
            return View(transaction);
        }

        // GET: Transaction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
