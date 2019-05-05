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
    [Authorize]
    public class DriversController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Drivers
        // All Rides listed from all users
        [HttpGet]
        [Authorize(Roles ="Driver")]
        public async Task<ActionResult> AllTrips()
        {
            var trips = db.Trips.Where(s => s.Driver.DriverIdentity == null);

            var user = User.Identity.GetUserId();
            var driver = db.Drivers
                .Where(s => s.DriverIdentity.Equals(user))
                .ToList();

        


            return View(await trips.ToListAsync());
        }

        //Orders on route to customers 
        [HttpGet]
        [Authorize(Roles = "Driver")]
        public async Task<ActionResult> OnDelivery()
        {
            var user = User.Identity.GetUserId();
            var ondelivery = db.Trips.Where(s => s.Driver.DriverIdentity.Length > 1 && s.Driver.DriverIdentity.Equals(user) && s.Status == TripStatus.Active);

            return View(await ondelivery.ToListAsync());
        }


        //Delivered orders for current driver
        [HttpGet]
        [Authorize(Roles = "Driver")]
        public async Task<ActionResult> Completed()
        {
            var user = User.Identity.GetUserId();
            // get all completed trips
            var completed = db.Trips.Where(s => s.Status.Equals(TripStatus.Completed) && s.Driver.DriverIdentity.Equals(user));

            return View(await completed.ToListAsync());
        }

        //Get statistics for driver
        [HttpGet]
        [Authorize(Roles = "Driver")]
        public ActionResult Statistics()
        {
            var user = User.Identity.GetUserId();
            var trips = db.Trips.Where(s => s.Driver.DriverIdentity.Equals(user));

            if (trips.Count() == 0)
            {
                ViewBag.Message = "There are no Orders";
            }
            else
            {
                ViewBag.TripsCount = trips.Count();
                ViewBag.TotalCommision = trips.Sum(s => s.Commission);
            }

            return View();
        }

        //Put the driver/car owner in OFF-line mode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DriverOffline([Bind(Include = "DriverId,DriverIdentity,Name,OnLine,OnRide")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.GetUserId();
                var medriver = db.Drivers.Where(s => s.DriverIdentity.Equals(user)).ToList();
                var meid = medriver[0].DriverId;
                var name = medriver[0].Name;

                var currentdriver = db.Drivers.Find(meid);

                currentdriver.DriverIdentity = user;
                currentdriver.Name = name;
              

                db.Entry(currentdriver).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("AllTrips", "Drivers");
            }

            return RedirectToAction("AllTrips", "Drivers");
        }

        //Set the driver in On-Line Mode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DriverOnline([Bind(Include = "DriverId,DriverIdentity,Name,OnLine,OnRide")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.GetUserId();
                var medriver = db.Drivers.Where(s => s.DriverIdentity.Equals(user)).ToList();
                var meid = medriver[0].DriverId;
                var name = medriver[0].Name;

                var currentdriver = db.Drivers.Find(meid);

                currentdriver.DriverIdentity = user;
                currentdriver.Name = name;
           

                db.Entry(currentdriver).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("AllTrips", "Drivers");
            }

            return RedirectToAction("AllTrips", "Drivers");
        }

        //Trips taken by driver 
        [Authorize(Roles = "Driver")]
        public async Task<ActionResult> AddDriver(int? id, Trip trip)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currenttrip = await db.Trips.FindAsync(id);
            if (currenttrip == null)
            {
                return HttpNotFound();
            }

            var user = User.Identity.GetUserId();
            currenttrip.Driver.DriverIdentity = user;

            if (ModelState.IsValid)
            {
                db.Entry(currenttrip).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("OnRide", "Drivers");
            }

            return RedirectToAction("OnRide", "Drivers");
        }

        //Trip Completed
        [Authorize(Roles = "Driver")]
        public async Task<ActionResult> OrderDelivered(int? id, Trip trip)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currenttrip = await db.Trips.FindAsync(id);
            if (currenttrip == null)
            {
                return HttpNotFound();
            }

            var user = User.Identity.GetUserId();
            //order.DriverIdentity = user;
            currenttrip.Status = TripStatus.Completed;

            if (ModelState.IsValid)
            {
                db.Entry(currenttrip).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Completed", "Drivers");
            }

            return RedirectToAction("Completed", "Drivers");
        }

         
        // GET: All Drivers
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Drivers.ToListAsync());
        }

        // GET: Drivers/Details/5
        [Authorize(Roles = "Admin")]
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


        // GET: Drivers/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DriverId,Name,OnLine,OnRide")] Driver driver)
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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




