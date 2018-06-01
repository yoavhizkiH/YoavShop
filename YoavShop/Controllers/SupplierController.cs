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
    public class SupplierController : Controller
    {
        private YoavShopContext db = new YoavShopContext();

        // GET: Suppliers
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = string.IsNullOrEmpty(sortOrder) ? "FirstName_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewBag.UserNameSortParm = sortOrder == "UserName" ? "UserName_desc" : "UserName";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var suppliers = from supplier in db.Suppliers select supplier;

            if (!string.IsNullOrEmpty(searchString))
            {
                suppliers = suppliers.Where(supplier => supplier.LastName.Contains(searchString)
                || supplier.FirstName.Contains(searchString) || supplier.UserName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "FirstName_desc":
                    suppliers = suppliers.OrderByDescending(s => s.FirstName);
                    break;
                case "LastName":
                    suppliers = suppliers.OrderBy(s => s.LastName);
                    break;
                case "LastName_desc":
                    suppliers = suppliers.OrderByDescending(s => s.LastName);
                    break;
                case "UserName":
                    suppliers = suppliers.OrderBy(s => s.UserName);
                    break;
                case "UserName_desc":
                    suppliers = suppliers.OrderByDescending(s => s.UserName);
                    break;
                default:
                    suppliers = suppliers.OrderBy(s => s.FirstName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(suppliers.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            Supplier supplier;
            supplier = db.Suppliers.Find(id ?? GlobalVariables.StoreUser.Id);

            if (supplier == null)
            {
                return HttpNotFound();
            }
            supplier.Sellings = db.Transactions.Where(t => t.Product.SupplierId == supplier.Id).ToList();

            return View(supplier);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,CardNumber,ExiprationYear,ExiprationMounth")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Details");
            }

            return View(supplier);
        }

        // GET: Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,CardNumber,ExiprationYear,ExiprationMounth")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(supplier);
        }

        // GET: Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
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
