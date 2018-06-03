using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
        private ProductSearch productSearch = new ProductSearch();
        private YoavShopContext db = new YoavShopContext();
        private TweetsFactory tweetsFactory = new TweetsFactory();

        // GET: Product

        public ActionResult Index(string searchParams, int? PageNumber, [Bind(Include = "sortOrder")]string sortOrder, [Bind(Include = "currentFilter")] string currentFilter, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.SupplierUserNameSortParm = sortOrder == "SupplierUserName" ? "SupplierUserName_desc" : "SupplierUserName";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.CategorieNameSortParm = sortOrder == "CategorieName" ? "CategorieName_desc" : "CategorieName";
            var pspViewModel = new PagedSearchProductsViewModel{ProductSearchModel = new ProductSearchModel()};
            
            IEnumerable<Product> products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams))//pspViewModel.ProductSearchModel != null && pspViewModel.ProductSearchModel.IsSearched())
            {
                pspViewModel.ProductSearchModel = new ProductSearchModel(searchParams);
                products = productSearch.GetProducts(pspViewModel.ProductSearchModel);
                ViewBag.CurrentProducts = products;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            
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
            int pageNumber = (PageNumber ?? 1);
            pspViewModel.PagedList = products.ToPagedList(pageNumber, pageSize);
            return View(pspViewModel);
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
        public ActionResult Create()
        {
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "UserName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Price,Amount,ProductColor,ProductCategorieId,SupplierId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                product.ProductCategorie = db.ProductCategories.Single(pc => pc.Id == product.ProductCategorieId);
                product.Supplier = db.Suppliers.Single(supplier => supplier.Id == product.SupplierId);
                tweetsFactory.Create(product);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "UserName", product.SupplierId);
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
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
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Product = product;
            productViewModel.ProductCategories = db.ProductCategories.ToList();
            return View(productViewModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Description,Price,Amount,ProductColor,ProductCategorieId,SupplierId,ProductCategorie,Supplier")] Product product)
        {
            if (ModelState.IsValid)
            {
                var oldProduct = db.Products.Single(p => p.Id == product.Id);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                product.ProductCategorie = db.ProductCategories.Single(pc => pc.Id == product.ProductCategorieId);
                product.Supplier = db.Suppliers.Single(supplier => supplier.Id == product.SupplierId);
                tweetsFactory.Edit(product, oldProduct);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "UserName", product.SupplierId);
            return View(product);
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
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
