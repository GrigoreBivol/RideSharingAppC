using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RideSharing.Models
{
    public class TripReview
    {

        [Key]
        public int TripReviewId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PassengerReview { get; set; }
        public string DriverReview { get; set; }
        public virtual Trip Trip { get; set; }
        //public virtual Passenger Passenger { get; set; }


        public TripReview()
        {
            TimeStamp = DateTime.Now;

        }
    }
}