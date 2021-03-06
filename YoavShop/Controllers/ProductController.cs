﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using YoavShop.BL;
using System.Web.Mvc;
using PagedList;
using YoavShop.DAL;
using YoavShop.ExternalFeauters;
using YoavShop.Models;
using YoavShop.ViewModels;

namespace YoavShop.Controllers
{
    public class ProductController : Controller
    {
        private YoavShopContext db = new YoavShopContext();
        private TweetsFactory tweetsFactory = new TweetsFactory();

        // GET: Product
        public ActionResult Index(string sortOrder, [Bind(Include = "currentNameFilter")]string currentNameFilter, string currentSupplierFilter,
            string searchName, string searchSupplierName, string searchCategorie, string currentCategorieFilter, bool? active, bool? notActive, int? page)
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

            var sproducts = new List<Product>();

            if (active != null && ViewBag.isActive)
            {
                sproducts.AddRange(db.Products.Where(p => p.IsActive).ToList());
            }
            if (ViewBag.isNotActive != null && ViewBag.isNotActive)
            {
                sproducts.AddRange(db.Products.Where(p => !p.IsActive).ToList());
            }

            var products = sproducts.AsQueryable();

            if (!string.IsNullOrEmpty(searchName) || !string.IsNullOrEmpty(searchSupplierName) || !string.IsNullOrEmpty(searchCategorie))
            {
                products = string.IsNullOrEmpty(searchName) ? products : products.Where(product => product.Name.Contains(searchName));
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

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create(int SupplierId)
        {
            var productViewModel = new ProductViewModel
            {
                Product = new Product(){SupplierId = SupplierId},
                ProductCategories = db.ProductCategories.ToList()
            };
            return View(productViewModel);
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,SupplierId,Description,Price,Amount,Color,ProductCategorieId")]Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength <= 0)
                    throw new Exception("Error while uploading");

                db.Products.Add(product);
                db.SaveChanges();

                var fileName = $"Photo{product.Id}";
                var extension = Path.GetExtension(file.FileName);
                var fullName = fileName + extension;
                var physicalPath = Server.MapPath("~/Images/" + fullName);
                file.SaveAs(physicalPath);
                
                product.ProductCategorie = db.ProductCategories.Single(pc => pc.Id == product.ProductCategorieId);
                product.Supplier = db.Suppliers.Single(supplier => supplier.Id == product.SupplierId);
                tweetsFactory.Create(product);
                return RedirectToAction("Details", "Supplier");
            }

            var productViewModel = new ProductViewModel
            {
                Product = product,
                ProductCategories = db.ProductCategories.ToList()
            };
            return View(productViewModel);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.AsNoTracking().Single(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var productViewModel = new ProductViewModel
            {
                Product = product,
                ProductCategories = db.ProductCategories.ToList()
            };
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,SupplierId,Description,Price,Amount,Color,ProductCategorieId,IsActive")]Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid) return View(product);

            if (file != null)
            {
                if (file.ContentLength <= 0)
                    throw new Exception("Error while uploading");

                var filePath = Server.MapPath($"~/Images/Photo{product.Id}.jpg");

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                file.SaveAs(filePath);
            }

            var oldProduct = db.Products.AsNoTracking().Single(p => p.Id == product.Id);

            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            tweetsFactory.Edit(product, oldProduct);
            return RedirectToAction("Index", "Home");
        }

        
        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        
        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (!db.Transactions.Any(t => t.ProductId == product.Id))
            {
                var filePath = Server.MapPath($"~/Images/Photo{product.Id}.jpg");

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                db.Products.Remove(product);
            }
            else
            {
                product.IsActive = false;
                db.Entry(product).State = EntityState.Modified;
            }

            db.SaveChanges();

            return RedirectToAction("Details", "Supplier");
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
