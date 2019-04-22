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
    public class TripsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //Trips listed by current passenger

        [HttpGet]
       // [Authorize(Roles = "Passenger")]
        public async Task<ActionResult> Index()
        {
            var user = User.Identity.GetUserId();
            var trips = db.Trips
                .Where(s => s.PassengerIdentity.Equals(user) && s.DriverIdentity == null)
                .OrderByDescending(t => t.TimeStamp);

            var passenger = db.Passengers
                .Where(s => s.PassengerIdentity.Equals(user))
                .ToList();

            var isOnLine = passenger[0].OnLine;

            ViewBag.Open = isOnLine;

            return View(await trips.ToListAsync());
        }


        // Current trips on route

        [HttpGet]
       // [Authorize(Roles = "Passenger")]
        public async Task<ActionResult> OnDelivery()
        {
            var user = User.Identity.GetUserId();

            var ondelivery = db.Trips
                .Where(s => s.DriverIdentity.Length > 1 && s.PassengerIdentity.Equals(user) && s.IsCompleted == false)
                .OrderByDescending(t => t.TimeStamp);

            return View(await ondelivery.ToListAsync());
        }


        // Current trips completed

        [HttpGet]
        [Authorize(Roles = "Passenger")]
        public async Task<ActionResult> Completed()
        {
            var user = User.Identity.GetUserId();

            var completed = db.Trips.Where(s => s.IsCompleted.Equals(true) && s.PassengerIdentity.Equals(user));

            return View(await completed.ToListAsync());
        }


        //Trips listed from all Passengers

        //[HttpGet]
        //[Authorize(Roles = "Passenger")]
        ////public ActionResult AllTrips()
        //{
        //    var user = User.Identity.GetUserId();
        //    var trips = db.Trips.Where(s => s.PassengerIdentity.Equals(user));

        //    if (trips.Count() == 0)
        //    {
        //        ViewBag.Message = "There are no Trips";
        //    }
        //    else
        //    {
        //        ViewBag.TripsCount = trips.Count();
        //        ViewBag.TotalValue = trips.Sum(s => s.Total);
        //        ViewBag.TotalCommision = trips.Sum(s => s.Commission);

        //    }

        //    return View();
        //}


        /// GET: Create trip

        [HttpGet]
       // [Authorize(Roles = "Passenger")]
        public ActionResult Create()
        {
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverIdentity", "Name");
            ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerIdentity", "Name");
            return View();
        }


        // POST: Create new trip
        [HttpPost]
       // [Authorize(Roles = "Passenger")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TripId,TimeStamp,Total,Commission,OriginAddress,DestAddress,DriverIdentity,PassengerIdentity")] Trip trip)
                              
          {
            trip.PassengerIdentity = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Trips.Add(trip);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DriverId = new SelectList(db.Drivers, "DriverIdentity", "Name", trip.Driver.DriverIdentity);
            ViewBag.ShopId = new SelectList(db.Trips, "PassengerIdentity", "Name", trip.Passenger.PassengerIdentity);
            return View(trip);
        }


        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = await db.Trips.FindAsync(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverIdentity", "Name", trip.Driver.DriverIdentity);
            ViewBag.PassengerId = new SelectList(db.Passengers, "ShopIdentity", "Name", trip.Passenger.PassengerIdentity);
            return View(trip);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TripId,TimeStamp,Total,Commission,OriginAddress,DestAddress,IsCompleted,DriverIdentity,PassengerIdentity")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trip).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DriverId = new SelectList(db.Drivers, "DriverIdentity", "Name", trip.DriverIdentity);
            ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerId", "Name", trip.PassengerIdentity);
            return View(trip);
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = await db.Trips.FindAsync(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = await db.Trips.FindAsync(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Trip trip = await db.Trips.FindAsync(id);
            db.Trips.Remove(trip);
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








    
