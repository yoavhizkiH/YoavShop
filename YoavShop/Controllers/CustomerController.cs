using System;
using PagedList;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YoavShop.DAL;
using YoavShop.Models;

namespace YoavShop.Controllers
{
    public class CustomerController : Controller
    {
        private YoavShopContext db = new YoavShopContext();

        // GET: Customers
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
            var customers = from customer in db.Customers select customer;

            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(customer => customer.LastName.Contains(searchString)
                || customer.FirstName.Contains(searchString) || customer.UserName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "FirstName_desc":
                    customers = customers.OrderByDescending(s => s.FirstName);
                    break;
                case "LastName":
                    customers = customers.OrderBy(s => s.LastName);
                    break;
                case "LastName_desc":
                    customers = customers.OrderByDescending(s => s.LastName);
                    break;
                case "UserName":
                    customers = customers.OrderBy(s => s.UserName);
                    break;
                case "UserName_desc":
                    customers = customers.OrderByDescending(s => s.UserName);
                    break;
                default:
                    customers = customers.OrderBy(s => s.FirstName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(customers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            //ViewBag.Id = new SelectList(db.s, "Id", "UserName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,UserName,Password,CardNumber,ExiprationYear,ExiprationMounth")]Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //db.CreditCards.Add(creditCard);
                    //customer.CreditCard = creditCard;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", $"Unable to save changes. Try again, and if the problem persists see your system administrator. {dex.Message}");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,UserName,Password,CardNumber,ExiprationYear,ExiprationMounth")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(creditCard).State = EntityState.Modified;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
