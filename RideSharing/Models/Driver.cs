using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RideSharing.Models
{
    public class Driver
    {

        //test github
        

        public Driver()
        {
            OnLine = false;
            OnRide = false;
        }


        [Key]
        public int DriverId { get; set; }
        public string DriverIdentity { get; set; }
        public string Name { get; set; }
        public bool OnLine { get; set; }
        public bool OnRide { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }

    }
}