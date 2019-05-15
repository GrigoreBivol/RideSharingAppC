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
using System.Net.Mail;

namespace RideSharing.Controllers
{
    [Authorize]
    public class TripsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //Trips listed by current passenger
        [HttpGet]
       // [Authorize(Roles = "Passenger")]
        public async Task<ActionResult> Index()
        {
            // set role of user to ViewData to use it in the views
            ViewData["Role"] = Config.Role;
            // get the current user name 
            var user = User.Identity.Name;
            // new querybale list of trips
            //IQueryable<Trip> trips=null;
            List<TripModelView> tripsReviews = null;
            // check if the current user connect ass passenger 
            List<Trip> trips=null;
            // if the user is a passenger get all trips created by the user
            if (Config.Role == "Passenger")
            {
                   trips = db.Trips
                .Where(s => s.PassengerIdentity.Equals(user) )
                .OrderByDescending(t => t.TimeStamp).ToList();

            }
            else
            {  trips = db.Trips.Where(c=>c.Status==TripStatus.Active
            || (c.Status==TripStatus.Accepted && c.DriverIdentity== user) 
            || (c.Status == TripStatus.Completed && c.DriverIdentity == user)).ToList() ;
            }

            return View(GetReviewList(trips));
        }


        public List<TripModelView> GetReviewList(List<Trip> trips )
        {
            // set role of user to ViewData to use it in the views
            ViewData["Role"] = Config.Role;
            // get the current user name 
            var user = User.Identity.Name;
            // new querybale list of trips
            //IQueryable<Trip> trips=null;
            List<TripModelView> tripsReviews = null;
            // check if the current user connect ass passenger 
            if (Config.Role == "Passenger")
            {
                // if the user is a passenger get all trips created by the user

                var   tripsReview = db.TripReviews.ToList();
                tripsReviews = new List<TripModelView>();
                foreach (var item in trips)
                {
                    tripsReviews.Add(new TripModelView()
                    {
                        Commission = item.Commission,
                        DestAddress = item.DestAddress,
                        DriverIdentity = item.DriverIdentity,
                        OriginAddress = item.OriginAddress,
                        PassengerIdentity = item.PassengerIdentity,
                        Status = item.Status,
                        TimeStamp = item.TimeStamp,
                        TripId = item.TripId,
                        TripReview = tripsReview.Where(c => c.TripReviewId == item.TripId).FirstOrDefault()
                    });
                }
            }
            else
            {

        
                 var   tripsReview = db.TripReviews.ToList();
                tripsReviews = new List<TripModelView>();
                foreach (var item in trips)
                {
                    tripsReviews.Add(new TripModelView()
                    {
                        Commission = item.Commission,
                        DestAddress = item.DestAddress,
                        DriverIdentity = item.DriverIdentity,
                        OriginAddress = item.OriginAddress,
                        PassengerIdentity = item.PassengerIdentity,
                        Status = item.Status,
                        TimeStamp = item.TimeStamp,
                        TripId = item.TripId,
                        TripReview = tripsReview.Where(c => c.TripReviewId == item.TripId).FirstOrDefault()
                    });
                }

            }
            return tripsReviews;


        }


        public async Task<ActionResult> AcceptTrip(int id)
        {
            // get the curent user name
            var user = User.Identity.Name;
            //get the trips by id
            var trips = db.Trips.Where(s => s.TripId.Equals(id)).FirstOrDefault();
            // update status of trips tp accepted
            trips.Status = TripStatus.Accepted;
            //set the user to driver identity 
            trips.DriverIdentity = user;
            var userReceiver = db.Users.Where(c => c.UserName == trips.PassengerIdentity).FirstOrDefault();
            SendMail(userReceiver.Email, userReceiver.UserName, user, userReceiver.UserName, trips.PassengerIdentity);
            db.SaveChanges();
            return Redirect("/Trips/");
        }
        public ActionResult TripDriverReview(int? TripReviewId, string Comment)
        {
            var tripsReview = db.TripReviews.Where(C => C.TripReviewId == TripReviewId).FirstOrDefault();
            var user = User.Identity.Name;
            tripsReview.DriverIsReview = true;
            tripsReview.DriverUserName = user;
            tripsReview.DriverReview = Comment;
            tripsReview.TimeStampDrivRev = DateTime.Now;
            db.SaveChanges();
            return Redirect("/Trips/");
        }
        public ActionResult TripPassengerReview(int? TripReviewId, string Comment)
        {
            var tripsReview = db.TripReviews.Where(C => C.TripReviewId == TripReviewId).FirstOrDefault();
            var user = User.Identity.Name;
            tripsReview.PassengerIsReview = true;
            tripsReview.PassengerUserName = user;
            tripsReview.PassengerReview = Comment;
            tripsReview.TimeStampPassRev = DateTime.Now;
            db.SaveChanges();
            return Redirect("/Trips/");
        }
         public async Task<ActionResult> CompleteTrip(int id)
         {
            string EmailReceiver = "";
            string UserNameReceiver = "";      
            // get the curent user name
            var user = User.Identity.Name;
            //get the trips by id
            var trips = db.Trips.Where(s => s.TripId.Equals(id)).FirstOrDefault();
            // update status of trips tp compketed
            trips.Status = TripStatus.Completed;
            trips.DriverIdentity = user;
            var userReceiver = db.Users.Where(c => c.UserName == trips.PassengerIdentity).FirstOrDefault();
            var tripReview = new TripReview()
            { 
                PassengerUserName= userReceiver.UserName,
                DriverUserName= user, 
                TripId= trips.TripId
            };
            db.TripReviews.Add(tripReview);
            db.SaveChanges();
            return Redirect("/Trips/");
        }

        public void SendMail(string to , string UserTo, string UserFrom,string driverName ,string PassengerName)
        {
            var client = new SmtpClient(Config.smtpServer, Config.Port)
            {
                Credentials = new NetworkCredential(Config.EmailServer, Config.EmailPass),
                EnableSsl = true
            };
            String Body= "Hello passenger "+ PassengerName  + ".Your trip has been accepted by the driver "+ driverName + " .The driver will contact you soon. Thank you for using RideSharing App.";
            //client.Send(Config.EmailServer, to, to + " " + UserTo + " " + UserFrom, Body);
           client.Send(Config.EmailServer, to, to + "" + UserTo + " " + UserFrom, Body);       



        }
        // Current trips on route

        [HttpGet]
       // [Authorize(Roles = "Passenger")]
        public async Task<ActionResult> OnDelivery()
        {
            var user = User.Identity.GetUserId();

            var ondelivery = db.Trips
                .Where(s => s.DriverIdentity.Length > 1 && s.PassengerIdentity.Equals(user) && s.Status != TripStatus.Completed)
                .OrderByDescending(t => t.TimeStamp);

            return View(await ondelivery.ToListAsync());
        }


        // Current trips completed

        [HttpGet]
     
        public async Task<ActionResult> Completed()
        {
            var user = User.Identity.GetUserId();

            var completed = db.Trips.Where(s => s.Status.Equals(TripStatus.Completed) && s.PassengerIdentity.Equals(user));
       
            return View(GetReviewList(completed.ToList()));
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
            trip.PassengerIdentity = User.Identity.Name;
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
        //[Authorize(Roles = "Admin")]
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
            //ViewBag.DriverId = new SelectList(db.Drivers, "DriverIdentity", "Name", trip.Driver.DriverIdentity);
            //ViewBag.PassengerId = new SelectList(db.Passengers, "ShopIdentity", "Name", trip.Passenger.PassengerIdentity);
            return View(trip);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
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








    
