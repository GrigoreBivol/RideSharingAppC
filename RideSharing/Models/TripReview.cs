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
        public DateTime? TimeStampPassRev { get; set; }
        public DateTime? TimeStampDrivRev { get; set; }
        public string PassengerUserName { get; set; }
        public string DriverUserName { get; set; }
        public string PassengerReview { get; set; }
        public string DriverReview { get; set; }
        public Boolean PassengerIsReview { get; set; }
        public Boolean DriverIsReview { get; set; }
        public int? TripId { get; set; }
        public int Id { get; set; }
        public virtual Trip Trip { get; set; }
        //public virtual Passenger Passenger { get; set; }

        public TripReview()
        {
            PassengerIsReview = false;
            DriverIsReview = false;
        }


    }
}