using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RideSharing.Models
{
    public class Trip
    {
        public Trip()
        {
            TimeStamp = DateTime.Now;
            IsCompleted = false;
        }

        [Key]
        public int TripId { get; set; }
        public DateTime TimeStamp { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Total { get; set; }

        [Display(Name = "Driver Commision")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Commission { get; set; }

        [Display(Name = "Origin Address")]
        public string OriginAddress { get; set; }

        [Display(Name = "Destination Address")]
        public string DestAddress { get; set; }

        public bool IsCompleted { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual Passenger Passenger { get; set; }

    }
}