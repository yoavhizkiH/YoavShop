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
    public class ProductCategorieController : Controller
    {
        private YoavShopContext db = new YoavShopContext();

        // GET: ProductCategorie
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var categories = from categorie in db.ProductCategories select categorie;

            if (!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(categorie => categorie.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    categories = categories.OrderByDescending(s => s.Name);
                    break;
                default:
                    categories = categories.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(categories.ToPagedList(pageNumber, pageSize));
        }

        // GET: ProductCategorie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategorie productCategorie = db.ProductCategories.Find(id);
            if (productCategorie == null)
            {
                return HttpNotFound();
            }
            return View(productCategorie);
        }

        // GET: ProductCategorie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductCategorie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ProductCategorie productCategorie)
        {
            if (ModelState.IsValid)
            {
                db.ProductCategories.Add(productCategorie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCategorie);
        }

        // GET: ProductCategorie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategorie productCategorie = db.ProductCategories.Find(id);
            if (productCategorie == null)
            {
                return HttpNotFound();
            }
            return View(productCategorie);
        }

        // POST: ProductCategorie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ProductCategorie productCategorie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productCategorie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCategorie);
        }

        // GET: ProductCategorie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategorie productCategorie = db.ProductCategories.Find(id);
            if (productCategorie == null)
            {
                return HttpNotFound();
            }
            return View(productCategorie);
        }

        // POST: ProductCategorie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategorie productCategorie = db.ProductCategories.Find(id);
            db.ProductCategories.Remove(productCategorie);
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
