using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RideSharing.Models
{
    public class Passenger
    {
        [Key]
        public int PassengerId { get; set; }
        public string PassengerIdentity { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public bool OnLine { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }

    }
}