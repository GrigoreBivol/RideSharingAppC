using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RideSharing.Models;


namespace TestUnit
{
    [TestClass]
    public class UnitTest1
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [TestMethod]
        public void AddTrip()
        {

            var trip = new Trip()
            {
                Commission = 10,
                DestAddress = "sampleHereDest",
                DriverIdentity = "sampleHereDriverIdentity",
                OriginAddress = "sampleHereOriginAddress",
                PassengerIdentity = "sampleHerePassengerIdentity",



            };
            db.Trips.Add(trip);
            db.SaveChanges();

            Assert.AreEqual(10, trip.Commission);
            Assert.AreEqual("sampleHereDest", trip.DestAddress);
            Assert.AreEqual("sampleHereDriverIdentity", trip.DriverIdentity);
            Assert.AreEqual("sampleHereOriginAddress", trip.OriginAddress);
            Assert.AreEqual("sampleHerePassengerIdentity", trip.PassengerIdentity);
            //  Assert.()




        }
    }
}
