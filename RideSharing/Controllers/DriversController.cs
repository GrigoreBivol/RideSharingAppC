using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RideSharing.Models;
using Microsoft.AspNet.Identity;

namespace RideSharing.Controllers
{
    public class DriversController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Drivers
        // All Rides listed
        [HttpGet]
        [Authorize(Roles ="Driver")]
        public async Task<ActionResult> AllTrips()
        {
            var rides = db.Trips.Where(s => s.Driver.DriverIdentity == null);

            var user = User.Identity.GetUserId();
            var driver = db.Drivers
                .Where(s => s.DriverIdentity.Equals(user))
                .ToList();

            var isOnline = driver[0].Online;
            ViewBag.Online = isOnline;


            return View(await rides.ToListAsync());
        }

        // GET: Drivers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // GET: Drivers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DriverId,DriverIdentity,Name,Online,OnRide")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                db.Drivers.Add(driver);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(driver);
        }

        // GET: Drivers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DriverId,DriverIdentity,Name,Online,OnRide")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(driver).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Driver driver = await db.Drivers.FindAsync(id);
            db.Drivers.Remove(driver);
            await db.SaveChangesAsync();
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
