using System;
using System.Collections.Generic;
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
    public class MapLocationController : Controller
    {
        private YoavShopContext db = new YoavShopContext();

        // GET: MapLocation
        public ActionResult Index()
        {
            return View(db.MapLocations.ToList());
        }

        // GET: MapLocation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapLocation mapLocation = db.MapLocations.Find(id);
            if (mapLocation == null)
            {
                return HttpNotFound();
            }
            return View(mapLocation);
        }

        // GET: MapLocation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MapLocation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PlaceName,GeoLong,GeoLat,Info")] MapLocation mapLocation)
        {
            if (ModelState.IsValid)
            {
                db.MapLocations.Add(mapLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mapLocation);
        }

        // GET: MapLocation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapLocation mapLocation = db.MapLocations.Find(id);
            if (mapLocation == null)
            {
                return HttpNotFound();
            }
            return View(mapLocation);
        }

        // POST: MapLocation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PlaceName,GeoLong,GeoLat,Info")] MapLocation mapLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mapLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mapLocation);
        }

        // GET: MapLocation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapLocation mapLocation = db.MapLocations.Find(id);
            if (mapLocation == null)
            {
                return HttpNotFound();
            }
            return View(mapLocation);
        }

        // POST: MapLocation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MapLocation mapLocation = db.MapLocations.Find(id);
            db.MapLocations.Remove(mapLocation);
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
