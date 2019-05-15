using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RideSharing.Models
{


    public enum TripStatus
    {

      
        Active=1,
        Accepted,
        Completed

    }

    public class Trip
    {
        public Trip()
        {
            TimeStamp = DateTime.Now;
            Status = TripStatus.Active;
        }

        [Key]
        public int TripId { get; set; }


        [Display(Name ="Desired Arrival Date/Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime TimeStamp { get; set; }

        [Display(Name = "Driver Commision")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "€{0:n} ")]
        public decimal Commission { get; set; }

        [Display(Name = "Origin Address")]
        public string OriginAddress { get; set; }

        [Display(Name = "Destination Address")]
        public string DestAddress { get; set; }

        public TripStatus Status { get; set; }

        public string DriverIdentity { get; set; }
        public string PassengerIdentity { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual Passenger Passenger { get; set; }

    

    }


    public class TripModelView
    {
        public TripModelView()
        {
            TimeStamp = DateTime.Now;
            Status = TripStatus.Active;
        }

        [Key]
        public int TripId { get; set; }


        [Display(Name = "Desired Arrival Date/Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime TimeStamp { get; set; }

        [Display(Name = "Driver Commision")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "€{0:n} ")]
        public decimal Commission { get; set; }

        [Display(Name = "Origin Address")]
        public string OriginAddress { get; set; }

        [Display(Name = "Destination Address")]
        public string DestAddress { get; set; }

        public TripStatus Status { get; set; }
        public string DriverIdentity { get; set; }
        public string PassengerIdentity { get; set; }
        public  TripReview TripReview { get; set; }


    }
}